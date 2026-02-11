using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.Tutorial.WebIndex.WWW.Setting.Catalog;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebFragment;
using WebExpress.WebUI.WebIcon;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Headline
{
    /// <summary>
    /// Represents a fragment control for a link to the index settings page.
    /// </summary>
    /// <remarks>
    /// This fragment is used within the secondary toolbar to provide a link to the index settings page.
    /// </remarks>
    [Section<SectionHeadlineMoreSecondary>]
    [Scope<Index>]
    public sealed class CatalogClearButton : FragmentControlDropdownItemLink
    {
        /// <summary>
        /// Returns the modal form used to confirm delete operations.
        /// </summary>
        public ControlModalFormConfirmDelete DeleteModal { get; } = new ControlModalFormConfirmDelete("modal-delete");

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub used to manage components.</param>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public CatalogClearButton(IComponentHub componentHub, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Text = "webexpress.tutorial.webindex:setting.catalog.clear.label";
            //Uri = componentHub.SitemapManager.GetUri<Catalog>(fragmentContext.ApplicationContext);
            Color = TypeColorText.Danger;
            Icon = new IconTrash();
            PrimaryAction = new ActionModal(DeleteModal.Id);

            DeleteModal.Confirm += OnDeleteModalConfirm;
        }

        /// <summary>
        /// Handles the confirmation action from a delete modal dialog.
        /// </summary>
        /// <param name="sender">The source of the event, typically the control that triggered the confirmation.</param>
        /// <param name="e">An event argument containing details about the form control and its state.</param>
        private void OnDeleteModalConfirm(object sender, ControlFormEvent e)
        {
            ViewModel.ClearCatalog(e.Context);
        }

        /// <summary>
        /// Converts the fragment to an HTML representation.
        /// </summary>
        /// <param name="renderContext">The context in which the control is rendered.</param>
        /// <param name="visualTree">The visual tree representing the control's structure.</param>
        /// <returns>An HTML node representing the rendered control.</returns>
        public override IHtmlNode Render(IRenderControlContext renderContext, IVisualTreeControl visualTree)
        {
            return new HtmlList(base.Render(renderContext, visualTree), DeleteModal.Render(renderContext, visualTree));
        }
    }
}
