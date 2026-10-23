using System.Net.Http.Headers;
using System.Text;
using WebExpress.Tutorial.WebIndex.Model;
using Xunit;

namespace WebExpress.Tutorial.WebIndex.Test
{
    /// <summary>
    /// Protects the crawler's metadata, encoding, and link extraction contracts.
    /// </summary>
    public class UnitTestWebCrawlerPage
    {
        /// <summary>
        /// Keeps escaped text and attributes readable in the search index.
        /// </summary>
        [Fact]
        public void Parse_DecodesMetadataAndFiltersLinks()
        {
            // arrange
            const string html = """
                <HTML LANG='d&#101;'><head><TITLE>Müller &amp; Söhne</TITLE></head>
                <body><p>Grüße &#x1F600;</p>
                <a HREF='https://example.com/?a=1&amp;b=2'>link</a>
                <a href=http://example.org/>other</a>
                <a href='/relative'>relative</a><a href='mailto:a@example.com'>mail</a>
                <a href='javascript:alert(1)'>script</a><a href='file:///a'>file</a></body>
                """;

            // act
            var page = WebCrawlerPage.Parse(html, "utf-8");

            // validation
            Assert.Equal("Müller & Söhne", page.Title);
            Assert.Equal("de", page.Language);
            Assert.Contains("Grüße 😀", page.Content);
            Assert.Equal(["https://example.com/?a=1&b=2", "http://example.org/"], page.Links);
        }

        /// <summary>
        /// Prevents markup inside scripts and raw text elements from becoming crawl targets.
        /// </summary>
        [Fact]
        public void Parse_IgnoresScriptStyleAndCommentLinks()
        {
            // arrange
            const string html = """
                <title>First <b>literal</b> &amp; title</title><title>Second</title>
                <script>let s = '<a href="https://script.invalid/">secret</a>'; </scripture></SCRIPT>
                <style>a::after { content: '<a href="https://style.invalid/">hidden</a>'; }</style>
                <!-- <a href="https://comment.invalid/">comment</a> -->
                <textarea><a href="https://textarea.invalid/">literal</a></textarea>
                <p>Visible</p><a href="https://real.example/">real</a>
                """;

            // act
            var page = WebCrawlerPage.Parse(html, "utf-8");

            // validation
            Assert.Equal("First <b>literal</b> & title", page.Title);
            Assert.Equal("https://real.example/", Assert.Single(page.Links));
            Assert.DoesNotContain("secret", page.Content);
            Assert.DoesNotContain("hidden", page.Content);
            Assert.DoesNotContain("comment", page.Content);
            Assert.Contains("Visible", page.Content);
            Assert.Contains("<a href=", page.Content);
        }

        /// <summary>
        /// Allows ordinary incomplete HTML and pages without optional metadata.
        /// </summary>
        [Fact]
        public void Parse_HandlesMissingMetadataAndUnclosedElements()
        {
            // arrange
            const string html = "<body><p>text<a href='https://example.com/'>link<script>hidden";

            // act
            var page = WebCrawlerPage.Parse(html, "utf-8");

            // validation
            Assert.Equal("", page.Title);
            Assert.Null(page.Language);
            Assert.Equal("textlink", page.Content);
            Assert.Single(page.Links);
            Assert.Empty(WebCrawlerPage.Parse("<title>Only title</title>", "utf-8").Links);
        }

        /// <summary>
        /// Preserves legacy characters declared through either supported HTML charset form.
        /// </summary>
        /// <param name="declaration">The charset declaration embedded in the document.</param>
        [Theory]
        [InlineData("<meta charset=windows-1252>")]
        [InlineData("<meta http-equiv='Content-Type' content='text/html; charset=windows-1252'>")]
        public async Task ReadAsync_UsesHtmlCharset(string declaration)
        {
            // arrange
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var bytes = Encoding.GetEncoding(1252).GetBytes(declaration + "<title>Grüße €</title>");
            using var content = new ByteArrayContent(bytes);

            // act
            var page = await WebCrawlerPage.ReadAsync(content);

            // validation
            Assert.Equal("Grüße €", page.Title);
            Assert.Equal("windows-1252", page.EncodingName);
        }

        /// <summary>
        /// Gives the HTTP declaration precedence over conflicting document metadata.
        /// </summary>
        [Fact]
        public async Task ReadAsync_PrefersHttpCharset()
        {
            // arrange
            using var content = new ByteArrayContent(Encoding.UTF8.GetBytes(
                "<meta charset=windows-1252><title>Grüße 😀</title>"));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/html; charset=\"utf-8\"");

            // act
            var page = await WebCrawlerPage.ReadAsync(content);

            // validation
            Assert.Equal("Grüße 😀", page.Title);
            Assert.Equal("utf-8", page.EncodingName);
        }

        /// <summary>
        /// Uses a byte order mark even when headers declare another encoding.
        /// </summary>
        [Fact]
        public async Task ReadAsync_PrefersByteOrderMark()
        {
            // arrange
            var bytes = Encoding.Unicode.GetPreamble().Concat(
                Encoding.Unicode.GetBytes("<title>Grüße 😀</title>")).ToArray();
            using var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/html; charset=windows-1252");

            // act
            var page = await WebCrawlerPage.ReadAsync(content);

            // validation
            Assert.Equal("Grüße 😀", page.Title);
            Assert.Equal("utf-16", page.EncodingName);
        }

        /// <summary>
        /// Falls back to UTF-8 when no usable declaration is available.
        /// </summary>
        /// <param name="charset">The optional unsupported charset header.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("invalid-charset")]
        public async Task ReadAsync_UsesUtf8Fallback(string charset)
        {
            // arrange
            using var content = new ByteArrayContent(Encoding.UTF8.GetBytes(
                "<script>let fake = '<meta charset=windows-1252>';</script><title>Grüße 😀</title>"));
            content.Headers.ContentType = new MediaTypeHeaderValue("text/html") { CharSet = charset };

            // act
            var page = await WebCrawlerPage.ReadAsync(content);

            // validation
            Assert.Equal("Grüße 😀", page.Title);
            Assert.Equal("utf-8", page.EncodingName);
        }
    }
}
