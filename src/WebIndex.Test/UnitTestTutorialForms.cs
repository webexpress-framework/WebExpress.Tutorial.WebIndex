using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.Tutorial.WebIndex.WebFragment.Content.Home;
using WebExpress.Tutorial.WebIndex.WebFragment.Content.Seed;
using WebExpress.WebUI.WebControl;
using Xunit;
using SeedEndpoint = WebExpress.Tutorial.WebIndex.WWW.Api._1_.Seed.Index;

namespace WebExpress.Tutorial.WebIndex.Test
{
    /// <summary>
    /// Protects the tutorial's form binding and search result contracts.
    /// </summary>
    public class UnitTestTutorialForms
    {
        /// <summary>
        /// Ensures seed forms declare the service required by the REST controller.
        /// </summary>
        [Fact]
        public void SeedForms_DeclareDataServicesAndRequiredUrls()
        {
            // arrange
            var add = new SeedDataFormAdd(null, null);
            var edit = new SeedDataFormEdit(null, null);
            var delete = new SeedDataFormDelete(null, null);

            // act
            var services = new[] { add.ServiceFactory, edit.ServiceFactory, delete.ServiceFactory };

            // validation
            Assert.All(services, service => Assert.NotNull(service));
            Assert.True(add.Url.Required(null));
            Assert.True(edit.Url.Required(null));
        }

        /// <summary>
        /// Rejects missing and unsupported crawl addresses before persistence.
        /// </summary>
        /// <param name="url">The proposed crawl address.</param>
        /// <param name="expected">Whether the address is supported.</param>
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("relative/path", false)]
        [InlineData("http://", false)]
        [InlineData("file:///tmp/test", false)]
        [InlineData("javascript:alert(1)", false)]
        [InlineData("https://example.com/path", true)]
        [InlineData("http://localhost/webindex", true)]
        public void SeedUrl_RequiresHttpAddress(string url, bool expected)
        {
            // arrange
            var candidate = url;

            // act
            var valid = SeedEndpoint.IsValidUrl(candidate);

            // validation
            Assert.Equal(expected, valid);
        }

        /// <summary>
        /// Ensures empty results have neither entries nor an unbound page switcher.
        /// </summary>
        [Fact]
        public void EmptySearch_HasNoEntriesOrPagination()
        {
            // arrange
            var fragment = new ResultFragment(null);

            // act
            var items = ResultFragment.CreateItems([]).ToArray();

            // validation
            Assert.Empty(items);
            Assert.DoesNotContain(fragment.Content, item => item is ControlPagination);
        }

        /// <summary>
        /// Ensures every matching document supplies a result entry, including null content.
        /// </summary>
        [Fact]
        public void SearchResults_IncludeEveryMatch()
        {
            // arrange
            var documents = new[]
            {
                new CatalogItem { Title = "First", Content = new string('x', 1200), Url = "https://example.com/first" },
                new CatalogItem { Title = "Second", Content = null, Url = "https://example.com/second" }
            };

            // act
            var items = ResultFragment.CreateItems(documents).ToArray();

            // validation
            Assert.Equal(2, items.Length);
            Assert.All(items, item => Assert.NotNull(item));
        }
    }
}
