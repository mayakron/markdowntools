using System;
using System.IO;

namespace MarkdownConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var markdownFilePath = args[0];
                var cssFilePath = (args.Length > 1) && !args[1].Equals("-") ? args[1] : @"Styles\Default.min.css";
                var documentTitle = (args.Length > 2) && !args[2].Equals("-") ? args[2] : Path.GetFileNameWithoutExtension(markdownFilePath);
                var htmlFilePath = (args.Length > 3) && !args[3].Equals("-") ? args[3] : Path.ChangeExtension(markdownFilePath, ".html");
                var isPreview = (args.Length > 4) && !args[4].Equals("-") ? args[4].Equals("True", StringComparison.OrdinalIgnoreCase) : false;
                var enableMath = (args.Length > 5) && !args[5].Equals("-") ? args[5].Equals("True", StringComparison.OrdinalIgnoreCase) : true;
                var enableDiagrams = (args.Length > 6) && !args[6].Equals("-") ? args[6].Equals("True", StringComparison.OrdinalIgnoreCase) : true;

                MarkdownLibrary.Facade.MarkdownToHtml(markdownFilePath, cssFilePath, documentTitle, htmlFilePath, isPreview, enableMath, enableDiagrams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
    }
}