using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sspe
{
    /// <summary>
    /// Represents a single subscription to a topic.
    /// </summary>
    internal class PublishSubscription
    {
        /// <summary>
        /// The topic that is subscribed to.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The callback method that can process the topic
        /// </summary>
        public Func<string, object, bool> Callback { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishSubscription"/> class.
        /// </summary>
        /// <param name="topic">The topic that is subscribed to.</param>
        /// <param name="callback">The callback method that processes the messages arriving for the topic.</param>
        public PublishSubscription(string topic, Func<string, object, bool> callback)
        {
            this.Topic = topic;
            this.Callback = callback;
        }
    }
}
