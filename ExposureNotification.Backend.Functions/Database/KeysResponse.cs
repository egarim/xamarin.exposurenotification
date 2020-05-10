using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.ExposureNotifications;

namespace ExposureNotification.Backend
{
  
        public class KeysResponse
        {
            [JsonProperty("latest")]
            public Int64 Latest { get; set; }

            [JsonProperty("keys")]
            public IEnumerable<TemporaryExposureKey> Keys { get; set; }
        }
    
}
