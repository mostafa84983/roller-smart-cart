from threading import Thread
from flask import Flask, request
from camera_module import CameraModule
from barcode_module import BarcodeModule
from product_api import get_product_by_code
from weight_sensor import WeightSensor
from cart_api import add_product, remove_product, get_token, TOKEN_FILE
from label_map import LABEL_MAP, get_label_by_barcode


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
    cam = CameraModule()
    weight_sensor = WeightSensor()
    barcode_detector = BarcodeModule()  # New barcode scanner wrapper class
    print("System ready.\nPress Enter to detect object and add to cart.\nPress 'r' to remove item.\nPress 'b' to scan barcode.\nPress 'q' to quit.")

    try:
        while True:
            isRemoveProduct = False
            useBarcode = False
            user_input = input("> ").strip().lower()

            if user_input == 'q':
                break
            elif user_input == 'r':
                isRemoveProduct = True
            elif user_input == 'b':
                useBarcode = True

            token = get_token()
            if not token:
                print("No token. User must log in first.")
                continue

            if useBarcode:
                barcode = barcode_detector.scan_once()
                if not barcode:
                    print("No barcode detected.")
                    continue
                print(f"Detected barcode: {barcode}")

                label = get_label_by_barcode(barcode)
                if not label:
                    print("Barcode not recognized.")
                    continue

                print(f"Matched label: {label}")
            else:
                result = cam.capture_and_detect(show_window=False)
                label, conf = cam.get_top_label(result)

                if not label:
                    print("No product detected.")
                    continue

                print(f"Detected: {label} ({conf*100:.1f}%)")

            product_info = LABEL_MAP.get(label)
            if not product_info:
                print(f"Label '{label}' not mapped to product info.")
                continue

            product_code = product_info["productCode"]
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
        barcode_detector.close()  # Optional if your barcode module has cleanup


if __name__ == "__main__":
    main()
