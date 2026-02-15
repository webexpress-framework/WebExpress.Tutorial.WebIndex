using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebRestApi;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebUri;
using WebExpress.WebIndex.Queries;
using WebExpress.WebUI.WebControl;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1.Catalog
{
    /// <summary>
    /// Represents a REST API table for managing catalog entities, providing data retrieval 
    /// and option generation functionality for seed records.
    /// </summary>
    [Title("webexpress.tutorial.webindex:setting.catalog.label")]
    [Cache]
    public sealed class Table : RestApiTable<CatalogItem>
    {
        private readonly ISitemapManager _sitemapManager;
        private readonly IRestApiContext _restApiContext;
        private readonly IUri _editFormUri;
        private readonly IUri _deleteFormUri;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="sitemapManager">The sitemap manager.</param>
        /// <param name="restApiContext">The rest api context.</param>
        public Table(ISitemapManager sitemapManager, IRestApiContext restApiContext)
        {
            ArgumentNullException.ThrowIfNull(sitemapManager);
            ArgumentNullException.ThrowIfNull(restApiContext);

            _sitemapManager = sitemapManager;
            _restApiContext = restApiContext;
            _editFormUri = _sitemapManager.GetUri<WWW.Setting.Catalog.Id.Edit>(_restApiContext.ApplicationContext);
            _deleteFormUri = _sitemapManager.GetUri<WWW.Setting.Catalog.Id.Delete>(_restApiContext.ApplicationContext);
        }

        /// <summary>
        /// Retrieves a collection of options.
        /// </summary>
        /// <param name="row">
        /// The row object for which options are being retrieved. Cannot be null.
        /// </param>
        /// <param name="request">
        /// The request object containing the criteria for retrieving options. Cannot be null.
        /// </param>
        public override IEnumerable<RestApiOption> GetOptions(CatalogItem row, IRequest request)
        {
            yield return new RestApiOptionHeader(request)
            {
                Text = "webexpress.webapp:header.setting.label"
            };

            yield return new RestApiOptionEdit(request)
            {
                PrimaryAction = new ActionModal
                (
                    "modal-form",
                    _editFormUri?.SetParameters
                    (
                        new ParameterGuid(row.Id)
                    ),
                    TypeModalSize.ExtraLarge
                )
            };

            yield return new RestApiOptionSeperator(request);
            yield return new RestApiOptionDelete(request)
            {
                PrimaryAction = new ActionModal
                (
                    "modal-form",
                    _deleteFormUri?.SetParameters
                    (
                        new ParameterGuid(row.Id)
                    ),
                    TypeModalSize.Small
                )
            };
        }

        /// <summary>
        /// Returns the REST API endpoint URI associated with the specified request and workspace.
        /// </summary>
        /// <param name="row">
        /// The workspace context used to determine the appropriate REST API endpoint.
        /// </param>
        /// <param name="request">
        /// The request for which to retrieve the REST API endpoint.
        /// </param>
        /// <returns>
        /// An object representing the URI of the REST API endpoint for the given request and workspace.
        /// </returns>
        public override IUri GetRestApiForInlineEdit(CatalogItem row, IRequest request)
        {
            return _sitemapManager.GetUri<WWW.Api._1.Catalog.Index>(_restApiContext.ApplicationContext)
                .Add(new UriQuery("id", row.Id.ToString()));
        }

        /// <summary>
        /// Retrieves a queryable collection of index items that match the specified query criteria.
        /// </summary>
        /// <param name="query">
        /// An object containing the query parameters used to filter and select index items. Cannot 
        /// be null.
        /// </param>
        /// <param name="context">
        /// The context in which the query is executed. Provides additional information or constraints 
        /// for the retrieval operation. Cannot be null.
        /// </param>
        /// <param name="request">
        /// The request that provides the operational context.
        /// </param>
        /// <returns>
        /// An <see cref="IQueryable{TIndexItem}"/> representing the filtered set of index items. The 
        /// result may be empty if no items match the query.
        /// </returns>
        protected override IEnumerable<CatalogItem> Retrieve(IQuery<CatalogItem> query, IQueryContext context, IRequest request)
        {
            return query.Apply(ViewModel.Catalog.AsQueryable());
        }

        /// <summary>
        /// Applies the specified filter criteria to the given query object.
        /// </summary>
        /// <param name="filter">
        /// A string representing the filter expression to apply. The format and supported 
        /// operators depend on the implementation.
        /// </param>
        /// <param name="query">
        /// The query object to which the filter will be applied.
        /// </param>
        /// <param name="request">
        /// The request that provides the operational context for resolving
        /// the appropriate REST API URI.
        /// </param>
        protected override void Filter(string filter, IQuery<CatalogItem> query, IRequest request)
        {
            if (filter is null || filter == "null")
            {
                return;
            }

            query.Where
            (
                x => x.Url.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
            );

            if (request.GetParameter<ParameterGuid>() is Parameter category)
            {
                query.Where
                (
                    x => x.Url.Contains(category.Value.ToLower(), StringComparison.CurrentCultureIgnoreCase)
                );
            }
        }
    }
}
