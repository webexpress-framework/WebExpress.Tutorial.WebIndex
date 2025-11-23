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
    /// Represents a REST API endpoint for CRUD operations on Seed entities.
    /// </summary>
    [Method(CrudMethod.GET)]
    [Method(CrudMethod.DELETE)]
    [Method(CrudMethod.PUT)]
    public sealed class Seed : RestApiCrudTable<Model.Seed>
    {
        private readonly string _formUri;

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

            Data = indexManager?.All<Model.Seed>();
        }

        /// <summary>
        /// Retrieves a collection of options.
        /// </summary>
        /// <param name="request">
        /// The request object containing the criteria for retrieving options. Cannot be null.
        /// </param>
        /// <param name="row">The row object for which options are being retrieved. Cannot be null.</param>
        public override IEnumerable<RestApiCrudOption> GetOptions(Request request, Model.Seed row)
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
        public override IEnumerable<Model.Seed> GetData(IWqlStatement<Model.Seed> wql, Request request)
        {
            return wql?.Apply() ?? Enumerable.Empty<Model.Seed>();
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
                return Data;
            }

            return Data
                .Where
                (
                    x => x.Url.Contains(filter)
                );
        }

        /// <summary>
        /// Performs validation before updating data.
        /// </summary>
        /// <param name="item"> The item containing the updated data.</param>
        /// <param name="request">The HTTP request containing input data and parameters.</param>
        /// <returns>
        /// A <see cref="RestApiValidationResult"/> containing any validation errors 
        /// encountered during the update process. If the operation completes successfully, 
        /// the result will contain no errors.
        /// </returns>
        public override RestApiValidationResult ValidateUpdateData(Model.Seed item, Request request)
        {
            return new RestApiValidator(request)
                .Require(nameof(Model.Seed.Url))
                .MinLength(nameof(Model.Seed.Url), 3)
                .Result;
        }

        /// <summary>
        /// Updates the data record identified by the specified ID.
        /// </summary>
        /// <param name="item"> The item containing the updated data.</param>
        /// <param name="request">The HTTP request containing the update parameters.</param>
        public override void UpdateData(Model.Seed item, Request request)
        {
            item.Url = request.GetParameter(nameof(Model.Seed.Url))?.Value;

            ViewModel.UpdateSeed(item);
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="id">The id of the data to delete.</param>
        /// <param name="request">The request.</param>
        public override void DeleteData(string id, Request request)
        {
            ViewModel.DeleteSeed(id);
        }
    }
}
