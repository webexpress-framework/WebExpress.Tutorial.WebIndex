using System.Net.Http;
using WebExpress.WebApp.WebData;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Seed
{
    /// <summary>
    /// Represents the initial link fragment which is a control panel fragment.
    /// </summary>
    [Section<SectionContentPrimary>]
    [Scope<WWW.Setting.Seed.Index>]
    public sealed class SeedTable : FragmentControlDataTable
    {
        private readonly ISitemapManager _sitemapManager;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public SeedTable(ISitemapManager sitemapManager, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            _sitemapManager = sitemapManager;

            ServiceFactory = rennderContext => new DataServiceBuilder("data")
                .Endpoint<WWW.Api._1_.Seed.Table>()
                .Method(HttpMethod.Get)
                .Query(q => q
                    .Map("search", "q")
                    .Map("wql", "wql")
                    .Map("filter", "f")
                    .Map("page", "p")
                    .Map("pageSize", "l")
                    .Map("orderBy", "o")
                    .Map("orderDir", "d"))
                .Response(r => r
                    .Items("items")
                    .Total("total"))
                .Build(rennderContext);
        }

        /// <summary>
        /// Converts the fragment to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the control is rendered.</param>
        /// <param name="visualTree">The visual tree representing the control's structure.</param>
        /// <returns>An HTML node representing the rendered control.</returns>
        public override IHtmlNode Render(IRenderControlContext renderContext, IVisualTreeControl visualTree)
        {
            return base.Render(renderContext, visualTree);
        }
    }
}
