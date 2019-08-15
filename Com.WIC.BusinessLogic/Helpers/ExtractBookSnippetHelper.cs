using System.Text.RegularExpressions;

namespace Com.WIC.BusinessLogic.Helpers
{
    public static class Helpers
    {
        public static string GetSnippet(string text, string keyword)
        {
            if(string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(keyword))
            {
                return null;
            }
            string decodedText = System.Net.WebUtility.HtmlDecode(text);
            string strippedText = Regex.Replace(decodedText, @"<(.|\n)*?>", string.Empty);
            // Todo: [Bb]iler
            string surroundingSentence = Regex.Match(strippedText, @"([^.!?]*?\b" + keyword + @"\b.*?[.!?])(?:$|\s(?=[A-Z]))").Value;
            return surroundingSentence;
        }
    }
}
