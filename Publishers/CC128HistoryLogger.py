import clr

from System import *
from System.Xml import *
from System.IO import *
from System.Diagnostics import *

historyWriter = StreamWriter('.\Flat9SansSouciCC128.csv', True)

def Publish(topic, data):
    watts, temp = ExtractData(data)

    if watts is None:
        return 0

    Trace.WriteLine("AboutToLog")

    historyWriter.WriteLine(String.Format('{0:s},{1},{2}', DateTime.Now, watts, temp))
    return 1
	
def ExtractData(data):
    cc128 = XmlDocument()
    dataStream = MemoryStream(data)
    cc128.Load(dataStream)

    isHist = cc128.SelectSingleNode('/msg/hist')    
    if (isHist):
        return None, None

    watts = cc128.SelectSingleNode('/msg/ch1/watts').InnerText
    temp = cc128.SelectSingleNode('/msg/tmpr').InnerText
    dataStream.Dispose()
    return int(watts), temp
	
def Shutdown():
    historyWriter.Flush()
    historyWriter.Dispose()
    return 1

def GetTopics():
    return ["PowerMeter/CC128/Mark"]
			