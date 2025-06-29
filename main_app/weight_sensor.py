import serial
import time

class WeightSensor:
    def __init__(self, port="/dev/ttyUSB0", baudrate=115200):
        self.ser = serial.Serial(port, baudrate, timeout=2)
        time.sleep(2)

    def get_weight(self):
        try:
            self.ser.reset_input_buffer()  # Flush old/stale data
            self.ser.write(b'R')           # Send request
            line = self.ser.readline().decode("utf-8").strip()
            if line:
                return float(line)
        except (ValueError, serial.SerialException):
            pass
        return None

    def close(self):
        self.ser.close()

