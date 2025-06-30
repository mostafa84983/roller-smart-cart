# camera_module.py

import cv2
from ultralytics import YOLO

class CameraModule:
    def __init__(self, cam_manager, config_name, model_path="my_model_ncnn_model", imgsz=480):
        self.cam_manager = cam_manager
        self.config_name = config_name
        self.model = YOLO(model_path)
        self.imgsz = imgsz

    def capture_and_detect(self, show_window=False):
        frame = self.cam_manager.capture(self.config_name)
        results = self.model.predict(frame, imgsz=self.imgsz)
        result = results[0]

        if show_window:
            annotated = result.plot()
            annotated = cv2.resize(annotated, (640, 480))
            cv2.imshow("Detection", annotated)
            cv2.waitKey(1)

        return result

    def get_top_label(self, result, min_conf=0.2):
        if len(result.boxes) == 0:
            return None, 0.0

        best_box = sorted(result.boxes, key=lambda box: float(box.conf[0]), reverse=True)[0]
        confidence = float(best_box.conf[0])
        if confidence < min_conf:
            return None, confidence

        class_id = int(best_box.cls[0])
        label = result.names[class_id]
        return label, confidence

    def release(self):
        cv2.destroyAllWindows()
