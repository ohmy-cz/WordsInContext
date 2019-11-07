using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.WIC.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<string> GetSupportedLanguages()
		{
			return new List<string> {
				"en-US",
				"sc-CZ",
				"da-DK"
			};
		}
	}
}