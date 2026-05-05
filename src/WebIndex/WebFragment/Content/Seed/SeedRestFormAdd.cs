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
        /// Gets the control element for entering the login identifier.
        /// </summary>
        public ControlFormItemInputText Url { get; } = new ControlFormItemInputText()
        {
            Name = _ => nameof(Model.Seed.Url),
            Label = _ => "webexpress.tutorial.webindex:setting.seed.add.label",
            Placeholder = _ => "webexpress.tutorial.webindex:setting.seed.add.placeholder",
            Help = _ => "webexpress.tutorial.webindex:setting.seed.add.help"
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

            Mode = _ => TypeRestFormMode.Add;
            Uri = _ => sitemapManager.GetUri<WWW.Api._1_.Seed.Index>(fragmentContext.ApplicationContext);
        }
    }
}
