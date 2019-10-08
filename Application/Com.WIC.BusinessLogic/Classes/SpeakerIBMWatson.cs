using Com.WIC.BusinessLogic.Interfaces;
using Com.WIC.BusinessLogic.Models;
using IBM.Cloud.SDK.Core.Authentication.BasicAuth;
using System.IO;
using IBM.Watson.TextToSpeech.v1;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using System;

namespace Com.WIC.BusinessLogic.Classes
{
    class SpeakerIBMWatson : ISpeaker
    {
        readonly string _audioFilesFolder;
        readonly IBM.Watson.TextToSpeech.v1.TextToSpeechService _service;
        public SpeakerIBMWatson(APIConfiguration config, string audioFilesFolder) {
            _audioFilesFolder = audioFilesFolder;
            IamConfig IBMConfig = new IamConfig(
                apikey: config.APIKey
            );

            _service = new IBM.Watson.TextToSpeech.v1.TextToSpeechService(IBMConfig);
            _service.SetEndpoint(config.APIEndpoint);
        }

        public Result<string> Speak(string text)
        {
            try
            {
                var fileName = "Output.ogg";
                var response = _service.Synthesize(text);
                if (response.Result.Length == 0)
                    throw new Exception("IBM Watson Response empty");
                using (var fileStream = File.Create(_audioFilesFolder + Path.DirectorySeparatorChar + fileName))
                {
                    //response.Result.Seek(0, SeekOrigin.Begin);
                    response.Result.CopyTo(fileStream);
                }
                return new Result<string>(fileName, ResultStatusEnum.OK);
            } catch (Exception e)
            {
                return new Result<string>(e.Message, ResultStatusEnum.Error);
            }
        }
    }
}
