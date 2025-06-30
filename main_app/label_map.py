# label_map.py

LABEL_MAP = {
    "Bawady_Halawa": {
        "productCode": 1009,
        "barcode": "6223000760062"
    },
    "Arousa_Tea": {
        "productCode": 1001,
        "barcode": "6222002300153"
    },
    "Juhayna_ Guava": {
        "productCode": 1012,
        "barcode": "6222014330513"
    },
    "Dasany_Water": {
        "productCode": 1003,
        "barcode": "87303322"
    }
}

def get_product_info(label_or_barcode):
    # Direct label match
    if label_or_barcode in LABEL_MAP:
        info = LABEL_MAP[label_or_barcode]
        return label_or_barcode, info["productCode"], info

    # Try to match by barcode
    for label, info in LABEL_MAP.items():
        if str(info.get("barcode")) == str(label_or_barcode):
            return label, info["productCode"], info

    return None, None, None
