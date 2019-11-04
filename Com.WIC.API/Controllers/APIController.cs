using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.WIC.BusinessLogic.Services;
using Com.WIC.Client.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Com.WIC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _logger;
        readonly WordInSentencesService _wordInSentencesService;
        readonly BookSearchService _bookSearchService;

        public APIController(ILogger<APIController> logger, WordInSentencesService wordInSentencesService, BookSearchService bookSearchService)
        {
            _logger = logger;
            _wordInSentencesService = wordInSentencesService ?? throw new ArgumentNullException(nameof(wordInSentencesService));
            _bookSearchService = bookSearchService ?? throw new ArgumentNullException(nameof(bookSearchService));
        }

        [HttpGet]
        public IEnumerable<string> GetSupportedLanguages()
        {
            return new List<string> {
                "en-US",
                "sc-CZ",
                "da-DK"
            };
        }

        [HttpGet("speak")]
        public IEnumerable<AudioBundle> Speak(string sentence)
        {
            return _wordInSentencesService.Speak(sentence);
        }
    }
}
