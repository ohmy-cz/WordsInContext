using Com.WIC.BusinessLogic.Extensions;
using Com.WIC.Client.Web.Models;
using Com.WIC.Encoder;
using Com.WIC.Encoder.Enums;
using Com.WIC.Encoder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<AudioBundle>> Speak(string sentencesText)
        {
            var ob = new List<AudioBundle>();
			await Task.Run(() => {
				var sentences = sentencesText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x));
				var outputFiles = new List<AudioFile>();
				var filesForTranscoding = new List<AudioFile>();
				foreach (var sentence in sentences)
				{
					var fileName = sentence.GetHash();
					var filePathOgg = Path.Combine(_storageProviderService.OutputPath, fileName + ".ogg");
					var filePathMp3 = Path.Combine(_storageProviderService.OutputPath, fileName + ".mp3");
					var ab = new AudioBundle { Sources = new List<AudioSource>() };

					var oggAudioFile = new AudioFile
					{
						FileName = fileName + ".ogg",
						AudioType = FileTypeEnum.Ogg,
						LocalPath = _storageProviderService.OutputPath
					};

					// Get an ogg file, if it does not exist yet
					if (!File.Exists(filePathOgg))
					{
						var speaker = _textToSpeechService.GetSpeaker(TextToSpeechProvidersEnum.IBMWatson);
						speaker.Speak(sentence, filePathOgg);
					}
					ab.Sources.Add(new AudioSource { Uri = $"{_storageProviderService.OutputFolder}/{fileName}.ogg", ContentType = "audio/ogg" });
					outputFiles.Add(oggAudioFile);

					if (!File.Exists(filePathMp3))
					{
						filesForTranscoding.Add(oggAudioFile);
					}
					ab.Sources.Add(new AudioSource { Uri = $"{_storageProviderService.OutputFolder}/{fileName}.mp3", ContentType = "audio/mp3" });
					ob.Add(ab);
				}

				if (filesForTranscoding.Count() > 0)
				{
					// TODO: Add succesfully converted mp3s to the output
					var convertedResult = _encoderService.ConvertAsync(FileTypeEnum.Mp3, filesForTranscoding.ToArray()).Result;
				}
				var combinedFileName = $"Combined-{string.Join("-", sentences).GetHash()}";
				var combinedFileNameOgg = combinedFileName + ".ogg";
				var combinedFileNameMp3 = combinedFileName + ".mp3";
				if (filesForTranscoding.Count() >= 2)
				{
					var ab = new AudioBundle { Sources = new List<AudioSource>() };
					var joinAudioFile = new AudioFile
					{
						FileName = combinedFileNameMp3,
						AudioType = FileTypeEnum.Mp3,
						LocalPath = _storageProviderService.OutputPath
					};

					if (!File.Exists(Path.Combine(_storageProviderService.OutputPath, combinedFileNameMp3)))
					{
						var joinResult = _encoderService.JoinAsync(joinAudioFile, 3, filesForTranscoding.Select(x => new AudioFile { AudioType = FileTypeEnum.Mp3, FileName = x.FileName.Replace(".ogg", ".mp3"), LocalPath = x.LocalPath }).ToArray()).Result;
					}

					ab.Sources.Add(new AudioSource { Uri = $"{_storageProviderService.OutputFolder}/{combinedFileNameMp3}", ContentType = "audio/mp3" });

					//if (!File.Exists(Path.Combine(_storageProviderService.OutputPath, combinedFileNameOgg)))
					//{
					//    var convertResult = _encoderService.ConvertAsync(FileTypeEnum.Ogg, joinAudioFile).Result;
					//}
					//ab.Sources.Add(new AudioSource { Uri = $"{_storageProviderService.OutputFolder}/{combinedFileNameOgg}", ContentType = "audio/ogg" });
					ob.Add(ab);
				}
			});
			return ob;
			//return await new Task<IEnumerable<AudioBundle>>(ob);
        }
    }
}
