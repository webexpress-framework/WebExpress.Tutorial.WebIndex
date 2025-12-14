using System.Collections.Generic;
using System.Linq;
using WebExpress.WebApp.WebIndex;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebSitemap;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1
{
    /// <summary>
    /// Represents a REST API endpoint for operations on Seed entities.
    /// </summary>
    public sealed class Seed : RestApiTable<Model.Seed>
    {
        private readonly string _formUri;
        private readonly IEnumerable<Model.Seed> _data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="applicationContext">The application context.</param>
        public Seed(ISitemapManager sitemapManager, IApplicationContext applicationContext)
        {
            var indexManager = WebEx.ComponentHub.GetComponentManager<IndexManager>();

            var uri = sitemapManager.GetUri<WWW.Setting.Seed>(applicationContext);
            _formUri = uri?.SetFragment("seedForm")?.ToString();

            _data = indexManager?.All<Model.Seed>();
        }

        /// <summary>
        /// Retrieves a collection of options.
        /// </summary>
        /// <param name="request">
        /// The request object containing the criteria for retrieving options. Cannot be null.
        /// </param>
        /// <param name="row">The row object for which options are being retrieved. Cannot be null.</param>
        public override IEnumerable<RestApiOption> GetOptions(Request request, Model.Seed row)
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
        public override IEnumerable<Model.Seed> GetData(string filter, Request request)
        {
            if (filter is null || filter == "null")
            {
                return _data;
            }

            return _data
                .Where
                (
                    x => x.Url.Contains(filter)
                );
        }
    }
}
