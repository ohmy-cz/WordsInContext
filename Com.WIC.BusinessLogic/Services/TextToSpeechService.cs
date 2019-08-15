using Com.WIC.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.WIC.BusinessLogic.Services
{
    public class TextToSpeechService
    {
        readonly Configuration _configuration;
        public TextToSpeechService(Configuration c)
        {
            _configuration = c ?? throw new ArgumentNullException(nameof(c));
            // Create instances for all enabled text to speech apis using a factory
        }

        /// <summary>
        /// Generates an mp3 file with the text snippets spoken.
        /// </summary>
        /// <param name="text">A list of text snippets to be inserted in the autio file</param>
        /// <param name="delay">Delay between text snippets</param>
        /// <returns>URL to the file generated</returns>
        public string TextToAudioFile(List<string> text, int? delay = 30)
        {
            return "https://www.jancerveny.net";
        }
    }
}
