# camera_manager.py

import time
from picamera2 import Picamera2

class CameraManager:
    def __init__(self):
        self.picam2 = Picamera2()
        self.configs = {}
        self.current_config_name = None
        self.started = False

    def add_config(self, name, config):
        self.configs[name] = config

    def switch(self, name):
        if self.current_config_name != name:
            config = self.configs[name]
            self.picam2.switch_mode_and_configure(config)
            self.current_config_name = name
            time.sleep(0.1)

        if not self.started:
            self.picam2.start()
            self.started = True

    def capture(self, config_name):
        self.switch(config_name)
        return self.picam2.capture_array()

    def close(self):
        if self.started:
            self.picam2.stop()
            self.picam2.close()
