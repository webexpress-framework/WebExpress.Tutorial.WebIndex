using System;
using WebExpress.WebCore.WebDomain;
using WebExpress.WebIndex;
using WebExpress.WebIndex.WebAttribute;

namespace WebExpress.Tutorial.WebIndex.Model
{
    /// <summary>
    /// The class contains information about a webpage.
    /// </summary>
    public class CatalogItem : IIndexItem, IDomain
    {
        /// <summary>
        /// The URL of the webpage.
        /// </summary>
        [IndexIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the URL of the webpage.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title of the webpage.
        /// </summary>
        [IndexDefaultSearch]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the webpage.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the metadata of the webpage.
        /// </summary>
        public MetaData MetaData { get; set; }
    }
}
