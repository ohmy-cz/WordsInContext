using Com.WIC.BusinessLogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.WIC.Client.Web.Services
{
	public class APIService
	{
		private static readonly HttpClient client = new HttpClient();
		private readonly string Endpoint;
		public APIService(Configuration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));
			Endpoint = configuration.APIEndpoint;
		}

		public async Task<List<string>> Speak(string sentence)
		{
			var response = await client.GetAsync(Endpoint + "/speak?sentence=" + WebUtility.UrlEncode(sentence));
			return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
		}
	}
}
