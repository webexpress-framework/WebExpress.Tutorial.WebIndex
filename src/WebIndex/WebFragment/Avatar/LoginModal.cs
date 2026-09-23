using WebExpress.Tutorial.WebIndex.WWW.Api._1_;
using WebExpress.WebApp.WebCondition;
using WebExpress.WebApp.WebControl;
using WebExpress.WebApp.WebScope;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebUI.WebFragment;
using WebExpress.WebUI.WebPage;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Avatar
{
    /// <summary>
    /// Renders the login dialog that <see cref="LoginLink"/> opens, submitting the credentials
    /// to the <see cref="Session"/> endpoint the full-page login of a protected page uses as well.
    /// </summary>
    [Section<SectionBodySecondary>]
    [Scope<IScopeGeneral>]
    [Scope<IScopeAdmin>]
    [Condition<ConditionLogout>]
    [Cache]
    public sealed class LoginModal : ControlDataModalLogin, IFragmentControl<ControlDataModalLogin>
    {
        /// <summary>
        /// The id the login link targets to open the dialog.
        /// </summary>
        public const string ModalId = "modal-login";

        /// <summary>
        /// Gets the context of the fragment.
        /// </summary>
        public IFragmentContext FragmentContext { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public LoginModal(IFragmentContext fragmentContext)
            : base(ModalId)
        {
            FragmentContext = fragmentContext;
            Header = _ => "webexpress.webapp:login.label";

            this.DataService<Session>();
        }

        /// <summary>
        /// Convert the fragment to HTML.
        /// </summary>
        /// <param name="renderContext">The context in which the fragment is rendered.</param>
        /// <param name="visualTree">The visual tree used for rendering the fragment.</param>
        /// <returns>An HTML node representing the rendered fragment, or null when the fragment is not shown.</returns>
        public override IHtmlNode Render(IRenderControlContext renderContext, IVisualTreeControl visualTree)
        {
            if (!FragmentContext.Check(renderContext?.Request))
            {
                return null;
            }

            return base.Render(renderContext, visualTree);
        }
    }
}
