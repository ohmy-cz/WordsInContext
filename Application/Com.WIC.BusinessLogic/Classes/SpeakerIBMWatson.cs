using Com.WIC.BusinessLogic.Interfaces;
using Com.WIC.BusinessLogic.Models;
using IBM.Cloud.SDK.Core.Authentication.BasicAuth;
using System.IO;
using IBM.Watson.TextToSpeech.v1;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using System;
using Com.WIC.BusinessLogic.Extensions;

namespace Com.WIC.BusinessLogic.Classes
{
    class SpeakerIBMWatson : ISpeaker
    {
        readonly IBM.Watson.TextToSpeech.v1.TextToSpeechService _service;
        public SpeakerIBMWatson(APIConfiguration config)
        {
            IamConfig IBMConfig = new IamConfig(
                apikey: config.APIKey
            );

            _service = new IBM.Watson.TextToSpeech.v1.TextToSpeechService(IBMConfig);
            _service.SetEndpoint(config.APIEndpoint);
        }

        public void Speak(string text, string filePath)
        {
            var response = _service.Synthesize(text);
            if (response.Result.Length == 0)
                throw new Exception("IBM Watson Response empty");
            using (var fileStream = File.Create(filePath))
            {
                response.Result.CopyTo(fileStream);
            }
        }
    }
}
