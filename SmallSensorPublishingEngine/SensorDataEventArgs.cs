using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sspe
{
    public class SensorDataEventArgs : EventArgs
    {
        public byte[] SensorData { get; set; }
        public string Topic { get; set; }

        public SensorDataEventArgs(string topic, byte[] data)
        {
            this.Topic = topic;
            this.SensorData = data;
        }
    }
}
