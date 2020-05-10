using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.ExposureNotifications;

namespace ExposureNotification.Backend
{
    
        public class SelfDiagnosisSubmissionRequest
        {
            [JsonProperty("diagnosisUid")]
            public string DiagnosisUid { get; set; }

            [JsonProperty("keys")]
            public IEnumerable<TemporaryExposureKey> Keys { get; set; }
        }
    
}
