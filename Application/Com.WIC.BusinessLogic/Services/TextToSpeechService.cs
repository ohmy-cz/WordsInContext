using Com.WIC.BusinessLogic.Classes;
using Com.WIC.BusinessLogic.Interfaces;
using Com.WIC.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.WIC.BusinessLogic.Services
{
    public class TextToSpeechService
    {
        readonly Configuration _configuration;
        readonly string _audioFilesFolder;
        public TextToSpeechService(Configuration c, string audioFilesFolder)
        {
            _configuration = c ?? throw new ArgumentNullException(nameof(c));
            _audioFilesFolder = audioFilesFolder ?? throw new ArgumentNullException(nameof(audioFilesFolder));
            // Create instances for all enabled text to speech apis using a factory
        }

        public ISpeaker GetSpeaker(TextToSpeechProvidersEnum providerType)
        {
            var relevantConfig = _configuration.APIs.TextToSpeech.Where(x => x.Name == providerType).FirstOrDefault();
            if (relevantConfig == null)
                throw new NotSupportedException($"Requested text to speech {providerType} tech is not supported");
            switch (providerType) {
                case TextToSpeechProvidersEnum.IBMWatson:
                    return new SpeakerIBMWatson(relevantConfig, _audioFilesFolder);
                default:
                    throw new NotImplementedException($"Requested text to speech {providerType} not implemented");
            }
        }
    }
}
