using Com.WIC.BusinessLogic.Services;
using Com.WIC.Client.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.WIC.API.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class SpeakController : ControllerBase
    {
        private readonly ILogger<SpeakController> _logger;
        readonly WordInSentencesService _wordInSentencesService;
        readonly BookSearchService _bookSearchService;

        public SpeakController(ILogger<SpeakController> logger, WordInSentencesService wordInSentencesService, BookSearchService bookSearchService)
        {
            _logger = logger;
            _wordInSentencesService = wordInSentencesService ?? throw new ArgumentNullException(nameof(wordInSentencesService));
            _bookSearchService = bookSearchService ?? throw new ArgumentNullException(nameof(bookSearchService));
        }

        [HttpGet]
        public async Task<IEnumerable<AudioBundle>> Speak([FromQuery]string sentence)
        {
            return await _wordInSentencesService.Speak(sentence);
        }
    }
}
