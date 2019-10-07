using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.WIC.Client.Web.Models
{
    public class HomeViewModel
    {
        public string Keyword { get; set; }
        public List<string> Results { get; set; }
        public string AudioFile { get; set; }
    }
}
