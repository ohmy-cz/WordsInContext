using Com.WIC.BusinessLogic.Exceptions;
using Com.WIC.BusinessLogic.Models;
using Com.WIC.BusinessLogic.Services;
using Com.WIC.Client.Web.Models;
using Com.WIC.Client.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Com.WIC.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionKeyName = "WIC";
        readonly WordInSentencesService _wordInSentencesService;
        readonly ReCaptchaService _recaptchaService;
        readonly BookSearchService _bookSearchService;
        public HomeController(WordInSentencesService wordInSentencesService, ReCaptchaService recaptchaService, BookSearchService bookSearchService)
        {
            _wordInSentencesService = wordInSentencesService ?? throw new ArgumentNullException(nameof(wordInSentencesService));
            _recaptchaService = recaptchaService ?? throw new ArgumentNullException(nameof(recaptchaService));
            _bookSearchService = bookSearchService ?? throw new ArgumentNullException(nameof(bookSearchService));
        }
        public IActionResult Index()
        {
            var model = new HomeViewModel();
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return Error();
            }
            if(!string.IsNullOrWhiteSpace(model.RecaptchaResponse))
            {
                var r = _recaptchaService.Verify(model.RecaptchaResponse, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                if(!r.Success)
                {
                    throw new UserFacingException($"You are not human.", $"Google response: {string.Join(", ", r.ErrorCodes)}");
                }
            }
            try
            {
                model.SuggestedSentences = _bookSearchService.Suggest(model.Keyword).Select(sm => new Tuple<bool, SentenceModel>(false, sm));
            }
            catch (Exception e)
            {
                model.ErrorMessage = e.Message;
            }
            HttpContext.Session.SetString(SessionKeyName, JsonConvert.SerializeObject(model));
            return RedirectToAction("Editor");
        }

        public IActionResult Editor()
        {
            if(HttpContext?.Session?.Keys == null || !HttpContext.Session.Keys.Contains(SessionKeyName))
            {
                return RedirectToAction("Index");
            }
            var model = JsonConvert.DeserializeObject<HomeViewModel>(HttpContext.Session.GetString(SessionKeyName));
            return View(model);
        }

        [HttpPost]
        public IActionResult Editor(HomeViewModel model)
        {
            if(HttpContext?.Session?.Keys != null && HttpContext.Session.Keys.Contains(SessionKeyName) && model.SuggestedSentences == null)
            {
                var storedModel = JsonConvert.DeserializeObject<HomeViewModel>(HttpContext.Session.GetString(SessionKeyName));
                model.SuggestedSentences = storedModel.SuggestedSentences;
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.RecaptchaResponse))
            {
                var r = _recaptchaService.Verify(model.RecaptchaResponse, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                if (!r.Success)
                {
                    throw new UserFacingException($"You are not human.", $"Google response: {string.Join(", ", r.ErrorCodes)}");
                }
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(model.SentencesToSpeak))
                {
                    var result = _wordInSentencesService.Speak(model.SentencesToSpeak);
                    if (result != null)
                    {
                        model.AudioFiles = result;
                    }
                } else
                {
                    model.ErrorMessage = "No sentences entered!";
                }
            }
            catch (Exception e)
            {
                model.ErrorMessage = e.Message;
            }
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
