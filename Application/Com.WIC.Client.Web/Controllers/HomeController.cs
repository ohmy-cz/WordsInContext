﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Com.WIC.Client.Web.Models;
using System.Threading.Tasks;
using Com.WIC.BusinessLogic.Services;
using System.IO;
using Com.WIC.BusinessLogic.Models;
using Microsoft.Extensions.Configuration;
using Com.WIC.BusinessLogic.Extensions;

namespace Com.WIC.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly WordInSentencesService _wordInSentencesService;
        public HomeController(WordInSentencesService wordInSentencesService)
        {
            _wordInSentencesService = wordInSentencesService ?? throw new ArgumentNullException(nameof(wordInSentencesService));
        }
        public IActionResult Index()
        {
            var model = new HomeViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            try
            {
                var result = _wordInSentencesService.Find(model.Keyword);
                if (result != null)
                {
                    model.AudioFile = result.Item1;
                    model.Results = result.Item2;
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
