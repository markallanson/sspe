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

namespace Sspe
{
    public class PublishingManager
    {
        private static string PublishingMonitorDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @".\publishers";

        private ScriptEngine scriptEngine = Python.CreateEngine();
        private Dictionary<string, ScriptInfo> PublishingScripts = new Dictionary<string, ScriptInfo>();

        public PublishingManager()
        {
            // add some common assemblies
            ScriptRuntime runtime = scriptEngine.Runtime;
            runtime.LoadAssembly(Assembly.GetExecutingAssembly());
            runtime.LoadAssembly(typeof(string).Assembly);
            runtime.LoadAssembly(typeof(Uri).Assembly);
            runtime.LoadAssembly(typeof(System.Xml.XmlDocument).Assembly);
            runtime.LoadAssembly(typeof(System.Diagnostics.Trace).Assembly);

            // enumerate the scripts, and prepare the scopes and sources
            foreach (string scriptPath in Directory.GetFiles(PublishingMonitorDirectory, "*.py"))
            {
                try
                {
                    ScriptInfo info = new ScriptInfo();
                    info.ScriptPath = scriptPath;
                    info.CompiledCode = scriptEngine.CreateScriptSourceFromFile(scriptPath).Compile();
                    info.ScriptScope = scriptEngine.CreateScope();

                    // execute it to run any initialization code in the publisher
                    info.CompiledCode.Execute(info.ScriptScope);

                    // get the topics that the publisher wants to be notified of.
                    Func<IEnumerable<string>> GetTopics = info.ScriptScope.GetVariable<Func<IEnumerable<string>>>("GetTopics");

                    // get the publish function provided by the publisher, the MQTT client will directly invoke it for us.
                    Func<string, object, bool> Publish = info.ScriptScope.GetVariable<Func<string, object, bool>>("Publish");

                    IEnumerable<string> topics = GetTopics();
                    foreach (var topic in topics)
                    {
                        MqttClientManager.Subscribe(topic, Nmqtt.MqttQos.AtMostOnce, Publish);
                    }

                    info.Topics.AddRange(topics);
                    PublishingScripts.Add(scriptPath, info);
                }
                catch (Exception ex)
                {
                    ExceptionOperations eo;
                    eo = scriptEngine.GetService<ExceptionOperations>();
                    string error = eo.FormatException(ex);
                    Trace.WriteLine(String.Format("Initialising Publishers:: {0}", error));
                }
            }
        }


        public void Shutdown()
        {
            // load and start each Publishing 
            foreach (ScriptInfo info in PublishingScripts.Values)
            {
                Func<bool> Shutdown = info.ScriptScope.GetVariable<Func<bool>>("Shutdown");
                Shutdown();
            }
        }

    }
}
