// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace Microsoft.Rest
{
    /// <summary>
    /// API key based credentials for use with a REST Service Client.
    /// </summary>
    public class ApiKeyCredentials : ServiceClientCredentials
    {
        private const string ApiKeyNameDefault = "api_key";
        protected string ApiKeyName { get; private set; }
        protected string ApiKey { get; private set; }

        public ApiKeyCredentials(string apikey)
            : this(apikey,ApiKeyNameDefault)
        {
        }

        public ApiKeyCredentials(string apikey, string apikeyname)
        {
            if (string.IsNullOrEmpty(apikey))
            {
                throw new ArgumentNullException("apikey");
            }
            if (string.IsNullOrEmpty(apikeyname))
            {
                throw new ArgumentNullException("apikeyname");
            }
            ApiKey = apikey;
            ApiKeyName = apikeyname;
        }

        /// <summary>
        /// Apply the credentials to the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// Task that will complete when processing has completed.
        /// </returns>
        public async override Task ProcessHttpRequestAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            Uri ru = request.RequestUri;
            string r = ru.ToString();
            if (ru.Query.Contains("?"))
            {
                r += "&" + ApiKeyName + "=" + ApiKey;

            }
            else
            {
                r += "?" + ApiKeyName + "=" + ApiKey;

            }
            Uri ru2 = new Uri(r);
            request.RequestUri = ru2;
            await base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
