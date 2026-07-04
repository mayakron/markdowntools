using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;
using System;

namespace MarkdownLibrary
{
    public sealed class MarkdigLinkRewriteRenderer : LinkInlineRenderer
    {
        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            if (!link.IsImage && IsLocalMarkdownLink(link.Url))
            {
                link.Url = link.Url.Substring(0, link.Url.Length - 3) + ".html";
            }

            base.Write(renderer, link);
        }

        private static bool IsLocalMarkdownLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            if (url.StartsWith("#")) return false;

            if (Uri.TryCreate(url, UriKind.Absolute, out _)) return false;

            return url.EndsWith(".md", StringComparison.OrdinalIgnoreCase);
        }
    }
}