using Com.WIC.BusinessLogic.Classes;
using Com.WIC.BusinessLogic.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.WIC.BusinessLogic.Services
{
    public class WordInSentencesService
    {
        readonly BookSearchService _bookSearchService;
        readonly TextToSpeechService _textToSpeechService;
        readonly StorageProviderService _storageProviderService;
        public WordInSentencesService(StorageProviderService storageProviderService, BookSearchService bookSearchService, TextToSpeechService textToSpeechService)
        {
            _bookSearchService = bookSearchService ?? throw new ArgumentNullException(nameof(bookSearchService));
            _textToSpeechService = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
            _storageProviderService = storageProviderService;
        }
        public Tuple<string, List<string>> Find(string keyword)
        {
            keyword = keyword.Sanitize();
            var fileName = keyword.GetHash() + ".ogg";
            var filePath = Path.Combine(_storageProviderService.OutputPath, fileName);
            var results = _bookSearchService.SearchBooks(keyword, null);
            if (results == null || results.Count == 0)
                return null;
            if (!File.Exists(filePath))
            {
                var toBeSpoken = new List<string>() { $"The word selected is: {keyword}." };
                toBeSpoken.AddRange(results);
                var speaker = _textToSpeechService.GetSpeaker(TextToSpeechProvidersEnum.IBMWatson);
                speaker.Speak(string.Join(" ", results), filePath);
            }
            return new Tuple<string, List<string>>(_storageProviderService.OutputFolder + "/" + fileName, results);
        }
    }
}
