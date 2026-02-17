using WebExpress.WebApp.WebControl;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Catalog
{
    /// <summary>
    /// Represents a form for deleting a catalog index item.
    /// </summary>
    public class CatalogFormDelete : FragmentControlRestFormDelete
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="fragmentContext">The context of the fragment.</param>
        public CatalogFormDelete(ISitemapManager sitemapManager, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Mode = TypeRestFormMode.Delete;
            Uri = sitemapManager.GetUri<WWW.Api._1_.Catalog.Index>(fragmentContext.ApplicationContext);
        }

        /// <summary>
        /// Renders the control as an HTML node.
        /// </summary>
        /// <param name="renderContext">
        /// The context in which the control is rendered.
        /// </param>
        /// <param name="visualTree">
        /// The visual tree representing the control's structure.
        /// </param>
        /// <returns>
        /// An HTML node representing the rendered control.
        /// </returns>
        public override IHtmlNode Render(IRenderControlContext renderContext, IVisualTreeControl visualTree)
        {
            var id = renderContext.Request.GetParameter<ParameterGuid>();

            return base.Render(renderContext, visualTree, Items, id.Value.ToString());
        }
    }
}
