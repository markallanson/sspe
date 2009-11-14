import clr

from System import *
from System.Net import WebClient
from System.Xml import XmlDocument
from System.IO import MemoryStream
from System.Diagnostics import Trace

url = "http://pachube.com/api/"
apiKey = "40ab667a92d6f892fef6099f38ad5eb31e619dffd793ff8842ae3b00eaf7d7cb"
environmentId = 2058

def Publish(topic, data):
    client = WebClient()
    client.Headers.Add('X-PachubeApiKey', apiKey)

    watts, temp = ExtractData(data)
    if watts is None:
        return 0

    Trace.WriteLine("cc128->Pachube(watts= " + str(watts) + ", temp=" + temp + ")")

    resp = client.UploadString(CreateFullUrl(), "PUT", str(watts) + "," + str(temp))
    client.Dispose();
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
	
def CreateFullUrl():
    return url + str(environmentId) + '.csv'

def Shutdown():
    return 1

def GetTopics():
    return ["PowerMeter/CC128/Mark"]		