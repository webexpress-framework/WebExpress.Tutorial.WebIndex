using WebExpress.WebApp.WebFragment;
using WebExpress.WebApp.WebScope;
using WebExpress.WebApp.WebSection;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebFragment;

namespace WebExpress.Tutorial.WebIndex.WebFragment.Content
{
    /// <summary>
    /// Represents a modal form for all porpose within the application.
    /// </summary>
    [Section<SectionContentSecondary>]
    [Scope<IScopeAdmin>]
    [Cache]
    public sealed class RemoteFormModal : FragmentControlModalRemoteForm
    {
        /// <summary>
        /// Initializes a new instance of the class using the 
        /// specified fragment context.
        /// </summary>
        public RemoteFormModal(IFragmentContext fragmentContext)
            : base(fragmentContext, "modal-form")
        {
        }
    }
}
