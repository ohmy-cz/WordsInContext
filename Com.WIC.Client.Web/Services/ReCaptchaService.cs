using Com.WIC.BusinessLogic.Models;
using Com.WIC.Client.Web.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Collections.Generic;

namespace Com.WIC.Client.Web.Services
{
    public class ReCaptchaService
    {
        private readonly Configuration _configuration;
        public ReCaptchaService(Configuration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public GoogleRecaptchaValidationResponse Verify(string response, string remoteIpAddress)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>("secret", _configuration.RecaptchaSecret),
                new KeyValuePair<string,string>("response", response),
                new KeyValuePair<string,string>("remoteip", remoteIpAddress)
            };
            //var gResponseRaw = new HttpClient().PostAsync("https://www.google.com/recaptcha/api/siteverify", new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json")).Result;
            var gResponseRaw = new HttpClient().PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(data)).Result;
            var gResponse = JsonConvert.DeserializeObject<GoogleRecaptchaValidationResponse>(gResponseRaw.Content.ReadAsStringAsync().Result);
            return gResponse;
        }
    }
}
