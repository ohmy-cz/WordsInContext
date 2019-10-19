using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Com.WIC.Client.Web.Models
{
    public class HomeViewModel
    {
        [MaxLength(20)]
        public string Keyword { get; set; }
        [MaxLength(255)]
        public string Sentence { get; set; }
        public bool SentenceMode { get; set; }
        public List<string> Results { get; set; }
        public string AudioFile { get; set; }
        public string ErrorMessage { get; set; }
        public string RecaptchaResponse { get; set; }
    }
}
