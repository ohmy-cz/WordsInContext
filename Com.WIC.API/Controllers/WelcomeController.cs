using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Com.WIC.API.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Welcome to the WRDS API.");
        }

		[Route("welcome/404")]
		public IActionResult PageNotFound()
		{
			return Content("Endpoint not found");
		}

		[Route("welcome/500")]
		public IActionResult ServerError()
		{
			return Content("There has been a server error");
		}
	}
}