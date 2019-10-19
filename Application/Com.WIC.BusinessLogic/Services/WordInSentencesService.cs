using Com.WIC.BusinessLogic.Classes;
using Com.WIC.BusinessLogic.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Com.WIC.BusinessLogic.Services
{
    public class WordInSentencesService
    {
        readonly TextToSpeechService _textToSpeechService;
        readonly StorageProviderService _storageProviderService;
        public WordInSentencesService(StorageProviderService storageProviderService, TextToSpeechService textToSpeechService)
        {
            _textToSpeechService = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
            _storageProviderService = storageProviderService;
        }
        public IEnumerable<string> Speak(string sentencesText)
        {
            var sentences = sentencesText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x));
            var outputFiles = new List<string>();
            foreach(var sentence in sentences)
            {
                var fileName = sentence.GetHash() + ".ogg";
                var filePath = Path.Combine(_storageProviderService.OutputPath, fileName);
                if (!File.Exists(filePath))
                {
                    var speaker = _textToSpeechService.GetSpeaker(TextToSpeechProvidersEnum.IBMWatson);
                    speaker.Speak(sentence, filePath);
                }
                outputFiles.Add(_storageProviderService.OutputFolder + "/" + fileName);
            }
            return outputFiles;
        }
    }
}
