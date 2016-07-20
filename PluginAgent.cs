using System;
using System.Collections.Generic;
using System.Reflection;
using NewRelic.Platform.Sdk;
using NewRelic.Platform.Sdk.Utils;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Org.Healthwise.NewRelic.Microsoft.ServiceBus
{
    class PluginAgent : Agent
    {
        // Name of Agent
        private string name;
        private string connectionString;

        // Provides logging for Plugin
        private Logger log = Logger.GetLogger(typeof(PluginAgent).Name);

        /// <summary>
        /// Constructor for Agent Class
        /// Accepts name and other parameters from plugin.json file
        /// </summary>
        /// <param name="name"></param>
        public PluginAgent(string name, string connString)
        {
            this.name = name;
            this.connectionString = connString;
        }

        #region "NewRelic Methods"
        /// <summary>
        /// Provides the GUID which New Relic uses to distiguish plugins from one another
        /// Must be unique per plugin
        /// </summary>
        public override string Guid
        {
            get
            {
                return "Org.Healthwise.NewRelic.Microsoft.ServiceBus";
            }
        }

        /// <summary>
        /// Provides the version information to New Relic.
        /// Uses the 
        /// </summary>
        public override string Version
        {
            get
            {
                return typeof(PluginAgent).Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Returns a human-readable string to differentiate different hosts/entities in the New Relic UI
        /// </summary>
        public override string GetAgentName()
        {
            return this.name;
        }

        /// <summary>
        /// This is where logic for fetching and reporting metrics should exist.  
        /// Call off to a REST head, SQL DB, virtually anything you can programmatically 
        /// get metrics from and then call ReportMetric.
        /// </summary>
        public override void PollCycle()
        {

            GetQueuedItems();  
        }
        #endregion


        public bool GetQueuedItems()
        {
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (namespaceManager == null)
            {
                log.Error("\nUnexpected Error: NamespaceManager is NULL");
                return false;
            }
            else
            {
                IEnumerable<QueueDescription> queues = namespaceManager.GetQueues();
                IEnumerable<TopicDescription> topics = namespaceManager.GetTopics();
                long totalQMessages = 0;
                long totalQMsgBytes = 0;

                foreach (QueueDescription q in queues)
                {
                    // Get Metrics
                    totalQMessages += q.MessageCount;
                    totalQMsgBytes += q.SizeInBytes;

                    // Send Metrics to New Relic
                    ReportMetric("Queues/" + q.Path + "/Items/", "items", q.MessageCountDetails.ActiveMessageCount);
                    ReportMetric("Queues/" + q.Path + "/DLItems/", "items", q.MessageCountDetails.DeadLetterMessageCount);
                }
                
                // Summary Queue Statistics
                ReportMetric("Queue/TotalMessages/", "items", totalQMessages);
                ReportMetric("Queue/TotalMsgBytes/", "bytes", totalQMsgBytes);

                // TOPICS
                long totalTMsgBytes = 0;

                foreach (TopicDescription t in topics)
                {
                    totalTMsgBytes += t.SizeInBytes;

                    // Send Metrics to New Relic
                    ReportMetric("Topics/" + t.Path + "/Items/", "items", t.MessageCountDetails.ActiveMessageCount);
                    ReportMetric("Topics/" + t.Path + "/DLItems/", "items", t.MessageCountDetails.DeadLetterMessageCount);
                    ReportMetric("Topics/" + t.Path + "/Subscription/", "count", t.SubscriptionCount);
                }

                // Summary Topic Statistics
                ReportMetric("Topic/TotalMsgBytes/", "bytes", totalTMsgBytes);

                return true;
            }
        }
    }
}
