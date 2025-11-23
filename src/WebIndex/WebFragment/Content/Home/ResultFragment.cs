using System;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.Tutorial.WebIndex.WebCondition;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebUri;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebFragment;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Home
{
    /// <summary>
    /// Represents a fragment control panel for displaying search results on the home page.
    /// </summary>
    [Section<SectionContentSecondary>]
    [Scope<WWW.Index>]
    [Condition<SerachCondition>]
    public sealed class ResultFragment : FragmentControlPanelFlex
    {
        /// <summary>
        /// Returns the list control for displaying search results.
        /// </summary>
        public ControlText Counter { get; } = new ControlText()
        {
            Margin = new PropertySpacingMargin
            (
                PropertySpacing.Space.Two,
                PropertySpacing.Space.Auto,
                PropertySpacing.Space.Two,
                PropertySpacing.Space.None
            ),
            Format = TypeFormatText.H4
        };

        /// <summary>
        /// Returns the list control for displaying search results.
        /// </summary>
        public ControlVirtualList List { get; } = new ControlVirtualList()
        {
            Margin = new PropertySpacingMargin
            (
                PropertySpacing.Space.Two,
                PropertySpacing.Space.Auto,
                PropertySpacing.Space.Two,
                PropertySpacing.Space.None
            )
        };

        /// <summary>
        /// Returns the pagination control for navigating through search results.
        /// </summary>
        public ControlPagination Pagination { get; } = new ControlPagination()
        {
            Margin = new PropertySpacingMargin
            (
                PropertySpacing.Space.Two,
                PropertySpacing.Space.Auto,
                PropertySpacing.Space.Two,
                PropertySpacing.Space.None
            ),
        };

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public ResultFragment(IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Layout = TypeLayoutFlex.Default;
            Align = TypeAlignFlex.Center;
            Justify = TypeJustifiedFlex.Center;
            Direction = TypeDirection.Vertical;

            Add(Counter);
            Add(List);
            Add(Pagination);

            List.RetrieveVirtualItem += OnRetrieveVirtualItem;
        }

        /// <summary>
        /// Handles the event when a virtual item needs to be retrieved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data containing information about the virtual item to retrieve.</param>
        private void OnRetrieveVirtualItem(object sender, RetrieveVirtualListItemEventArgs e)
        {
            var param = e.RenderContext.Request.GetParameter("search");

            if (param is null)
            {
                return;
            }

            var res = ViewModel.Retrieve($"Content ~ '{param?.Value}'");

            Counter.Text = I18N.Translate(e.RenderContext, "webexpress.tutorial.webindex:homepage.conter", res.Count());
            e.Items = res.Select
            (
                x => new ControlListItem
                (
                    null,
                    new ControlText()
                    {
                        Text = x?.Title,
                        Format = TypeFormatText.H5,
                        TextColor = new PropertyColorText(TypeColorText.Primary)
                    },
                    new ControlText()
                    {
                        Text = x?.Content.Length > 1000
                            ? string.Concat(x.Content.AsSpan(0, 1000), "...")
                            : x?.Content,
                        Format = TypeFormatText.Paragraph,
                        TextColor = new PropertyColorText(TypeColorText.Dark),
                        Margin = new PropertySpacingMargin
                        (
                            PropertySpacing.Space.Two,
                            PropertySpacing.Space.Null
                        )
                    },
                    new ControlLink()
                    {
                        Text = new UriEndpoint(x?.Url),
                        Uri = new UriEndpoint(x?.Url),
                        TextColor = new PropertyColorText(TypeColorText.Secondary),
                        Margin = new PropertySpacingMargin
                        (
                            PropertySpacing.Space.Two,
                            PropertySpacing.Space.Null
                        )
                    }
                )
                {
                    Margin = new PropertySpacingMargin
                    (
                        PropertySpacing.Space.Null,
                        PropertySpacing.Space.Two
                    )
                }
            );
        }

        /// <summary>
        /// Converts the fragment to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the control is rendered.</param>
        /// <param name="visualTree">The visual tree representing the control's structure.</param>
        /// <returns>An HTML node representing the rendered control.</returns>
        public override IHtmlNode Render(IRenderControlContext renderContext, IVisualTreeControl visualTree)
        {
            var param = renderContext.Request.GetParameter("search");
            var res = ViewModel.Retrieve($"Content ~ '{param?.Value}'");

            Counter.Text = I18N.Translate(renderContext, "webexpress.tutorial.webindex:homepage.conter", res.Count());

            return base.Render(renderContext, visualTree);
        }
    }
}