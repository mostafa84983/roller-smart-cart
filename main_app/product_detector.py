# product_detector.py

def get_top_label(result):
    if len(result.boxes) == 0:
        return None, 0.0

    # Get the most confident box
    best_box = result.boxes[0]
    class_id = int(best_box.cls[0])
    confidence = float(best_box.conf[0])
    label = result.names[class_id]

    return label, confidence
