using WebExpress.WebApp.WebControl;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebUI.WebControl;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Seed
{
    /// <summary>
    /// Represents a add form control for the initial page settings.
    /// </summary>
    [Section<SectionContentPreferences>]
    [Scope<WWW.Setting.Seed.Add>]
    public sealed class SeedRestFormAdd : FragmentControlRestFormAdd
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
        public SeedRestFormAdd(ISitemapManager sitemapManager, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Add(Url);

            Mode = TypeRestFormMode.Add;
            Uri = sitemapManager.GetUri<WWW.Api._1_.Seed.Index>(fragmentContext.ApplicationContext);
        }
    }
}
