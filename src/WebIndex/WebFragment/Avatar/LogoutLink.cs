using WebExpress.Tutorial.WebIndex.WWW.Api._1_;
using WebExpress.WebApp.WebCondition;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebApp.WebScope;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Avatar
{
    /// <summary>
    /// Ends the administrator's session from the avatar menu through the application's session endpoint.
    /// </summary>
    [Section<SectionAppAvatarSecondary>]
    [Scope<IScopeGeneral>]
    [Scope<IScopeAdmin>]
    [Condition<ConditionLogin>]
    [Cache]
    public sealed class LogoutLink : FragmentControlDropdownItemLinkLogout
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub used to resolve the session endpoint.</param>
        /// <param name="fragmentContext">The context in which the fragment is used.</param>
        public LogoutLink(IComponentHub componentHub, IFragmentContext fragmentContext)
            : base(fragmentContext)
        {
            RestEndpoint = renderContext => componentHub?.SitemapManager
                .GetUri<Session>(renderContext?.PageContext.ApplicationContext);
        }
    }
}
