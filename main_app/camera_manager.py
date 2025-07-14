# camera_manager.py

import time
from picamera2 import Picamera2

class CameraManager:
    def __init__(self):
        print("[CameraManager] Initializing CameraManager")
        self.picam2 = Picamera2()
        print("[CameraManager] Picamera2 initialized")
        self.configs = {}
        self.current_config_name = None
        self.started = False

    def add_config(self, name, config):
        self.configs[name] = config

    def switch(self, name):
        if self.current_config_name != name:
            print(f"[CameraManager] Switching to '{name}' camera configuration")
            config = self.configs[name]

            if self.started:
                print("[CameraManager] Stopping camera")
                self.picam2.stop()
                self.started = False

            print("[CameraManager] Starting camera")
            self.picam2.configure(config)
            self.picam2.start()
            self.started = True
            self.current_config_name = name
            time.sleep(0.1)

        elif not self.started:
            print("[CameraManager] Starting camera")
            self.picam2.start()
            self.started = True

    def capture(self, config_name):
        print(f"[CameraManager] Capturing image with {config_name} camera configuration")
        self.switch(config_name)
        return self.picam2.capture_array()

    def close(self):
        if self.started:
            print("[CameraManager] Stopping camera")
            self.picam2.stop()
            self.picam2.close()
