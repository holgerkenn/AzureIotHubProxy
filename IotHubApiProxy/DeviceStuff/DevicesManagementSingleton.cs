
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Common.Security;

namespace IotHubApiProxy.DeviceStuff
{

    public static class DevicesManagementSingleton
    {


        static RegistryManager globalRegistryManager;

        public static RegistryManager GlobalRegistryManager
        {
            get
            {
                if (globalRegistryManager == null)
                {
                    globalRegistryManager = RegistryManager.CreateFromConnectionString(ConfigurationManager.ConnectionStrings["IotHub"].ConnectionString);
                }

                return globalRegistryManager;
            }

        }
    }
}