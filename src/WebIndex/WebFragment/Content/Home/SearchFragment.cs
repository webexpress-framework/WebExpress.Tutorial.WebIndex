using WebExpress.Tutorial.WebIndex.WebControl;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebFragment;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Home
{
    /// <summary>
    /// Represents the home content fragment which includes a search form.
    /// </summary>
    [Section<SectionContentPrimary>]
    [Scope<WWW.Index>]
    public sealed class SearchFragment : FragmentControlPanelFlex
    {
        /// <summary>
        /// Gets the control image for the home content fragment.
        /// </summary>
        private ControlImage Image { get; } = new ControlImage()
        {
            Width = 400,
            Classes = ["rounded"],
            Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.None, PropertySpacing.Space.None, PropertySpacing.Space.Two)
        };

        /// <summary>
        /// Gets the search formular.
        /// </summary>
        public ControlForm Form { get; } = new SearchForm()
        {
            Justify = TypeJustifiedFlex.Center
        };

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public SearchFragment(IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Layout = TypeLayoutFlex.Default;
            Align = TypeAlignFlex.Center;
            Justify = TypeJustifiedFlex.Center;
            Direction = TypeDirection.Vertical;

            Add(Image);
            Add(Form);

            Image.Uri = fragmentContext.ApplicationContext.Route
                .Concat("/assets/img/webindexlogo.png")
                ?.ToUri();
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