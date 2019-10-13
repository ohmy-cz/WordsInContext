using System;
using System.Collections.Generic;
using System.Text;

namespace Com.WIC.BusinessLogic.Models
{
    public class Configuration
    {
        public string IBMApiKey { get; set; }
        public APISettings APIs { get; set; }
        public override string ToString()
        {
            return $"{IBMApiKey}\r\n{APIs.TextToSpeech[0].APIKey}";
        }
    }

    public class APISettings
    {
        public List<APIConfiguration> Books { get; set; }
        public List<APIConfiguration> TextToSpeech { get; set; }
    }

    public class APIConfiguration
    {
        public TextToSpeechProvidersEnum Name { get; set; }
        public bool Enabled { get; set; }
        public string APIEndpoint { get; set; }
        public string APIKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
