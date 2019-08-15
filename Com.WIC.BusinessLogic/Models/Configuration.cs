using System;
using System.Collections.Generic;
using System.Text;

namespace Com.WIC.BusinessLogic.Models
{
    public class Configuration
    {
        public APISettings APIs {get;set;}
    }

    public class APISettings
    {
        public List<APIConfiguration> Books { get; set; }
        public List<APIConfiguration> TextToSpeech { get; set; }
    }

    public class APIConfiguration
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }
}
