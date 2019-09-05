using Com.WIC.BusinessLogic.Interfaces;
using Com.WIC.BusinessLogic.Models;
using IBM.Cloud.SDK.Core.Authentication.BasicAuth;
using System.IO;
using IBM.Watson.TextToSpeech.v1;
using IBM.Cloud.SDK.Core.Authentication.Iam;

namespace Com.WIC.BusinessLogic.Classes
{
    class SpeakerIBMWatson : ISpeaker
    {
        readonly IBM.Watson.TextToSpeech.v1.TextToSpeechService _service;
        public SpeakerIBMWatson(APIConfiguration config) {
            IamConfig IBMConfig = new IamConfig(
                apikey: config.APIKey
            );

            _service = new IBM.Watson.TextToSpeech.v1.TextToSpeechService(IBMConfig);
            _service.SetEndpoint(config.APIEndpoint);
        }

        public bool Speak(string text)
        {
            var response = _service.Synthesize(text);
            if (response.Result.Length == 0)
                return false;
            using (var fileStream = File.Create("C:\\GIT\\WordsInContext\\Output.ogg"))
            {
                //response.Result.Seek(0, SeekOrigin.Begin);
                response.Result.CopyTo(fileStream);
            }
            return response.StatusCode == 200;
        }
    }
}
