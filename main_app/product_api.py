import requests

productCode_url = "http://localhost:5138/api/product/code"
weightMismatch_url = "http://localhost:5138/api/product/Failed/Product"

def get_product_by_code(product_code):
    try:
        response = requests.get(f"{productCode_url}/{product_code}")
        response.raise_for_status()
        data = response.json()
        print(f"[Product API] Fetched product {product_code}: {data}")
        return data
    except requests.exceptions.RequestException as e:
        print(f"[Product API] Failed to fetch product {product_code}: {e}")
        return None
