using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.Tutorial.WebIndex.WebCondition
{
    /// <summary>
    /// Represents a condition that checks for the presence of a "search" parameter in the request.
    /// </summary>
    public class SerachCondition : ICondition
    {
        /// <summary>
        /// Check whether the condition is fulfilled.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if the condition is fulfilled, false otherwise.</returns>
        public bool Fulfillment(IRequest request)
        {
            var check = request.HasParameter("search");

            return check;
        }
    }
}
