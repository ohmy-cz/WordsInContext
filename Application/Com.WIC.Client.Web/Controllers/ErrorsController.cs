using Com.WIC.BusinessLogic.Exceptions;
using Com.WIC.Client.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Com.WIC.Client.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Error/{statusCode?}")]
        [AllowAnonymous]
        public IActionResult Error(int? statusCode)
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var model = new ErrorViewModel { StatusCode = -1, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            if (exceptionHandlerPathFeature?.Error is UserFacingException)
            {
                model.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
                model.ErrorDetails = ((UserFacingException)exceptionHandlerPathFeature.Error).Details;
            }


            if (statusCode.HasValue)
            {
                var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                model.StatusCode = statusCode.Value;
                switch (statusCode)
                {
                    case 404:
                        model.ErrorMessage = "Sorry the page you requested could not be found";
                        model.OriginalPath = statusCodeData.OriginalPath;
                        break;
                    case 500:
                        model.ErrorMessage = "Sorry something went wrong on the server";
                        model.OriginalPath = statusCodeData.OriginalPath;
                        break;
                }
            }

            return View(model);
        }
    }
}