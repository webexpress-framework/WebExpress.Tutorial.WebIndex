using WebExpress.WebApp.WebControl;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebIcon;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content.Catalog
{
    /// <summary>
    /// Represents a form for editing a catalog index item, 
    /// providing controls for entering the item's URI and title.
    /// </summary>
    [Section<SectionContentPreferences>]
    [Scope<WWW.Setting.Catalog.Id.Edit>]
    public class CatalogFormEdit : FragmentControlRestFormEdit
    {
        /// <summary>
        /// Returns or sets the uir associated with the object.
        /// </summary>
        public ControlFormItemInputText ItemUri { get; } = new ControlFormItemInputText
        {
            Label = "Uri",
            Name = nameof(Model.CatalogItem.Url),
            Placeholder = "Enter the uri of the index item.",
            Required = true,
            MaxLength = 100,
            Icon = new IconCopy(),
            Help = "The URI of the index item. This is a required field and should be unique."
        };

        /// <summary>
        /// Returns or sets the title associated with the object.
        /// </summary>
        public ControlFormItemInputText Title { get; } = new ControlFormItemInputText
        {
            Label = "Title",
            Name = nameof(Model.CatalogItem.Title),
            Format = TypeEditTextFormat.Wysiwyg,
            Placeholder = "Enter a brief description of the index item",
            Required = true,
            MaxLength = 500,
            Help = "A brief description of the index item. This field is required and can include details about the item’s purpose, content, or context."
        };

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="fragmentContext">The context of the fragment.</param>
        public CatalogFormEdit(ISitemapManager sitemapManager, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Add(ItemUri);
            Add(Title);

            Mode = TypeRestFormMode.Edit;
            Uri = sitemapManager.GetUri<WWW.Api._1_.Catalog.Index>(fragmentContext.ApplicationContext);
        }
    }
}
