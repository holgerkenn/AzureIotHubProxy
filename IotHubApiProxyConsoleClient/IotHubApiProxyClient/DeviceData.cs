// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IotHubApiProxy.DeviceStuff
{
    public class DeviceData 
    {
        public string Id { get; set; }
        public string ConnectionState { get; set; }
        public DateTime LastActivityTime { get; set; }
        public DateTime LastConnectionStateUpdatedTime { get; set; }
        public DateTime LastStateUpdatedTime { get; set; }
        public int MessageCount { get; set; }
        public string State { get; set; }
        public string SuspensionReason { get; set; }

    }
}
    