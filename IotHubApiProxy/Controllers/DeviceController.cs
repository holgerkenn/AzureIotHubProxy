
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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using System.Configuration;
using IotHubApiProxy.Security;
using System.Web;

namespace IotHubApiProxy.Controllers
{
    public class DeviceController : ApiController
    {
        [ApiKeyAuthenticationAttribute("1234", BasicRealm = "your-realm")]
        public async Task<IHttpActionResult> Get()
        {
            var rm = DeviceStuff.DevicesManagementSingleton.GlobalRegistryManager;
            var devlist = await rm.GetDevicesAsync(1000);
            var devdatalist = new DeviceStuff.DeviceDataList();
            foreach (Device dev in devlist)
            {
                devdatalist.Add(new DeviceStuff.DeviceData(dev));
            }

            return Ok(devdatalist);
        }

        // GET: api/Device/5
        public async Task<IHttpActionResult> Get(string Id)
        {
            var rm = DeviceStuff.DevicesManagementSingleton.GlobalRegistryManager;
            var dev = await rm.GetDeviceAsync(Id);
            if (null==dev)
            { return NotFound(); }

            var devd = new DeviceStuff.DeviceData(dev);
            //devda.SetDeviceConnectionString(ConfigurationManager.ConnectionStrings["IotHub"].ConnectionString);

            return Ok(devd);
        }

        // POST: api/Device

        public async Task<IHttpActionResult> Post([FromBody]string Id)
        {

            //TODO: Validate deviceID string!


            var rm = DeviceStuff.DevicesManagementSingleton.GlobalRegistryManager;
            var olddev = await rm.GetDeviceAsync(Id);
            if (null != olddev)
            { return Unauthorized(); }


            var dev = new Device(Id);
            await rm.AddDeviceAsync(dev);


            dev = await rm.GetDeviceAsync(dev.Id);
            dev.Authentication.SymmetricKey.PrimaryKey = CryptoKeyGenerator.GenerateKey(32);
            dev.Authentication.SymmetricKey.SecondaryKey = CryptoKeyGenerator.GenerateKey(32);
            dev = await rm.UpdateDeviceAsync(dev);

            var devda = new DeviceStuff.DeviceDataAuth(dev);
            devda.SetDeviceConnectionString(ConfigurationManager.ConnectionStrings["IotHub"].ConnectionString);

            return Ok(devda);

        }

        //// PUT: api/Device/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/Device/5
        public async Task<IHttpActionResult> Delete(string Id)
        {
            //TODO: Validate deviceID string!


            var rm = DeviceStuff.DevicesManagementSingleton.GlobalRegistryManager;

            var dev = await rm.GetDeviceAsync(Id);
            if (null == dev)
            { return NotFound(); }

            await rm.RemoveDeviceAsync(Id);
            return Ok();
        }
    }
}
