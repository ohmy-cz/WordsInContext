using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.WIC.Client.Web.Models
{
    public class GoogleRecaptchaValidationResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; } 
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
    }
}
