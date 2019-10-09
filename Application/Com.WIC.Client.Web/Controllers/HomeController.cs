using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Com.WIC.Client.Web.Models;
using Com.WIC.BusinessLogic.Services;
using System.IO;
using Com.WIC.BusinessLogic.Models;

namespace Com.WIC.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly BookSearchService _bookSearchService;
        readonly TextToSpeechService _textToSpeechService;
        readonly Configuration _c;
        public HomeController(Configuration c, BookSearchService bookSearchService, TextToSpeechService textToSpeechService)
        {
            _c = c ?? throw new ArgumentNullException(nameof(c));
            _bookSearchService = bookSearchService ?? throw new ArgumentNullException(nameof(bookSearchService));
            _textToSpeechService = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
        }
        public IActionResult Index()
        {
            ViewBag.ttt = _c.IBMApiKey;
            ViewBag.ttt1 = _c.APIs.TextToSpeech[0].APIKey;
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
            var result = speaker.Speak(string.Join(' ', model.Results));
            if (result.Status == ResultStatusEnum.OK)
            {
                model.AudioFile = Path.DirectorySeparatorChar + "Output" + Path.DirectorySeparatorChar + result.Data;
                return View(model);
            } else
            {
                return Content($"Could not process {model.Keyword}.\r\n\r\nAn error occured:\r\n{result.Data}", "text/plain");
            }
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
