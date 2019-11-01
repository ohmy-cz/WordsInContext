using System;
using System.Collections.Generic;
using System.Text;
using IBM.Watson.TextToSpeech;
using Google.Apis.Books.v1;

namespace Com.WIC.BusinessLogic.Classes
{
    public class Core
    {
        private readonly BooksService _booksService;

        public Core(BooksService booksService)
        {
            _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
        }
        /// <summary>
        /// Returns URL of the MP3 generated from keyword and book category provided
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public string CreateTrack(string keyword, string category)
        {
            // 1. Connect to Google books api, search the books in given category for given keywords,
            // 2. Use Regex to parse the results and extract relevant sentences,
            // 3. Call IBM Watson API to read the sentences gathered
            // 4. Store the resulting mp3 file, return the file path

            var searchResults = _booksService.Volumes.List(keyword);
            var volumes = searchResults.Execute();
            var text = string.Empty;
            foreach(var result in volumes.Items)
            {
                text += result.SearchInfo.TextSnippet;
            }
            return text;
        }
    }
}
