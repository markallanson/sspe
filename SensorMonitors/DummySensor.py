import clr
from System import *
from System.Threading import Thread;

stopping = False

def Run(sensorBridge):
    while not stopping:
        sensorBridge.PublishAscii('PowerMeter/CC128/Mark', 'Dummy Sensor')
        Thread.Sleep(6000)
    return 1
	
def Stop():
    Console.WriteLine("Stopping")
    stopping = True
    return 1
