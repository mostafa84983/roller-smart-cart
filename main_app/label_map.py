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

def get_label_by_barcode(barcode):
    for label, info in LABEL_MAP.items():
        if str(info["barcode"]) == str(barcode):
            return label
    return None
