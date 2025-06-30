# barcode_module.py

import cv2

class BarcodeModule:
    def __init__(self, cam_manager, config_name):
        print(f"Initializing BarcodeModule with config_name: {config_name}")
        self.cam_manager = cam_manager
        self.config_name = config_name
        self.detector = cv2.barcode_BarcodeDetector()

    def scan_once(self):
        print("BarcodeModule: Capturing frame")
        frame = self.cam_manager.capture(self.config_name)

        # Crop center region
        crop_w, crop_h = 1280, 960
        full_h, full_w = frame.shape[:2]
        x1 = (full_w - crop_w) // 2
        y1 = (full_h - crop_h) // 2
        cropped = frame[y1:y1+crop_h, x1:x1+crop_w]

        print("BarcodeModule: Running detectAndDecodeMulti")
        ok, decoded_infos, _, _ = self.detector.detectAndDecodeMulti(cropped)
        if ok and decoded_infos:
            for code in decoded_infos:
                if code:
                    print(f"BarcodeModule: Detected barcode: {code}")
                    return str(code).strip()
        print("BarcodeModule: No barcode detected")
        return None

