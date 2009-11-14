using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using Sspe;

namespace Sspe.Service
{
    [Description("Analyses sensor and telemetry information and publishes it to an MQTT message broker")]
    public partial class Service : ServiceBase
    {
        private Engine engine;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            engine = new Engine();
            engine.Start();
        }

        protected override void OnStop()
        {
            engine.Stop();
        }

        protected override void OnPause()
        {
            engine.Pause();
        }

        protected override void OnContinue()
        {
            engine.Continue();
        }
    }
}
