using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebFragment;
using WebExpress.WebUI.WebIcon;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Headline
{
    /// <summary>
    /// Represents a fragment control form for initiating a web crawling operation.
    /// </summary>
    [Section<SectionHeadlineSecondary>]
    [Scope<WWW.Setting.Seed.Index>]
    public sealed class CrawlButton : FragmentControlForm
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CrawlButton(IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Margin = new PropertySpacingMargin(PropertySpacing.Space.Two);

            AddPrimaryButton(new ControlFormItemButtonSubmit()
            {
                Text = "webexpress.tutorial.webindex:run.label",
                BackgroundColor = new PropertyColorBackground(TypeColorBackground.Success),
                Icon = new IconPlayCircle()
            });

            ProcessForm += OnProcessForm;
        }

        /// <summary>
        /// Processes the form submission and initiates the web crawling operation.
        /// </summary>
        /// <param name="processEvent">The form event object containing the submission data.</param>
        private void OnProcessForm(ControlFormEventFormProcess processEvent)
        {
            WebCrawler.Crawl(processEvent.Context.Request);
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
