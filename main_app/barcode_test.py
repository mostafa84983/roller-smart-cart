from picamera2 import Picamera2
import cv2
import numpy as np
import time

def main():
    detector = cv2.barcode_BarcodeDetector()
    print("High-Res Barcode Detector with Cropping (5s cooldown). Press 'q' to quit.")

    picam2 = Picamera2()
    # Use full 5MP resolution
    config = picam2.create_still_configuration(main={"format": "RGB888", "size": (3280, 2464)})
    picam2.configure(config)
    picam2.set_controls({"Sharpness": 1.0, "Contrast": 1.0})
    picam2.start()
    time.sleep(1)

    cooldowns = {}  # barcode_text -> last_seen_timestamp

    try:
        while True:
            current_time = time.time()
            frame = picam2.capture_array()

            # Define a center crop area
            crop_w, crop_h = 1280, 960  # 720p area
            full_h, full_w = frame.shape[:2]
            x1 = (full_w - crop_w) // 2
            y1 = (full_h - crop_h) // 2
            cropped = frame[y1:y1+crop_h, x1:x1+crop_w]

            ok, decoded_infos, points, _ = detector.detectAndDecodeMulti(cropped)

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
                        # Adjust points from crop to full frame for visualization
                        pts += np.array([x1, y1])
                        cv2.polylines(frame, [pts], True, (0, 255, 0), 2)
                        cv2.putText(frame, s, tuple(pts[0]), cv2.FONT_HERSHEY_SIMPLEX, 0.6, (0, 255, 0), 2)

            # Resize full frame for display
            preview = cv2.resize(frame, (640, 480))
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
