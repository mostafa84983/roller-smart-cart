# camera_module.py

import cv2
from ultralytics import YOLO

class CameraModule:
    def __init__(self, cam_manager, config_name, model_path="my_model_ncnn_model", imgsz=480):
        print("[CameraModule] Initializing with configuration:", config_name)
        self.cam_manager = cam_manager
        self.config_name = config_name
        self.model = YOLO(model_path)
        self.imgsz = imgsz

    def capture_and_detect(self, show_window=False):
        print("[CameraModule] Capturing and detecting...")
        frame = self.cam_manager.capture(self.config_name)
        results = self.model.predict(frame, imgsz=self.imgsz)
        result = results[0]

        if show_window:
            annotated = result.plot()
            annotated = cv2.resize(annotated, (640, 480))
            cv2.imshow("Detection", annotated)
            cv2.waitKey(1)

        return result

    def get_top_label(self, result, min_conf=0.5):
        print("[CameraModule] Getting top label with minimum confidence:", min_conf)
        if len(result.boxes) == 0:
            print("[CameraModule] No boxes detected")
            return None, 0.0

        best_box = sorted(result.boxes, key=lambda box: float(box.conf[0]), reverse=True)[0]
        confidence = float(best_box.conf[0])
        if confidence < min_conf:
            print("[CameraModule] No label meets the minimum confidence")
            return None, confidence

        class_id = int(best_box.cls[0])
        label = result.names[class_id]
        print("[CameraModule] Top label:", label, "with confidence:", confidence)
        return label, confidence

    def release(self):
        print("[CameraModule] Releasing resources and closing windows")
        cv2.destroyAllWindows()
