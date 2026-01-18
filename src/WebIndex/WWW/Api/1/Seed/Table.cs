using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebRestApi;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebUri;
using WebExpress.WebUI.WebControl;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1.Seed
{
    /// <summary>
    /// Represents a REST API table for managing seed entities, providing data retrieval 
    /// and option generation functionality for seed records.
    /// </summary>
    [Title("webexpress.tutorial.webindex:setting.seed.label")]
    [Cache]
    public sealed class Table : RestApiTable<Model.Seed>
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
            _sitemapManager = sitemapManager;
            _restApiContext = restApiContext;
            _editFormUri = _sitemapManager.GetUri<WWW.Setting.Seed.Id.Edit>(_restApiContext.ApplicationContext);
            _deleteFormUri = _sitemapManager.GetUri<WWW.Setting.Seed.Id.Delete>(_restApiContext.ApplicationContext);
        }

        /// <summary>
        /// Retrieves a collection of options.
        /// </summary>
        /// <param name="request">
        /// The request object containing the criteria for retrieving options. Cannot be null.
        /// </param>
        /// <param name="row">
        /// The row object for which options are being retrieved. Cannot be null.
        /// </param>
        public override IEnumerable<RestApiOption> GetOptions(IRequest request, Model.Seed row)
        {
            yield return new RestApiOptionHeader(request)
            {
                Label = "webexpress.webapp:header.setting.label"
            };

            yield return new RestApiOptionEdit(request)
            {
                Uri = _editFormUri?.SetParameters
                (
                    new ParameterGuid(row.Id)
                )?
                    .ToString(),
                Modal = new ModalTarget("modal-form", TypeModalSize.ExtraLarge)
            };

            yield return new RestApiOptionSeperator(request);
            yield return new RestApiOptionDelete(request)
            {
                Uri = _deleteFormUri?.SetParameters
                (
                    new ParameterGuid(row.Id)
                )?
                    .ToString(),
                Modal = new ModalTarget("modal-form", TypeModalSize.Small)
            };
        }

        /// <summary>
        /// Returns the REST API endpoint URI associated with the specified request and workspace.
        /// </summary>
        /// <param name="request">
        /// The request for which to retrieve the REST API endpoint.
        /// </param>
        /// <param name="row">
        /// The workspace context used to determine the appropriate REST API endpoint.
        /// </param>
        /// <returns>
        /// An object representing the URI of the REST API endpoint for the given request and workspace.
        /// </returns>
        public override IUri GetRestApiForInlineEdit(IRequest request, Model.Seed row)
        {
            return _sitemapManager.GetUri<WWW.Api._1.Seed.Index>(_restApiContext.ApplicationContext)
                .Add(new UriQuery("id", row.Id.ToString()));
        }

        /// <summary>
        /// Retrieves a collection of objects based on the specified WQL statement and request.
        /// </summary>
        /// <param name="filter">
        /// The filter used to query the data. This parameter defines the filtering and 
        /// selection criteria.
        /// </param>
        /// <param name="request">
        /// The request context containing additional information for the operation.
        /// </param>
        /// <returns>
        /// An enumerable containing the objects that match the query criteria.
        /// </returns>
        public override IEnumerable<Model.Seed> GetData(string filter, IRequest request)
        {
            var data = Model.ViewModel.Seeds;

            if (request.GetParameter<ParameterGuid>() is Parameter category)
            {
                data = data.Where
                (
                    x => x.Url.Contains(category.Value?.ToLower(), StringComparison.CurrentCultureIgnoreCase)
                );
            }

            if (filter == null || filter == "null")
            {
                return data;
            }

            return data.Where
            (
                x => x.Url.Contains(filter, StringComparison.InvariantCultureIgnoreCase)
            );
        }
    }
}
