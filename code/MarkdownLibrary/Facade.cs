using Markdig;
using System.IO;
using System.Text;

namespace MarkdownLibrary
{
    public static class Facade
    {
        public static string MarkdownToHtml(string markdownFilePath, string cssFilePath, string documentTitle, string htmlFilePath = null, bool isPreview = true, bool enableMath = true, bool enableDiagrams = true)
        {
            var markdown = File.ReadAllText(markdownFilePath);

            var style = File.ReadAllText(cssFilePath);

            var body = Markdown.ToHtml(markdown, isPreview ? new MarkdownPipelineBuilder().UseAdvancedExtensions().Build() : new MarkdownPipelineBuilder().UseAdvancedExtensions().Use(new MarkdigLinkRewriteExtension()).Build());

            var document = new StringBuilder();

            document.AppendLine("<!DOCTYPE html>");
            document.AppendLine("<html>");
            document.AppendLine("<head>");
            document.AppendLine("<meta charset=\"utf-8\">");

            if (!string.IsNullOrEmpty(documentTitle))
            {
                document.AppendLine($"<title>{documentTitle}</title>");
            }

            document.AppendLine($"<style>{style}</style>");
            document.AppendLine("</head>");
            document.AppendLine("<body>");

            if (enableMath)
            {
                document.AppendLine("<script async src=\"https://cdn.jsdelivr.net/npm/mathjax@4.0.0/tex-mml-chtml.js\"></script>");
            }

            if (enableDiagrams)
            {
                document.AppendLine("<script async src=\"https://cdn.jsdelivr.net/npm/mermaid/dist/mermaid.min.js\"></script>");
                document.AppendLine("<script>mermaid.initialize({ startOnLoad: true });</script>");
            }

            document.AppendLine(body);
            document.AppendLine("</body>");
            document.AppendLine("</html>");

            var html = document.ToString();

            if (!string.IsNullOrEmpty(htmlFilePath))
            {
                File.WriteAllText(htmlFilePath, html);
            }

            return html;
        }
    }
}