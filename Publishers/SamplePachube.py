import clr

from System import *
from System.Net import WebClient
from System.Xml import XmlDocument
from System.Diagnostics import Trace

url = "http://pachube.com/api/"
apiKey = "40ab667a92d6f892fef6099f38ad5eb31e619dffd793ff8842ae3b00eaf7d7cb"
environmentId = 2065

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
			