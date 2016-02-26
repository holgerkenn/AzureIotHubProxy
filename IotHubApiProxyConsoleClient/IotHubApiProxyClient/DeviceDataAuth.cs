// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IotHubApiProxy.DeviceStuff
{
    public class DeviceDataAuth: DeviceData
    {

        
      
        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
        public string ConnectionString { get; set; }

       
    }
}