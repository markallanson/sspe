using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nmqtt;
using System.Diagnostics;

namespace Sspe
{
    public static class MqttClientManager
    {
        private static MqttClient mqttClient = null;
        private static List<PublishSubscription> subscriptions = new List<PublishSubscription>();

        public static void Connect()
        {
            mqttClient = new MqttClient(Settings.Default.MqttServerName, Settings.Default.MqttServerPort, Settings.Default.MqttClientIdentifier);
            mqttClient.Connect();
        }

        public static void Disconnect()
        {
            mqttClient.Dispose();
            mqttClient = null;

            // drop the old subscriptions
            subscriptions = new List<PublishSubscription>();
        }

        public static short Publish(string topic, MqttQos qos, byte[] data)
        {
            if (mqttClient == null)
            {
                throw new InvalidOperationException("You must call Connect() on the MqttClientManager before attempting to publish");
            }

            return mqttClient.PublishMessage(topic, qos, data);
        }

        public static void Subscribe(string topic, MqttQos qos, Func<string, object, bool> callback)
        {
            if (mqttClient == null)
            {
                throw new InvalidOperationException("You must call Connect() on the MqttClientManager before attempting to subscribe");
            }

            // if we don't have any for this topic yet, subscribe with the topic.
            if (subscriptions.Count(x => x.Topic == topic) == 0)
            {
                mqttClient.Subscribe(topic, qos, MessageReceiver);
            }

            // add it to the subscriptions list
            PublishSubscription pSub = new PublishSubscription(topic, callback);
            subscriptions.Add(pSub);
        }

        /// <summary>
        /// Distribute the message to all the publishers.
        /// </summary>
        /// <param name="topic">The topic that was published to.</param>
        /// <param name="data">The data that was published.</param>
        /// <returns>true; always.</returns>
        public static bool MessageReceiver(string topic, object data)
        {
            var topicSubs = from sub in subscriptions
                            where sub.Topic.Equals(topic)
                            select sub;

            // invoke each of the publishers.
            foreach (var sub in topicSubs)
            {
                try
                {
                    sub.Callback(topic, data);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(String.Format("Error while processing topic subscription: {0}", ex));
                }
            }

            return true;
        }
    }
}
