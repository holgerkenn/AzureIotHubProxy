
//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
// The device code of this sample is based on the device sdk samples 
// found at https://github.com/Azure/azure-iot-sdks
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IotHubApiProxy;
using Microsoft.Rest;
using IotHubApiProxy.DeviceStuff;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using System.Threading;

namespace IotHubApiProxyConsoleClient
{
    class Program
    {
        private const string DeviceConnectionString = "<replace>";

        private static int MESSAGE_COUNT = 5;
        private static DeviceClient deviceClient;

        static void Main(string[] args)
        {
            IotHubApiProxyClient i = new IotHubApiProxyClient();
            i.BaseUri = new Uri("<https url of your web service>");
            i.Credentials = new ApiKeyCredentials("1234"); // replace with your access key
            Console.WriteLine("Creating device in iot hub");

            DeviceDataAuth auth = GetMeADevice(i);
            
            Console.WriteLine(auth.Id);
            Console.WriteLine(auth.ConnectionString);
            Console.WriteLine("Created.");



            Console.WriteLine("press any key...");
            Console.ReadKey();
            Console.WriteLine("Connecting device in iot hub");

            //try
            //{
            deviceClient = DeviceClient.CreateFromConnectionString(auth.ConnectionString, TransportType.Http1);
            Console.WriteLine("Connected...");
            Console.WriteLine("press any key...");
            Console.ReadKey();

            SendEvent().Wait();
                ReceiveCommands().Wait();
                Console.WriteLine("Exited!\n");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error in sample: {0}", ex.Message);
            //}

            Console.WriteLine("press any key...");
            Console.ReadKey();
            Console.WriteLine("Deleting device");
            i.Device.DeleteById("1234567");
            Console.WriteLine("Deleted");

        }

        static DeviceDataAuth GetMeADevice(IotHubApiProxyClient i)
        {
            string result = null;
            try
            {
                result = i.Device.GetById("1234567");
            }
            catch (HttpOperationException e)
            {
                if (e.Response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw e;
                }

            }
            if (null != result)
            {
                Console.WriteLine("Found existing device!");
                DeviceData data = JsonConvert.DeserializeObject<DeviceData>(result);
                Console.WriteLine(JsonConvert.SerializeObject(data));
                Console.WriteLine("Deleting existing device!");
                i.Device.DeleteById("1234567");
            }
            string v = i.Device.PostById("1234567");
            
            return JsonConvert.DeserializeObject<DeviceDataAuth>(v);

        }

        static async Task SendEvent()
        {
            string dataBuffer;

            Console.WriteLine("Device sending {0} messages to IoTHub...\n", MESSAGE_COUNT);

            for (int count = 0; count < MESSAGE_COUNT; count++)
            {
                dataBuffer = Guid.NewGuid().ToString();
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
                Console.WriteLine("\t{0}> Sending message: {1}, Data: [{2}]", DateTime.Now.ToLocalTime(), count, dataBuffer);

                await deviceClient.SendEventAsync(eventMessage);
            }
        }

        static async Task ReceiveCommands()
        {
            Console.WriteLine("\nDevice waiting for commands from IoTHub...\n");
            Message receivedMessage;
            string messageData;

            for(int i=0;i<5;i++)
            {
                receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                    await deviceClient.CompleteAsync(receivedMessage);
                }

                //  Note: In this sample, the polling interval is set to 
                //  10 seconds to enable you to see messages as they are sent.
                //  To enable an IoT solution to scale, you should extend this //  interval. For example, to scale to 1 million devices, set 
                //  the polling interval to 25 minutes.
                //  For further information, see
                //  https://azure.microsoft.com/documentation/articles/iot-hub-devguide/#messaging
                Console.WriteLine("sleeping for 10 seconds...");
                Thread.Sleep(10000);
                
            }
        }

    }
}
