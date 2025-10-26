using System.Collections.Generic;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebIndex;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebRestApi;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebIndex.Wql;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1
{
    /// <summary>
    /// Handles REST API requests for Index entities.
    /// </summary>
    /// <param name="sitemapManager">
    /// The sitemap manager used to retrieve URIs for the application context.
    /// </param>
    /// <param name="applicationContext">
    /// The application context containing the current state of the application.
    /// </param>
    [Method(CrudMethod.GET)]
    [Method(CrudMethod.DELETE)]
    [Method(CrudMethod.PUT)]
    public sealed class Catalog : RestApiCrudTable<Model.Document>
    {
        private readonly string _formUri;

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

            Data = indexManager?.All<Model.Document>();
        }

        /// <summary>
        /// Retrieves a collection of options.
        /// </summary>
        /// <param name="request">The request object containing the criteria for retrieving options. Cannot be null.</param>
        /// <param name="row">The row object for which options are being retrieved. Cannot be null.</param>
        public override IEnumerable<RestApiCrudOption> GetOptions(Request request, Model.Document row)
        {
            yield return new RestApiCrudOptionHeader(request)
            {
                Label = "webexpress.webapp:header.setting.label"
            };

            yield return new RestApiCrudOptionEdit(request)
            {
                Uri = _formUri
            };

            yield return new RestApiCrudOptionSeperator(request);
            yield return new RestApiCrudOptionDelete(request);
        }

        /// <summary>
        /// Processing of the resource that was called via the get request.
        /// </summary>
        /// <param name="wql">The filtering and sorting options.</param>
        /// <param name="request">The request.</param>
        /// <returns>An enumeration of which json serializer can be serialized.</returns>
        public override IEnumerable<Model.Document> GetData(IWqlStatement<Model.Document> wql, Request request)
        {
            return wql?.Apply() ?? Enumerable.Empty<Model.Document>();
        }

        /// <summary>
        /// Processing of the resource that was called via the get request.
        /// </summary>
        /// <param name="filter">The filtering and sorting options.</param>
        /// <param name="request">The request.</param>
        /// <returns>An enumeration of which json serializer can be serialized.</returns>
        public override IEnumerable<Model.Document> GetData(string filter, Request request)
        {
            if (filter == null || filter == "null")
            {
                return Data;
            }

            return Data
                .Where
                (
                    x => x.Url.Contains(filter) ||
                    x.Title.Contains(filter)
                );
        }

        /// <summary>
        /// Updates the data record identified by the specified Id.
        /// </summary>
        /// <param name="item"> The item containing the updated data.</param>
        /// <param name="request">The HTTP request containing the update parameters.</param>
        public override void UpdateData(Model.Document item, Request request)
        {
            item.Url = request.GetParameter(nameof(Model.Document.Url))?.Value;
            item.Title = request.GetParameter(nameof(Model.Document.Title))?.Value;

            ViewModel.UpdateDocument(item);
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="id">The id of the data to delete.</param>
        /// <param name="request">The request.</param>
        public override void DeleteData(string id, Request request)
        {
            ViewModel.DeleteDocument(id);
        }
    }
}
