using WebExpress.WebApp.WebCondition;
using WebExpress.WebApp.WebScope;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebFragment;
using WebExpress.WebUI.WebIcon;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Avatar
{
    /// <summary>
    /// Offers the login in the avatar menu, since the settings are open to administrators only.
    /// </summary>
    /// <remarks>
    /// The entry opens the dialog <see cref="LoginModal"/> renders with the page, so signing
    /// in happens on top of the page the user is on without a round trip.
    /// </remarks>
    [Section<SectionAppAvatarSecondary>]
    [Scope<IScopeGeneral>]
    [Scope<IScopeAdmin>]
    [Condition<ConditionLogout>]
    [Cache]
    public sealed class LoginLink : FragmentControlDropdownItemLink
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public LoginLink(IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            Text = _ => "webexpress.webapp:login.label";
            Icon = _ => new IconRightToBracket();
            PrimaryAction = _ => new ActionModal(LoginModal.ModalId);
        }
    }
}
