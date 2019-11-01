using System.Collections.Generic;

namespace Com.WIC.Client.Web.Models
{
    public class AudioBundle
    {
        public IList<AudioSource> Sources { get; set; }
    }

    public class AudioSource
    {
        public string Uri { get; set; }
        public string ContentType { get; set; }
    }
}
