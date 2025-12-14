using System.Collections.Generic;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebIndex;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebSitemap;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1
{
    /// <summary>
    /// Handles REST API requests for document entities.
    /// </summary>
    /// <param name="sitemapManager">
    /// The sitemap manager used to retrieve URIs for the application context.
    /// </param>
    /// <param name="applicationContext">
    /// The application context containing the current state of the application.
    /// </param>
    public sealed class Catalog : RestApiTable<Document>
    {
        private readonly string _formUri;
        private readonly IEnumerable<Document> _data;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="applicationContext">The application context.</param>
        public Catalog(ISitemapManager sitemapManager, IApplicationContext applicationContext)
        {
            var indexManager = WebEx.ComponentHub.GetComponentManager<IndexManager>();

            var uri = sitemapManager.GetUri<Catalog>(applicationContext);
            _formUri = uri?.SetFragment("indexForm")?.ToString();

            _data = indexManager?.All<Document>();
        }

        /// <summary>
        /// Retrieves a collection of options.
        /// </summary>
        /// <param name="request">The request object containing the criteria for retrieving options. Cannot be null.</param>
        /// <param name="row">The row object for which options are being retrieved. Cannot be null.</param>
        public override IEnumerable<RestApiOption> GetOptions(Request request, Model.Document row)
        {
            yield return new RestApiOptionHeader(request)
            {
                Label = "webexpress.webapp:header.setting.label"
            };

            yield return new RestApiOptionEdit(request)
            {
                Uri = _formUri
            };

            yield return new RestApiOptionSeperator(request);
            yield return new RestApiOptionDelete(request);
        }

        /// <summary>
        /// Processing of the resource that was called via the get request.
        /// </summary>
        /// <param name="filter">The filtering and sorting options.</param>
        /// <param name="request">The request.</param>
        /// <returns>An enumeration of which json serializer can be serialized.</returns>
        public override IEnumerable<Document> GetData(string filter, Request request)
        {
            if (filter is null || filter == "null")
            {
                return _data;
            }

            return _data
                .Where
                (
                    x => x.Url.Contains(filter) ||
                    x.Title.Contains(filter)
                );
        }
    }
}
