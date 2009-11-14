using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using Nmqtt;

namespace Sspe
{
    public class SensorManager
    {
        private static string SensorMonitorDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\sensormonitors";

        private ScriptEngine scriptEngine = Python.CreateEngine();
        private Dictionary<string, ScriptInfo> sensorScripts = new Dictionary<string, ScriptInfo>();
        private SensorBridge sensorBridge = new SensorBridge();

        public SensorManager()
        {
            sensorBridge.SensorDataAvailable += new EventHandler<SensorDataEventArgs>(sensorBridge_SensorDataAvailable);

            // add some common assemblies
            ScriptRuntime runtime = scriptEngine.Runtime;
            runtime.LoadAssembly(Assembly.GetExecutingAssembly());
            runtime.LoadAssembly(typeof(string).Assembly);
            runtime.LoadAssembly(typeof(Uri).Assembly);
            runtime.LoadAssembly(typeof(System.Diagnostics.Trace).Assembly);

            // enumerate the scripts, and prepare the scopes and sources
            foreach (string script in Directory.GetFiles(SensorMonitorDirectory, "*.py"))
            {
                try
                {
                    ScriptInfo info = new ScriptInfo();
                    info.CompiledCode = scriptEngine.CreateScriptSourceFromFile(script).Compile();
                    info.ScriptScope = scriptEngine.CreateScope();
                    sensorScripts.Add(script, info);
                }
                catch (Exception ex)
                {
                    ExceptionOperations eo;
                    eo = scriptEngine.GetService<ExceptionOperations>();
                    string error = eo.FormatException(ex);
                    Trace.WriteLine(String.Format("Initialising SensorMonitors:: {0}", error));
                }
            }
        }

        public void Start()
        {
            // load and start each sensor 
            foreach (ScriptInfo info in sensorScripts.Values)
            {
                // each sensor script gets it's own thread. It is responsible for staying alive
                // with a loop etc if it wants to do any works.
                // no polling sensors yet, just event based.
                ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                    {
                        try
                        {
                            ScriptInfo threadInfo = (ScriptInfo)state;
                            threadInfo.CompiledCode.Execute(threadInfo.ScriptScope);
                            Func<SensorBridge, bool> Run = threadInfo.ScriptScope.GetVariable<Func<SensorBridge, bool>>("Run");
                            Run(sensorBridge);
                        }
                        catch (Exception ex)
                        {
                            ExceptionOperations eo;
                            eo = scriptEngine.GetService<ExceptionOperations>();
                            string error = eo.FormatException(ex);
                            Trace.WriteLine(String.Format("Running SensorMonitor:: {0}", error));
                        }
                    }), info);
            }
        }

        public void Stop()
        {
            // load and start each sensor 
            foreach (ScriptInfo info in sensorScripts.Values)
            {
                Func<bool> Stop = info.ScriptScope.GetVariable<Func<bool>>("Stop");
                Stop();
            }
        }

        void sensorBridge_SensorDataAvailable(object sender, SensorDataEventArgs e)
        {
            Trace.WriteLine(String.Format("SensorData(Topic='{0}', Data='{1}')", e.Topic, System.Text.Encoding.ASCII.GetString(e.SensorData)));
            MqttClientManager.Publish(e.Topic, MqttQos.AtMostOnce, e.SensorData);
        }
    }
}
