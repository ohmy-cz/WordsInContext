using Com.WIC.BusinessLogic.Interfaces;
using Com.WIC.BusinessLogic.Models;
using Google.Apis.Books.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.WIC.BusinessLogic.Services
{
    public class BookSearchService : ISentenceSuggestionService
    {
        readonly Configuration _configuration;
        private readonly BooksService _googleBooksService;
        public BookSearchService(Configuration c)
        {
            _configuration = c ?? throw new ArgumentNullException(nameof(c));
            _googleBooksService = new Google.Apis.Books.v1.BooksService();
        }


        public IEnumerable<SentenceModel> Suggest(string keyword)
        {
            var result = new List<SentenceModel>();
            var searchResults = _googleBooksService.Volumes.List(keyword);
            searchResults.LangRestrict = "en";
            searchResults.MaxResults = 20;
            searchResults.PrettyPrint = false;
            searchResults.Projection = VolumesResource.ListRequest.ProjectionEnum.Lite;
            var volumes = searchResults.Execute();
            result = volumes?.Items?.Select(i => new SentenceModel { 
                    Text = Helpers.Helpers.GetSnippet(i?.SearchInfo?.TextSnippet, keyword),
                    Rating = 0,
                    Source = new SourceModel { 
                        Name = i.VolumeInfo?.Title,
                        Author = i.VolumeInfo?.Authors != null ? string.Join(", ", i.VolumeInfo?.Authors) : null,
                        Uri = i.VolumeInfo?.InfoLink != null ? new Uri(i.VolumeInfo?.InfoLink) : null
                    }
                }).Where(x => !string.IsNullOrWhiteSpace(x.Text)).ToList();
            return result;
        }
    }
}
