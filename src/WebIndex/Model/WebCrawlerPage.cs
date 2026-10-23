using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebExpress.WebCore.WebHtml.Parser;

namespace WebExpress.Tutorial.WebIndex.Model
{
    /// <summary>
    /// Extracts searchable page data with the framework tokenizer and platform encodings.
    /// </summary>
    internal sealed class WebCrawlerPage
    {
        /// <summary>
        /// Gets the decoded document title used by the search results.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the declared document language when available.
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// Gets the searchable text without scripts, styles, or comments.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Gets the encoding actually used to decode the response.
        /// </summary>
        public string EncodingName { get; private set; }

        /// <summary>
        /// Gets the absolute HTTP and HTTPS anchor targets eligible for crawling.
        /// </summary>
        public IReadOnlyList<string> Links { get; private set; }

        /// <summary>
        /// Decodes a response before extracting metadata and indexable content.
        /// </summary>
        /// <param name="content">The HTTP content containing the page and its optional charset.</param>
        /// <returns>The extracted page data.</returns>
        public static async Task<WebCrawlerPage> ReadAsync(HttpContent content)
        {
            var bytes = await content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var encoding = FindEncoding(content.Headers.ContentType?.CharSet);
            if (encoding == null)
            {
                var prefix = Encoding.Latin1.GetString(bytes, 0, Math.Min(bytes.Length, 1024));
                foreach (var token in new HtmlTokenizer(prefix).Tokenize())
                {
                    if (token.TagName != "meta" || token.Type != HtmlTokenType.SelfClosingTag)
                    {
                        continue;
                    }

                    var charset = Attribute(token, "charset");
                    if (charset == null && string.Equals(Attribute(token, "http-equiv"),
                        "content-type", StringComparison.OrdinalIgnoreCase) &&
                        MediaTypeHeaderValue.TryParse(Attribute(token, "content"), out var declaration))
                    {
                        charset = declaration.CharSet;
                    }

                    encoding = FindEncoding(charset);
                    if (encoding != null)
                    {
                        break;
                    }
                }
            }

            using var stream = new MemoryStream(bytes, false);
            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8, true);
            var html = await reader.ReadToEndAsync().ConfigureAwait(false);
            return Parse(html, reader.CurrentEncoding.WebName);
        }

        /// <summary>
        /// Collects page data without requiring a well-formed HTML document tree.
        /// </summary>
        /// <param name="html">The decoded HTML document.</param>
        /// <param name="encodingName">The encoding recorded for the indexed document.</param>
        /// <returns>The metadata, text, and crawlable links extracted from the document.</returns>
        public static WebCrawlerPage Parse(string html, string encodingName)
        {
            var page = new WebCrawlerPage { EncodingName = encodingName };
            var title = new StringBuilder();
            var text = new StringBuilder();
            var links = new List<string>();
            var inTitle = false;
            var titleFound = false;
            string ignoredTag = null;

            foreach (var token in new HtmlTokenizer(html).Tokenize())
            {
                if (ignoredTag != null)
                {
                    if (token.Type == HtmlTokenType.EndTag && token.TagName == ignoredTag)
                    {
                        ignoredTag = null;
                    }

                    continue;
                }

                if (token.Type == HtmlTokenType.Text)
                {
                    var value = WebUtility.HtmlDecode(token.Value);
                    text.Append(value);
                    if (inTitle)
                    {
                        title.Append(value);
                    }

                    continue;
                }

                if (token.Type == HtmlTokenType.EndTag)
                {
                    if (token.TagName == "title")
                    {
                        inTitle = false;
                    }

                    continue;
                }

                if (token.Type is not (HtmlTokenType.StartTag or HtmlTokenType.SelfClosingTag))
                {
                    continue;
                }

                if (token.Type == HtmlTokenType.StartTag && token.TagName is "script" or "style")
                {
                    ignoredTag = token.TagName;
                }
                else if (token.TagName == "title" && !titleFound)
                {
                    inTitle = token.Type == HtmlTokenType.StartTag;
                    titleFound = true;
                }
                else if (token.TagName == "html" && page.Language == null)
                {
                    page.Language = Attribute(token, "lang");
                }
                else if (token.TagName == "a")
                {
                    var href = Attribute(token, "href");
                    if (Uri.TryCreate(href, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {
                        links.Add(uri.AbsoluteUri);
                    }
                }
            }

            page.Title = title.ToString();
            page.Content = text.ToString();
            page.Links = links;
            return page;
        }

        /// <summary>
        /// Decodes attribute entities consistently with document text.
        /// </summary>
        /// <param name="token">The tag containing the attribute.</param>
        /// <param name="name">The normalized attribute name.</param>
        /// <returns>The decoded value, or null when the attribute is absent.</returns>
        private static string Attribute(HtmlToken token, string name)
        {
            return WebUtility.HtmlDecode(token.Attributes.FirstOrDefault(x => x.Name == name)?.Value);
        }

        /// <summary>
        /// Allows legacy web encodings while ignoring invalid charset declarations.
        /// </summary>
        /// <param name="charset">The optional HTTP or HTML charset declaration.</param>
        /// <returns>The matching encoding, or null when the declaration cannot be used.</returns>
        private static Encoding FindEncoding(string charset)
        {
            if (string.IsNullOrWhiteSpace(charset))
            {
                return null;
            }

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return Encoding.GetEncoding(charset.Trim().Trim('"', '\''));
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
