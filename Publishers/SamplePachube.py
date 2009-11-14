import clr

from System import *
from System.Net import WebClient
from System.Xml import XmlDocument
from System.Diagnostics import Trace

url = "http://pachube.com/api/"
apiKey = "<Your-Pachube-Api-Key-Here>"
environmentId = -1

def Publish(topic, data):
    ms = MemoryStream()
    Trace.WriteLine("Pachube Sample")
    client = WebClient()
    client.Headers.Add('X-PachubeApiKey', apiKey)
    watts, temp = 25, 44
    resp = client.UploadString(CreateFullUrl(), "PUT", str(watts) + "," + str(temp))
    client.Dispose();
    return 1
	
def CreateFullUrl():
    return url + str(environmentId) + '.csv'

def Shutdown():
    return 1

def GetTopics():
    return ["PowerMeter/CC128/Mark"]
			