import clr
from System import *
from System.IO.Ports import *

stopping = False
cc128 = SerialPort("COM3", 57600)

def Run(sensorBridge):
    cc128.Open()
    while not stopping:
        readData = cc128.ReadLine()
        sensorBridge.PublishAscii("PowerMeter/CC128/Mark", readData)

    return 1;

def Stop():
    cc128.Close()
    stopping = True
    return 1
