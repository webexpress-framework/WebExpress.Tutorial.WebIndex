using System;
using WebExpress.WebApp.WebAttribute;
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
        /// Returns or sets the URL of the webpage.
        /// </summary>
        [RestTableColumnName("webexpress.tutorial.webindex:url.label")]
        public string Url { get; set; }

        /// <summary>
        /// Returns or sets the title of the webpage.
        /// </summary>
        [RestTableColumnName("webexpress.tutorial.webindex:title.label")]
        [IndexDefaultSearch]
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the content of the webpage.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Returns or sets the metadata of the webpage.
        /// </summary>
        public MetaData MetaData { get; set; }
    }
}
