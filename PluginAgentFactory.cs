using System;
using System.Collections.Generic;
using NewRelic.Platform.Sdk;

namespace Org.Healthwise.NewRelic.Microsoft.ServiceBus
{
    class PluginAgentFactory : AgentFactory
    {
        /// <summary>
        /// Creates agents for each item in the plugin.json file
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public override Agent CreateAgentWithConfiguration(IDictionary<string, object> properties)
        {
            string name = (string)properties["name"];
            string connString = (string)properties["connectionString"];

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(connString))
            {
                throw new ArgumentNullException("'name' and 'ConnectionString' cannot be null or empty. Do you have a 'config/plugin.json' file?");
            }

            return new PluginAgent(name, connString);
        }
    }
}
