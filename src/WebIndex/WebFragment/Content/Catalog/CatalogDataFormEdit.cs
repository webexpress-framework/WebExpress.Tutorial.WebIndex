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
    public class CatalogFormEdit : FragmentControlDataFormEdit
    {
        /// <summary>
        /// Gets or sets the uir associated with the object.
        /// </summary>
        public ControlFormItemInputText ItemUri { get; } = new ControlFormItemInputText
        {
            Label = _ => "Uri",
            Name = _ => nameof(Model.CatalogItem.Url),
            Placeholder = _ => "Enter the uri of the index item.",
            Required = _ => true,
            MaxLength = _ => 100,
            Icon = _ => new IconCopy(),
            Help = _ => "The URI of the index item. This is a required field and should be unique."
        };

        /// <summary>
        /// Gets or sets the title associated with the object.
        /// </summary>
        public ControlFormItemInputText Title { get; } = new ControlFormItemInputText
        {
            Label = _ => "Title",
            Name = _ => nameof(Model.CatalogItem.Title),
            Format = _ => TypeEditTextFormat.Wysiwyg,
            Placeholder = _ => "Enter a brief description of the index item",
            Required = _ => true,
            MaxLength = _ => 500,
            Help = _ => "A brief description of the index item. This field is required and can include details about the item’s purpose, content, or context."
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

            Uri = _ => sitemapManager.GetUri<WWW.Api._1_.Catalog.Index>(fragmentContext.ApplicationContext);
        }
    }
}
