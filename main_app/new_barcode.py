from picamera2 import Picamera2
import cv2
import numpy as np
import time

def main():
    detector = cv2.barcode_BarcodeDetector()
    print("OpenCV Barcode Detector using PiCamera2 (1920x1080 headless) active. Press Ctrl+C to stop.")

    picam2 = Picamera2()
    config = picam2.create_still_configuration(main={"format": "RGB888", "size": (1920, 1080)})
    picam2.configure(config)
    picam2.start()
    time.sleep(1)  # Let the sensor warm up

    try:
        while True:
            frame = picam2.capture_array()

            try:
                ok, decoded_infos, points, _ = detector.detectAndDecodeMulti(frame)

                if ok and decoded_infos is not None:
                    for s in decoded_infos:
                        if isinstance(s, (bytes, bytearray)):
                            s = s.decode('utf-8', errors='ignore')
                        s = str(s).strip()
                        if s:
                            print("Detected barcode:", s)

            except Exception as e:
                print("Detection error:", e)

            # Short delay to reduce CPU usage
            time.sleep(0.05)

    except KeyboardInterrupt:
        print("Shutting down...")

    finally:
        picam2.stop()

if __name__ == '__main__':
    main()
