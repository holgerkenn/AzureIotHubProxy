# AzureIotHubProxy

** This approach is deprecated, either use Azure IoT DPS https://docs.microsoft.com/en-us/azure/iot-dps/ or the successor project https://github.com/holgerkenn/qdrs **

This is example code to show how to manage an Azure IoT hub from an ASP.Net website. 

More infos soon on http://blogs.msdn.com/b/holgerkenn/

Includes a .net client example. 

Unmodified, it allows IoT devices to create their own IOT Hub registration through a simple REST api secured by a simple API key. 

**Great for demos! Don't use this for production as it circumvents a few security layers around IOT Hub.**

To use this sample, you need to enter/change a number of keys:

You should change the default api key "1234" for the individual calls in DeviceController.cs (for the service) and in Program.cs (for the sample client) 

In web.config, enter the connection string for an Azure IOT Hub. You can find more information here: https://azure.microsoft.com/en-us/services/iot-hub/

If you want to use Application Insights to monitor your web service, you need to add an ApplicationInsights.config file with a management key

See LICENSE for (MIT) License Information. 

This source code uses bits and pieces from the Azure IOT SDK https://github.com/Azure/azure-iot-sdks 
especially from the Device explorer https://github.com/Azure/azure-iot-sdks/tree/master/tools/DeviceExplorer 
and the .net device Client SDK https://github.com/Azure/azure-iot-sdks/tree/master/csharp/device

