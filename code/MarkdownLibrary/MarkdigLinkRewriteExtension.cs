using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;

namespace MarkdownLibrary
{
    public sealed class MarkdigLinkRewriteExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is not HtmlRenderer htmlRenderer) return;

            var existing = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();

            if (existing != null)
            {
                htmlRenderer.ObjectRenderers.Remove(existing);
            }

            htmlRenderer.ObjectRenderers.Add(new MarkdigLinkRewriteRenderer());
        }
    }
}