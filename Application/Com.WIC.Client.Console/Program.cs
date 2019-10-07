using System;
using Com.WIC.Client;

namespace Com.WIC.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Please enter a keyword");
            var keyword = System.Console.ReadLine();
            var bl = new Com.WIC.BusinessLogic.Classes.Core(new Google.Apis.Books.v1.BooksService());
            var output = bl.CreateTrack(keyword, "pharma");
            System.Console.WriteLine(output);
        }
    }
}
