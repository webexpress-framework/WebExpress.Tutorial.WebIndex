using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebIcon;

namespace WebExpress.Tutorial.WebIndex.WebControl
{
    /// <summary>
    /// Represents a form for creating or editing a catalog index item, 
    /// providing controls for entering the item's URI and title.
    /// </summary>
    public class CatalogForm : ControlForm
    {
        /// <summary>
        /// Returns or sets the uir associated with the object.
        /// </summary>
        public ControlFormItemInputText ItemUri { get; } = new ControlFormItemInputText
        {
            Label = "Uri",
            Name = nameof(Model.Document.Url),
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
            Name = nameof(Model.Document.Title),
            Format = TypeEditTextFormat.Wysiwyg,
            Placeholder = "Enter a brief description of the index item",
            Required = true,
            MaxLength = 500,
            Help = "A brief description of the index item. This field is required and can include details about the item’s purpose, content, or context."
        };

        /// <summary>
        /// Returns the submit button control for the form.
        /// </summary>
        public ControlFormItemButtonSubmit Submit { get; } = new ControlFormItemButtonSubmit
        {

        };

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="id">The unique identifier for the form control.</param>
        public CatalogForm(string id)
            : base(id)
        {
            Enable = false;

            Add(ItemUri);
            Add(Title);
            AddPrimaryButton(Submit);
        }
    }
}
