# cart_api.py
import requests

API_ADD = "http://localhost:5138/api/product/Add/Product"
API_REMOVE = "http://localhost:5138/api/product/Remove/Product"

TOKEN_FILE = "/tmp/cart_jwt.txt"

def get_token():
    try:
        with open(TOKEN_FILE, 'r') as f:
            return f.read().strip()
    except:
        return None

def add_product(product_code: int, cart_id: str = "1234"):
    token = get_token()
    if not token:
        print("[Cart API] No token available.")
        return False

    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }

    payload = {
        "productCode": product_code,
        "cartId": cart_id
    }

    try:
        response = requests.post(API_ADD, json=payload, headers=headers)
        if response.status_code == 200:
            print("[Cart API] Product successfully added to cart.")
            print(response.text)
            return True
        else:
            print(f"[Cart API] Failed to add product. Status: {response.status_code}")
            print(response.text)
            return False
    except requests.RequestException as e:
        print(f"[Cart API] Network error: {e}")
        return False


def remove_product(product_code: int, cart_id: str = "1234"):
    token = get_token()
    if not token:
        print("[Cart API] No token available.")
        return False

    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }

    payload = {
        "productCode": product_code,
        "cartId": cart_id
    }

    try:
        response = requests.post(API_REMOVE, json=payload, headers=headers)
        if response.status_code == 200:
            print("[Cart API] Product successfully removed from cart.")
            print(response.text)
            return True
        else:
            print(f"[Cart API] Failed to remove product. Status: {response.status_code}")
            print(response.text)
            return False
    except requests.RequestException as e:
        print(f"[Cart API] Network error: {e}")
        return False
