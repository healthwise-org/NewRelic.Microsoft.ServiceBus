The MIT License (MIT)
Copyright (c) 2016 Healthwise, Inc.

# Org.Healthwise.NewRelic.Microsoft.ServiceBus

# Requirements
1. .Net 4.5

# Supports
The plugin supports both Windows Service Bus and Azure Service Bus.

# Functionality
The Service Bus Plugin supports both Windows (On-Prem) and Azure Service Bus.  The monitor tracks Queues, Topics and Global stats.
The plugin will automatically discover and report metrics on all the Queues and Topics in a namespace that the monitor has access to.  

	Tracking
		Per Queue -  active message count, dead letter message count
		All Queues - bytes stored, total message count

		Per Topic - active message count, dead letter message count, subscriptions
		All Topics - bytes stored

# Known Issues
	No known at this time

# Configuration
When using NPI for installation all configuration will be handled during installation.  For manual configuration the plugin.json and newrelic.json in the /config folder need to be updated.

	Plugin.json - Add a name and connection string for each service bus instance you want to monitor
	NewRelic.json - Add your license key so the plugin can report back to New Relic

# Installation
The recomended method for installation is to use the NPI Method below.  The manual installation will not handle setting up the monitor to run as a service.

	# Use NPI to install the plugin to register as a service
	1. Run Command as admin: npi install Org.Healthwise.NewRelic.Microsoft.ServiceBus
	2. Follow on screen prompts

	# Manual Install
	1. Download release and unzip on machine to handle monitoring.
	2. Edit Config Files
		rename newrelic.template.json to newrelic.json
		Rename plugin.template.json to plugin.json
		Update settings in both config files for your environment
	3. Run plugin.exe from Command line