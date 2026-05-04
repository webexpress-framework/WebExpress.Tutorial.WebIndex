using WebExpress.WebCore.WebMessage;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebIcon;

namespace WebExpress.Tutorial.WebIndex.WebControl
{
    /// <summary>
    /// Represents a search form control.
    /// </summary>
    internal class SearchForm : ControlForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchForm"/> class.
        /// </summary>
        public SearchForm()
            : base("searchform")
        {
            FormLayout = _ => TypeLayoutForm.Inline;
            Method = _ => RequestMethod.GET;

            Add(new ControlFormItemInputText()
            {
                Name = _ => "search",
                Placeholder = "webexpress.tutorial.webindex:search.placeholder",
                Styles = ["width: 30rem;"]
            });

            AddPrimaryButton(new ControlFormItemButtonSubmit()
            {
                Text = "webexpress.tutorial.webindex:search.label",
                Icon = new IconPaperPlane()
            });
        }
    }
}
