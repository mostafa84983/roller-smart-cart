from threading import Thread
from flask import Flask, request
from camera_module import CameraModule
# from product_detector import get_top_label
from product_api import get_product_by_code
from weight_sensor import WeightSensor
from cart_api import add_product, get_token, TOKEN_FILE
from label_map import LABEL_TO_CODE


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

    time.sleep(1)  # Give Flask a moment to start

    print("Initializing components...")
    cam = CameraModule()
    weight_sensor = WeightSensor()

    print("System ready. Press Enter to detect product, or 'q' + Enter to quit.\n")

    try:
        while True:
            user_input = input("> ")
            if user_input.lower() == 'q':
                break

            token = get_token()
            if not token:
                print("No token. User must log in first.")
                continue

            result = cam.capture_and_detect(show_window=False)
            label, conf = cam.get_top_label(result)

            if not label:
                print("No product detected.")
                continue

            print(f"Detected: {label} ({conf*100:.1f}%)")

            product_code = LABEL_TO_CODE.get(label)
            if not product_code:
                print(f"Unrecognized label '{label}'. No product code mapping.")
                continue


            product = get_product_by_code(product_code)
            print(product.keys())


            if not product:
                print("Product not found in DB.")
                continue

            expected = float(product['productWeight'])
            tolerance = expected * 0.10  # 10% tolerance

            print(f"Expected Weight: {expected} ± {tolerance} g")

            weight = weight_sensor.get_weight()
            if weight is None:
                print("Failed to read weight.")
                continue

            print(f"Measured Weight: {weight:.2f} g")

            if verify_weight(expected, weight, tolerance):
                print("Item accepted.")
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
