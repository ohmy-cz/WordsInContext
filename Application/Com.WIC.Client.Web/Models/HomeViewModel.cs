using Com.WIC.BusinessLogic.Models;
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
        [MaxLength(1000)]
        public string SentencesToSpeak { get; set; }
        public bool ShowSentenceEntry { get; set; }
        public IEnumerable<string> AudioFiles { get; set; }
        public string ErrorMessage { get; set; }
        public string RecaptchaResponse { get; set; }
        public IEnumerable<Tuple<bool, SentenceModel>> SuggestedSentences { get; set; }
    }
}
