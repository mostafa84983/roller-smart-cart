import requests

API_BASE_URL = "http://localhost:5138/api/product/code"

def get_product_by_code(product_code):
    try:
        response = requests.get(f"{API_BASE_URL}/{product_code}")
        response.raise_for_status()
        data = response.json()
        print(f"[Product API] Fetched product {product_code}: {data}")
        return data
    except requests.exceptions.RequestException as e:
        print(f"[Product API] Failed to fetch product {product_code}: {e}")
        return None
