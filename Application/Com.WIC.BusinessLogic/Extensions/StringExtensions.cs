using System;
using System.Security.Cryptography;
using System.Text;

namespace Com.WIC.BusinessLogic.Extensions
{
    public static class StringExtensions
    {
        public static string GetHash(this string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static string Sanitize(this string text)
        {
            var input = text.ToLowerInvariant();
            var output = string.Empty;
            char[] allowedChars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                    'á', 'č', 'ć', 'é', 'ě', 'š', 'č', 'ř', 'ž', 'ý', 'á', 'í', 'é', 'ë', 'å', 'æ', 'ø', 'à', 'ê', 'ë', 'è' };
            for(var i = 0; i < input.Length; i++)
            {
                foreach(var j in allowedChars)
                {
                    if(j == text[i])
                    {
                        output += text[i];
                        break;
                    }
                }
            }

            return output;
        }
    }
}
