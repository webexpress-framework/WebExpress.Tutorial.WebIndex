using System.Linq;
using WebExpress.Tutorial.WebIndex.WebIdentity;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1_
{
    /// <summary>
    /// Routes the tutorial's login form through the same provider as the central authentication endpoint.
    /// </summary>
    [Cache]
    public sealed class Session : RestApiSession
    {
        private readonly IIdentityProviderManager _identityProviderManager;
        private readonly IApplicationContext _applicationContext;

        /// <summary>
        /// Binds login to the tutorial's registered providers so credentials and policy claims cannot diverge.
        /// </summary>
        /// <param name="identityProviderManager">The registry containing the application's authentication providers.</param>
        /// <param name="applicationContext">The application whose provider registration owns this login endpoint.</param>
        public Session(IIdentityProviderManager identityProviderManager, IApplicationContext applicationContext)
        {
            _identityProviderManager = identityProviderManager;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Rejects invalid credentials before the shared identity manager can issue administrator tokens.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        /// <returns>The authenticated identity if valid; otherwise, null.</returns>
        protected override IIdentity ValidateCredentials(string username, string password)
        {
            return _identityProviderManager.GetProviders(_applicationContext)
                .OfType<IdentityProvider>()
                .Select(provider => provider.Authenticate(username, password))
                .FirstOrDefault(identity => identity is not null);
        }
    }
}
