using System;
using WebExpress.WebCore.WebDomain;
using WebExpress.WebIndex;
using WebExpress.WebIndex.WebAttribute;

namespace WebExpress.Tutorial.WebIndex.Model
{
    /// <summary>
    /// The class contains information about a initial webpage.
    /// </summary>
    public class Seed : IIndexItem, IDomain
    {
        /// <summary>
        /// The URL of the webpage.
        /// </summary>
        [IndexIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the URL of the webpage.
        /// </summary>
        [IndexDefaultSearch]
        public string Url { get; set; }
    }
}
