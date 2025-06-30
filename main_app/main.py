# main.py

from threading import Thread
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

# ---------------------- Flask Server ----------------------
app = Flask(__name__)

@app.route('/set-token', methods=['POST'])
def set_token():
    data = request.get_json()
    token = data.get('token')
    if not token:
        return {"error": "No token provided"}, 400

    try:
        with open(TOKEN_FILE, 'w') as f:  # 'w' mode truncates the file
            f.write(token.strip())
        return {"message": "Token saved and file truncated"}, 200
    except Exception as e:
        return {"error": str(e)}, 500

@app.route('/health', methods=['GET'])
def health():
    return {"status": "ok"}, 200


def start_flask():
    app.run(host='0.0.0.0', port=5050)


# ---------------------- Main Logic ----------------------

def verify_weight(expected, actual, tolerance):
    return abs(actual - expected) <= tolerance


def main():
    print("Starting token server in background...")
    Thread(target=start_flask, daemon=True).start()
    time.sleep(1)  # Allow Flask server to start

    print("Initializing components...")
    cam_mgr = CameraManager()
    cam_mgr.add_config("barcode", cam_mgr.picam2.create_still_configuration(main={"format": "RGB888", "size": (3280, 2464)}))
    cam_mgr.add_config("detect", cam_mgr.picam2.create_preview_configuration(main={"format": "RGB888", "size": (640, 640)}))

    cam = CameraModule(cam_mgr, config_name="detect")
    barcode_detector = BarcodeModule(cam_mgr, config_name="barcode")
    weight_sensor = WeightSensor()
    
    print("System ready.\nPress Enter to detect object and add to cart.\nPress 'r' to remove item.\nPress 'b' to scan barcode.\nPress 'q' to quit.")

    try:
        while True:
            isRemoveProduct = False
            user_input = input("> ").strip().lower()

            if user_input == 'q':
                break

            token = get_token()
            if not token:
                print("No token. User must log in first.")
                continue

            if user_input == 'b':
                identifier = barcode_detector.scan_once()
                if not identifier:
                    print("No barcode detected.")
                    continue
                print(f"Detected barcode: {identifier}")

            elif user_input == 'r' or user_input == '':
                # 'r' means remove; empty string (Enter) means add
                isRemoveProduct = user_input == 'r'

                result = cam.capture_and_detect(show_window=False)
                identifier, conf = cam.get_top_label(result)

                if not identifier:
                    print("No product detected.")
                    continue
                print(f"Detected: {identifier} ({conf*100:.1f}%)")

            else:
                print("Invalid input.")
                continue


            label, product_code, product_info = get_product_info(identifier)

            if not product_info:
                print(f"'{identifier}' not recognized in label map.")
                continue

            product = get_product_by_code(product_code)
            if not product:
                print("Product not found in DB.")
                continue

            expected = float(product['productWeight'])
            tolerance = expected * 0.10

            print(f"Expected Weight: {expected} ± {tolerance} g")

            weight = weight_sensor.get_weight()
            if weight is None:
                print("Failed to read weight.")
                continue

            print(f"Measured Weight: {weight:.2f} g")

            if verify_weight(expected, weight, tolerance):
                print("Item accepted.")
                if isRemoveProduct:
                    print("Removing product from cart...")
                    success = remove_product(product['productCode'], cart_id="1234")
                    print("Removed from backend." if success else "Failed to remove from backend.")
                else:
                    print("Adding product to cart...")
                    success = add_product(product['productCode'], cart_id="1234")
                    print("Sent to backend." if success else "Failed to send to backend.")
            else:
                print("Weight mismatch. Item rejected.")

    finally:
        print("Shutting down...")
        weight_sensor.close()
        cam.release()


if __name__ == "__main__":
    main()
