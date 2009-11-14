using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sspe
{
    /// <summary>
    /// Provides a bridge between sensor scripts and the engine by hosting an event
    /// </summary>
    public class SensorBridge
    {
        /// <summary>
        /// Event that is fired when sensor data is available.
        /// </summary>
        public event EventHandler<SensorDataEventArgs> SensorDataAvailable;

        /// <summary>
        /// Configures the specified sensor reading callback.
        /// </summary>
        /// <param name="sensorReadingCallback">The sensor reading callback.</param>
        public void PublishRaw(string topic, byte[] data)
        {
            if (SensorDataAvailable != null)
            {
                SensorDataAvailable(this, new SensorDataEventArgs(topic, data));
            }
        }

        public void PublishAscii(string topic, string data)
        {
            if (SensorDataAvailable != null)
            {
                SensorDataAvailable(this, new SensorDataEventArgs(topic, System.Text.Encoding.ASCII.GetBytes(data)));
            }
        }

        public void PublishUtf8(string topic, string data)
        {
            if (SensorDataAvailable != null)
            {
                SensorDataAvailable(this, new SensorDataEventArgs(topic, System.Text.Encoding.UTF8.GetBytes(data)));
            }
        }

        public void PublishInt32(string topic, int data)
        {
            if (SensorDataAvailable != null)
            {
                var byteData = new [] 
                    {
                        (byte) (data << 32),
                        (byte) (data << 16),
                        (byte) (data << 8),
                        (byte) data
                    };
                SensorDataAvailable(this, new SensorDataEventArgs(topic, byteData));
            }
        }

        public void PublishInt16(string topic, int data)
        {
            if (SensorDataAvailable != null)
            {
                var byteData = new[] 
                    {
                        (byte) (data << 8),
                        (byte) data
                    };
                SensorDataAvailable(this, new SensorDataEventArgs(topic, byteData));
            }
        }
    }
}
