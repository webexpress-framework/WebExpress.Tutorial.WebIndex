using System;
using WebExpress.WebIndex.WebAttribute;

namespace WebExpress.Tutorial.WebIndex.Model
{
    /// <summary>
    /// Represents metadata information for a webpage.
    /// </summary>
    public class MetaData
    {
        /// <summary>
        /// Gets or sets the encoding of the webpage.
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// Gets or sets the language of the webpage.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the content length of the webpage.
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the date when the webpage was added to the index.
        /// </summary>
        [IndexIgnore]
        public DateTime Create { get; set; }
    }
}
