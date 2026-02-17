using WebExpress.WebApp.WebControl;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Seed
{
    /// <summary>
    /// Represents a edit form control for the initial page settings.
    /// </summary>
    [Section<SectionContentPreferences>]
    [Scope<WWW.Setting.Seed.Id.Edit>]
    public sealed class SeedRestFormEdit : FragmentControlRestFormEdit
    {
        /// <summary>
        /// Returns the control element for entering the login identifier.
        /// </summary>
        public ControlFormItemInputText Url { get; } = new ControlFormItemInputText()
        {
            Name = nameof(Model.Seed.Url),
            Label = "webexpress.tutorial.webindex:setting.seed.add.label",
            Placeholder = "webexpress.tutorial.webindex:setting.seed.add.placeholder",
            Help = "webexpress.tutorial.webindex:setting.seed.add.help"
        };

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="fragmentContext">The context of the fragment.</param>
        public SeedRestFormEdit(ISitemapManager sitemapManager, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Add(Url);

            Mode = TypeRestFormMode.Edit;
            Uri = sitemapManager.GetUri<WWW.Api._1_.Seed.Index>(fragmentContext.ApplicationContext);
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
