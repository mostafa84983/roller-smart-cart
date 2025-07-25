# main.py

import time
import os
import threading
import requests

from threading import Thread, Event
from flask import Flask, request
from picamera2 import Picamera2
from camera_manager import CameraManager
from camera_module import CameraModule
from barcode_module import BarcodeModule
from product_api import get_product_by_code
from weight_sensor import WeightSensor
from cart_api import add_product, remove_product, get_token, TOKEN_FILE
from label_map import get_product_info

# Constants
CART_ID = "1234"
WEIGHT_MISMATCH_URL = "http://localhost:5138/api/product/Failed/Product"

# Threading state
ocr_requested = Event()
last_detection_time = 0
last_expected_weight = 0.0  # weight expected from last detected product
state_lock = threading.Lock()
product = None  # currently detected product

app = Flask(__name__)

# ------------------- Flask Endpoints -------------------
@app.route('/set-token', methods=['POST'])
def set_token():
    data = request.get_json()
    t = data.get('token')
    if not t:
        return {"error": "No token provided"}, 400
    try:
        with open(TOKEN_FILE, 'w') as f:
            f.write(t.strip())
        return {"message": "Token saved"}, 200
    except Exception as e:
        return {"error": str(e)}, 500

@app.route('/open-OCR', methods=['POST'])
def open_ocr():
    print("[Flask] Barcode fallback requested.")
    ocr_requested.set()
    return {"status": "ok"}, 200

@app.route('/health', methods=['GET'])
def health():
    return {"status": "ok"}, 200

def start_flask():
    app.run(host='0.0.0.0', port=5050)

# ---------------------- Helper Functions ----------------------
def verify_weight(expected, actual, tolerance):
    return abs(actual - expected) <= tolerance


def report_weight_mismatch(actual_delta):
    payload = {"cartId": CART_ID}
    try:
        requests.post(WEIGHT_MISMATCH_URL, json=payload)
        print(f"[WeightMonitor] Reported mismatch: {actual_delta:.2f}g")
    except Exception as e:
        print(f"[WeightMonitor] Failed to report mismatch: {e}")

# ---------------------- Threads ----------------------
def weight_monitor(weight_sensor):
    global last_expected_weight
    print("[WeightMonitor] Started.")
    prev_weight = weight_sensor.get_weight() or 0.0

    while True:
        time.sleep(0.5)
        curr_weight = weight_sensor.get_weight() or prev_weight
        delta = curr_weight - prev_weight

        with state_lock:
            expected = last_expected_weight

        if abs(delta) > 50.0:
            # If the change matches expected weight -> add/remove
            if abs(abs(delta) - expected) <= expected * 0.10:
                if delta > 0:
                    print(f"[WeightMonitor] Detected addition: +{delta:.2f}g matches expected {expected:.2f}g")
                    success = add_product(product['productCode'], cart_id="1234")
                    print("[Backend] Added." if success else "[Backend] Failed to add.")
                else:
                    print(f"[WeightMonitor] Detected removal: {delta:.2f}g matches expected {expected:.2f}g")
                    success = remove_product(product['productCode'], cart_id="1234")
                    print("[Backend] Removed." if success else "[Backend] Failed to remove.")
            else:
                # Mismatch
                print(f"[WeightMonitor] Weight mismatch: delta={delta:.2f}g expected={expected:.2f}g")
                report_weight_mismatch(delta)

            # reset expected
            with state_lock:
                last_expected_weight = 0.0

        prev_weight = curr_weight


def camera_loop(cam, barcode_detector, weight_sensor):
    global last_detection_time, last_expected_weight

    while True:
        # Default: object detection or OCR fallback
        if ocr_requested.is_set():
            print("[Main] Running Barcode Scan Mode...")
            ocr_requested.clear()

            identifier = barcode_detector.scan_once(timeout=5)
            if identifier:
                print(f"[Barcode] Found: {identifier}")
                process_detection(identifier)
            else:
                print("[Barcode] No barcode found in 5 seconds.")

            continue

        # Object detection
        result = cam.capture_and_detect(show_window=False)
        identifier, conf = cam.get_top_label(result)

        if not identifier:
            continue

        print(f"[Detect] Found: {identifier} ({conf*100:.1f}%)")
        last_detection_time = time.time()
        process_detection(identifier)


def process_detection(identifier):
    global last_expected_weight, product

    label, product_code, product_info = get_product_info(identifier)
    if not product_info:
        print(f"[Detect] Unknown label: {identifier}")
        return

    product = get_product_by_code(product_code)
    if not product:
        print("[Detect] Not found in DB.")
        return

    expected = float(product['productWeight'])
    tolerance = expected * 0.10

    print(f"[Detect] Expected weight: {expected:.2f}g ±{tolerance:.2f}g")

    # store for weight monitor
    with state_lock:
        last_expected_weight = expected

# ------------------- Main -------------------
def main():
    print("[Main] Starting Flask server...")
    Thread(target=start_flask, daemon=True).start()
    time.sleep(1)

    print("[Main] Initializing components...")
    cam_mgr = CameraManager()
    cam_mgr.add_config("barcode", cam_mgr.picam2.create_still_configuration(
        main={"format": "RGB888", "size": (3280, 2464)}))
    cam_mgr.add_config("detect", cam_mgr.picam2.create_preview_configuration(
        main={"format": "RGB888", "size": (640, 640)}))

    cam = CameraModule(cam_mgr, config_name="detect")
    barcode_detector = BarcodeModule(cam_mgr, config_name="barcode")
    weight_sensor = WeightSensor()

    print("[Main] Launching threads...")
    Thread(target=weight_monitor, args=(weight_sensor,), daemon=True).start()
    Thread(target=camera_loop, args=(cam, barcode_detector, weight_sensor), daemon=True).start()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("[Main] Shutting down...")
        weight_sensor.close()
        cam.release()


if __name__ == "__main__":
    main()
