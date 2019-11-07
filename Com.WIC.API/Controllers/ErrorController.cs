using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Com.WIC.API.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index([Bind(Prefix = "id")] int statusCode = 0)
		{
			// Switch to the appropriate page
			switch (statusCode)
			{
				case 404:
					return Content("Not found");
			}

			return Content("Error");

			//// Retrieve error information in case of internal errors
			//var path = HttpContext
			//		  .Features
			//		  .Get<IExceptionHandlerPathFeature>();
			//if (path == null)
			//	return View(model);

			//// Use the information about the exception 
			//var exception = path.Error;
			//var pathString = path.Path;
		}
	}
}