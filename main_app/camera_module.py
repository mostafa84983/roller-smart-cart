# camera_module.py

import cv2
from picamera2 import Picamera2
from ultralytics import YOLO

class CameraModule:
    def __init__(self, model_path="my_model_ncnn_model", resolution=(1280, 1280), imgsz=480):
        self.model = YOLO(model_path)
        self.imgsz = imgsz
        self.picam2 = Picamera2()

        self.picam2.preview_configuration.main.size = resolution
        self.picam2.preview_configuration.main.format = "RGB888"
        self.picam2.preview_configuration.align()
        self.picam2.configure("preview")
        self.picam2.start()

    def capture_and_detect(self, show_window=False):
        frame = self.picam2.capture_array()
        results = self.model.predict(frame, imgsz=self.imgsz)
        result = results[0]

        if show_window:
            annotated = result.plot()
            cv2.imshow("Detection", annotated)
            # if cv2.waitKey(1) == ord("q"):
            #     cv2.destroyAllWindows()

        return result

    def get_top_label(result):
        if len(result.boxes) == 0:
            return None, 0.0

        # Get the most confident box
        best_box = result.boxes[0]
        class_id = int(best_box.cls[0])
        confidence = float(best_box.conf[0])
        label = result.names[class_id]

        return label, confidence
    
    def release(self):
        self.picam2.stop()
        self.picam2.close()
        cv2.destroyAllWindows()