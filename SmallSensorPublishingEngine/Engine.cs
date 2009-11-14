using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nmqtt;
using System.Diagnostics;

namespace Sspe
{
    public sealed class Engine : IDisposable
    {
        private SensorManager sensorManager = null;
        private PublishingManager publishingManager = null;
        
        public bool IsStarted { get; set; }
        public bool IsPaused { get; set; }

        public void Start()
        {
            MqttClientManager.Connect();
            publishingManager = new PublishingManager();
            sensorManager = new SensorManager();
            sensorManager.Start();

            IsStarted = true;
        }

        public void Stop()
        {
            Trace.Flush();

            if (sensorManager != null)
            {
                sensorManager.Stop();
            }

            if (publishingManager != null)
            {
                publishingManager.Shutdown();
            }

            MqttClientManager.Disconnect();
            IsStarted = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Continue()
        {
            IsPaused = false;
        }

        #region IDisposable Members

        public void Dispose()
        {
            // TODO: Shutdown and cleanup all sensor plugins.

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
