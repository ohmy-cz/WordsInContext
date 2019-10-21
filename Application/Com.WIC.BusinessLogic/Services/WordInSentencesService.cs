using Com.WIC.BusinessLogic.Classes;
using Com.WIC.BusinessLogic.Extensions;
using Com.WIC.Encoder;
using Com.WIC.Encoder.Enums;
using Com.WIC.Encoder.Models;
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
		readonly EncoderService _encoderService;
        public WordInSentencesService(StorageProviderService storageProviderService, TextToSpeechService textToSpeechService, EncoderService encoderService)
        {
            _textToSpeechService = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
			_encoderService = encoderService ?? throw new ArgumentNullException(nameof(encoderService));
            _storageProviderService = storageProviderService;
        }
        public IEnumerable<string> Speak(string sentencesText)
        {
            var sentences = sentencesText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x));
            var outputFiles = new List<AudioFile>();
            foreach(var sentence in sentences)
            {
                var fileName = sentence.GetHash();
				var fileExtension = ".ogg";
				var filePath = Path.Combine(_storageProviderService.OutputPath, fileName + fileExtension);

				// Get an ogg file, if it does not exist yet
                if (!File.Exists(filePath + fileExtension))
                {
                    var speaker = _textToSpeechService.GetSpeaker(TextToSpeechProvidersEnum.IBMWatson);
                    speaker.Speak(sentence, filePath);
				}

				// Make an mp3 file which is required by most browsers
				if (File.Exists(filePath + fileExtension) && !File.Exists(filePath + ".mp3"))
				{
					var transcodingResult = _encoderService.ConvertAsync(FileTypeEnum.Mp3, new AudioFile
					{
						FileName = fileName + fileExtension,
						Type = FileTypeEnum.Ogg,
						LocalPath = _storageProviderService.OutputPath
					}).Result;
				}

				outputFiles.Add(new AudioFile
				{
					FileName = fileName + fileExtension,
					Type = FileTypeEnum.Ogg,
					LocalPath = _storageProviderService.OutputPath
				});
			}

			var joinResult = _encoderService.JoinAsync(FileTypeEnum.Mp3, outputFiles.ToArray()).Result;

			return outputFiles.Select(x => _storageProviderService.OutputFolder + "/" + x.FileName);
        }
    }
}
