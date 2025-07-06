# main.py

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

import time
import os

app = Flask(__name__)

ocr_requested = Event()
last_detection_time = 0
detection_cooldown = 2  # seconds
last_detected_label = None
is_removal = False
token = None

# ------------------- Flask Endpoints -------------------

@app.route('/set-token', methods=['POST'])
def set_token():
    data = request.get_json()
    t = data.get('token')
    if not t:
        return {"error": "No token provided"}, 400
    try:
        with open("token.txt", 'w') as f:
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


# ---------------------- Main Logic ----------------------

def verify_weight(expected, actual, tolerance):
    return abs(actual - expected) <= tolerance


def weight_monitor(weight_sensor):
    print("[WeightMonitor] Started.")
    prev_weight = weight_sensor.get_weight()
    while True:
        time.sleep(0.5)
        curr_weight = weight_sensor.get_weight()
        delta = abs(curr_weight - prev_weight)

        if delta > 5.0 and time.time() - last_detection_time > 3:  # weight changed, but no detection recently
            print("[WeightMonitor] Weight mismatch detected!")
            # send_weight_mismatch_error_to_backend(curr_weight, delta)

        prev_weight = curr_weight


def camera_loop(cam, barcode_detector, weight_sensor):
    global last_detection_time, last_detected_label, is_removal

    while True:
        # Manual override for debugging
        try:
            if msvcrt.kbhit():
                ch = msvcrt.getch().decode('utf-8').lower()
                if ch == 'b':
                    ocr_requested.set()
                elif ch == 'c':
                    print("[Debug] Manual object detection triggered.")
        except:
            pass

        if ocr_requested.is_set():
            print("[Main] Running Barcode Scan Mode...")
            ocr_requested.clear()

            identifier = barcode_detector.scan_once(timeout=5)
            if identifier:
                print(f"[Barcode] Found: {identifier}")
                handle_detection(identifier, weight_sensor)
            else:
                print("[Barcode] No barcode found in 5 seconds.")

            continue  # loop back to object detection

        # Default: object detection
        result = cam.capture_and_detect(show_window=False)
        identifier, conf = cam.get_top_label(result)

        if not identifier:
            continue

        print(f"[Detect] Found: {identifier} ({conf*100:.1f}%)")
        last_detection_time = time.time()
        last_detected_label = identifier

        handle_detection(identifier, weight_sensor)

def handle_detection(identifier, weight_sensor):
    global is_removal

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

    print(f"[Detect] Waiting for weight. Expecting ~{expected}g")

    time.sleep(1)
    weight = weight_sensor.get_weight()
    if weight is None:
        print("[Detect] Failed to read weight.")
        return

    print(f"[Detect] Measured Weight: {weight:.2f}g")

    if verify_weight(expected, weight, tolerance):
        print("[Detect] Weight matched.")
        if is_removal:
            success = remove_product(product['productCode'], cart_id="1234")
            print("[Backend] Removed." if success else "[Backend] Failed to remove.")
        else:
            success = add_product(product['productCode'], cart_id="1234")
            print("[Backend] Added." if success else "[Backend] Failed to add.")
    else:
        print("[Detect] Weight mismatch.")


# ------------------- Main -------------------

def main():
    print("[Main] Starting Flask server...")
    Thread(target=start_flask, daemon=True).start()
    time.sleep(1)

    print("[Main] Initializing components...")
    cam_mgr = CameraManager()
    cam_mgr.add_config("barcode", cam_mgr.picam2.create_still_configuration(main={"format": "RGB888", "size": (3280, 2464)}))
    cam_mgr.add_config("detect", cam_mgr.picam2.create_preview_configuration(main={"format": "RGB888", "size": (640, 640)}))

    cam = CameraModule(cam_mgr, config_name="detect")
    barcode_detector = BarcodeModule(cam_mgr, config_name="barcode")
    weight_sensor = WeightSensor()

    print("[Main] Launching threads...")

    Thread(target=weight_monitor, args=(weight_sensor,), daemon=True).start()
    Thread(target=camera_loop, args=(cam, barcode_detector, weight_sensor), daemon=True).start()

    while True:
        time.sleep(1)


if __name__ == "__main__":
    main()