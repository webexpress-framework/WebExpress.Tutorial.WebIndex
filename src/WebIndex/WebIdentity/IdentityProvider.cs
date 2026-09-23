using System;
using System.Collections.Generic;
using WebExpress.Tutorial.WebIndex.WWW.Api._1_;
using WebExpress.WebCore;
using WebExpress.WebCore.WebIdentity;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPolicies;

namespace WebExpress.Tutorial.WebIndex.WebIdentity
{
    /// <summary>
    /// Supplies the tutorial's development administrator, so the protected settings for seeds and
    /// catalogs can be reached at all. The published demo credentials must never be used by a
    /// production application.
    /// </summary>
    public class IdentityProvider : WebApp.WebIdentity.IdentityProvider
    {
        private static readonly IIdentity _administrator = new Identity
        (
            new Guid("5d0c8f7e-3b8a-4f0e-9a51-6f2b1c4d7e93"),
            "admin",
            roles: ["Administrators"],
            policyNames: [typeof(SystemAccessPolicy).FullName]
        );

        /// <summary>
        /// Offers only the development administrator, because the index tutorial has no user directory of its own.
        /// </summary>
        /// <returns>The development administrator.</returns>
        public override IEnumerable<IIdentity> GetIdentities()
        {
            return [_administrator];
        }

        /// <summary>
        /// Keeps the administrator's system access as an explicit policy claim instead of a group membership.
        /// </summary>
        /// <returns>An empty enumeration.</returns>
        public override IEnumerable<IIdentityGroup> GetGroups()
        {
            return [];
        }

        /// <summary>
        /// Verifies the documented development credentials before issuing an identity with system access.
        /// </summary>
        /// <param name="username">The submitted name of the tutorial's development account.</param>
        /// <param name="password">The submitted demonstration password, which must match exactly.</param>
        /// <returns>The administrator for the documented credentials, or null for every other combination.</returns>
        public override IIdentity Authenticate(string username, string password)
        {
            return string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase) && password == "password"
                ? _administrator
                : null;
        }

        /// <summary>
        /// Routes the login form of a protected page to the application's own session endpoint.
        /// </summary>
        /// <param name="request">The request that hit the protected page.</param>
        /// <param name="initiator">The protected page that requires authentication.</param>
        /// <param name="identity">The identity of the request, which is null for an anonymous visitor.</param>
        /// <returns>The response showing the login page.</returns>
        public override IResponse CreateAuthenticationPrompt(IRequest request, IPageContext initiator, IIdentity identity)
        {
            var sessionUri = WebEx.ComponentHub.SitemapManager.GetUri<Session>(initiator);

            return base.CreateAuthenticationPrompt(request, initiator, identity, sessionUri);
        }
    }
}
