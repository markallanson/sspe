using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace Sspe.Service
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();

            System.ServiceProcess.ServiceInstaller installer = new System.ServiceProcess.ServiceInstaller()
            {
                ServiceName = "SSPEService",
                Description = "Publishes sensor information to a Message Queue Telemetry Transport broker",
                StartType = ServiceStartMode.Manual,
                DisplayName = "Small Sensor Publishing Service"
            };

            System.ServiceProcess.ServiceProcessInstaller pInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.NetworkService
            };

            this.Installers.AddRange(new Installer[] { installer, pInstaller });
        }
    }
}
