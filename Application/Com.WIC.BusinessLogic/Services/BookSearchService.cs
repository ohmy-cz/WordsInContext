using Com.WIC.BusinessLogic.Models;
using Google.Apis.Books.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.WIC.BusinessLogic.Services
{
    public class BookSearchService
    {
        readonly Configuration _configuration;
        private readonly BooksService _googleBooksService;
        public BookSearchService(Configuration c)
        {
            _configuration = c ?? throw new ArgumentNullException(nameof(c));
            _googleBooksService = new Google.Apis.Books.v1.BooksService();
            // Create instances for all enabled book search apis using a factory
        }


        public List<string> SearchBooks(string keyword, CategoryEnum? category)
        {
            //if (_configuration.APIs.Books.Where(x => x.Name == TextToSpeechProvidersEnum.Google && x.Enabled).Count() == 0)
            //{
            //    throw new Exception("Google books must be enabled.");
            //}
            var result = new List<string>();
            // 1. Connect to Google books api, search the books in given category for given keywords,
            // 2. Use Regex to parse the results and extract relevant sentences,
            // 3. Call IBM Watson API to read the sentences gathered
            // 4. Store the resulting mp3 file, return the file path

            var searchResults = _googleBooksService.Volumes.List(keyword);
            searchResults.LangRestrict = "en";
            searchResults.MaxResults = 20;
            searchResults.PrettyPrint = false;
            searchResults.Projection = VolumesResource.ListRequest.ProjectionEnum.Lite;
            var volumes = searchResults.Execute();
            result = volumes?.Items?.Select(i => Helpers.Helpers.GetSnippet(i?.SearchInfo?.TextSnippet, keyword))?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            return result;
        }
    }
}
