class BarcodeModule:
    def __init__(self):
        from picamera2 import Picamera2
        import cv2
        self.detector = cv2.barcode_BarcodeDetector()
        self.picam2 = Picamera2()
        config = self.picam2.create_still_configuration(main={"format": "RGB888", "size": (3280, 2464)})
        self.picam2.configure(config)
        self.picam2.set_controls({"Sharpness": 1.0, "Contrast": 1.0})
        self.picam2.start()
        time.sleep(1)

    def scan_once(self):
        frame = self.picam2.capture_array()

        # Center crop
        crop_w, crop_h = 1280, 960
        full_h, full_w = frame.shape[:2]
        x1 = (full_w - crop_w) // 2
        y1 = (full_h - crop_h) // 2
        cropped = frame[y1:y1+crop_h, x1:x1+crop_w]

        ok, decoded_infos, _, _ = self.detector.detectAndDecodeMulti(cropped)
        if ok and decoded_infos:
            for code in decoded_infos:
                if code:
                    return str(code).strip()
        return None

    def close(self):
        self.picam2.stop()
