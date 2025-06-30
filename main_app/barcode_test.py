from picamera2 import Picamera2
import cv2
import numpy as np
import time

def main():
    detector = cv2.barcode_BarcodeDetector()
    print("Barcode Detector with Preview (1920x1080, 5s cooldown). Press 'q' to quit.")

    picam2 = Picamera2()
    config = picam2.create_video_configuration(main={"format": "RGB888", "size": (1920, 1080)})
    picam2.configure(config)
    picam2.set_controls({"Sharpness": 1.0, "Contrast": 1.0})  # Optional tuning
    picam2.start()
    time.sleep(1)

    cooldowns = {}  # barcode_text -> last_seen_timestamp (float)

    try:
        while True:
            current_time = time.time()
            frame = picam2.capture_array()
            ok, decoded_infos, points, _ = detector.detectAndDecodeMulti(frame)

            if ok and decoded_infos and points is not None:
                for s, pts in zip(decoded_infos, points):
                    if isinstance(s, (bytes, bytearray)):
                        s = s.decode('utf-8', errors='ignore')
                    s = str(s).strip()

                    if s:
                        last_seen = cooldowns.get(s, 0)
                        if current_time - last_seen >= 5:
                            cooldowns[s] = current_time
                            print("Detected barcode:", s)

                        pts = np.int32(pts).reshape(-1, 2)
                        cv2.polylines(frame, [pts], True, (0, 255, 0), 2)
                        cv2.putText(frame, s, tuple(pts[0]), cv2.FONT_HERSHEY_SIMPLEX, 0.6, (0, 255, 0), 2)

            preview = cv2.resize(frame, (640, 360))
            cv2.imshow("Barcode Detection", preview)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

    except KeyboardInterrupt:
        print("Interrupted by user.")

    finally:
        picam2.stop()
        cv2.destroyAllWindows()

if __name__ == '__main__':
    main()
