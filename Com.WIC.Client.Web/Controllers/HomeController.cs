using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.WIC.Client.Web.Models;
using Com.WIC.BusinessLogic.Services;

namespace Com.WIC.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly BookSearchService _bookSearchService;
        readonly TextToSpeechService _textToSpeechService;
        public HomeController(BookSearchService bookSearchService, TextToSpeechService textToSpeechService)
        {
            _bookSearchService = bookSearchService ?? throw new ArgumentNullException(nameof(bookSearchService));
            _textToSpeechService = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
        }
        public IActionResult Index()
        {
            var model = new HomeViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            model.Results = _bookSearchService.SearchBooks(model.Keyword, null);
            var toBeSpoken = new List<string>();
            toBeSpoken.Add($"The word selected is: {model.Keyword}.");
            toBeSpoken.AddRange(model.Results);
            var speaker = _textToSpeechService.GetSpeaker(TextToSpeechProvidersEnum.IBMWatson);
            speaker.Speak(model.Keyword);
            model.AudioFile = _textToSpeechService.TextToAudioFile(toBeSpoken);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
