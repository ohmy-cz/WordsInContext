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
    }
}
