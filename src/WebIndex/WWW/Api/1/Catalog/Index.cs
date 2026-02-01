using System.Collections.Generic;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebRestApi;
using WebExpress.WebIndex.Queries;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1.Catalog
{
    /// <summary>
    /// Represents a REST API endpoint for operations on catalog entities.
    /// </summary>
    [Cache]
    public sealed class Index : RestApiCrud<CatalogItem>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Index()
        {
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
        /// <returns>
        /// A collection representing the filtered set of index items. 
        /// The collection may be empty if no items match the query.
        /// </returns>
        protected override IEnumerable<CatalogItem> Retrieve(IQuery<CatalogItem> query, IQueryContext context)
        {
            return query.Apply(ViewModel.Catalog.AsQueryable());
        }

        /// <summary>
        /// Retrieves an item by its identifier for update operations.
        /// </summary>
        /// <param name="query">
        /// An object containing the query parameters used to filter and select index items. Cannot 
        /// be null.
        /// </param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A result instance representing the data and metadata required
        /// to initialize a new item for creation.
        /// </returns>
        protected override IRestApiCrudResultRetrieve<CatalogItem> RetrieveForUpdate(IQuery<CatalogItem> query, IRequest request)
        {
            using var context = CreateContext();
            var data = Retrieve(query, context)
                .FirstOrDefault();

            return new RestApiCrudResultRetrieve<Model.CatalogItem>()
            {
                Title = I18N.Translate(request, "webexpress.tutorial.webindex:setting.catalog.edit.header"),
                Data = data
            };
        }

        /// <summary>
        /// Retrieves the item identified by the specified ID for the purpose of confirming 
        /// or preparing a delete operation.
        /// </summary>
        /// <param name="query">
        /// An object containing the query parameters used to filter and select index items. Cannot 
        /// be null.
        /// </param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// A result instance representing the data and metadata required
        /// to initialize a new item for creation.
        /// </returns>
        protected override IRestApiCrudResultRetrieveDelete<CatalogItem> RetrieveForDelete(IQuery<CatalogItem> query, IRequest request)
        {
            using var context = CreateContext();
            var data = Retrieve(query, context)
                .FirstOrDefault();

            return new RestApiCrudResultRetrieveDelete<CatalogItem>()
            {
                Data = data,
                Title = I18N.Translate(request, "webexpress.tutorial.webindex:setting.catalog.delete.header"),
                ConfirmItem = data?.Id.ToString()
            };
        }

        /// <summary>
        /// Validate the data for create or update operations. When creating, existingItem will 
        /// be null and proposedItem contains the values to create. When updating, existingItem 
        /// is the currently persisted entity and proposedItem contains the incoming values to 
        /// validate.
        /// </summary>
        /// <param name="existingItem">
        /// The currently persisted item (null for create).
        /// </param>
        /// <param name="payload">
        /// The dynamic payload containing updated fields.
        /// </param>
        /// <param name="request">
        /// The HTTP request providing additional context.
        /// </param>
        /// <returns>
        /// An IRestApiValidationResult indicating validation success or errors.
        /// </returns>
        protected override IRestApiValidationResult Validate(Model.CatalogItem existingItem, RestApiCrudFormData payload, IRequest request)
        {
            return base.Validate(existingItem, payload, request);
        }

        /// <summary>
        /// Updates the data record.
        /// </summary>
        /// <param name="existingItem">
        /// The currently persisted item.
        /// </param>
        /// <param name="payload">
        /// The dynamic payload containing updated fields.
        /// </param>
        /// <param name="request">
        /// The HTTP request providing additional context.
        /// </param>
        protected override IRestApiCrudResultUpdate Update(Model.CatalogItem existingItem, RestApiCrudFormData payload, IRequest request)
        {
            return base.Update(existingItem, payload, request);
        }

        /// <summary>
        /// Deletes the specified resource.
        /// </summary>
        /// <param name="existingItem">
        /// The currently persisted item that is to be deleted.
        /// </param>
        /// <param name="request">
        /// The HTTP request providing additional context for the delete operation.
        /// </param>
        /// <returns>
        /// A result object containing information about the delete operation.
        /// </returns>
        protected override IRestApiCrudResultDelete Delete(Model.CatalogItem existingItem, IRequest request)
        {
            ViewModel.DeleteDocument(existingItem.Id);

            return base.Delete(existingItem, request);
        }
    }
}
