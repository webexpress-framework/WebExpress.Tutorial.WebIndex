using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebRestApi;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1.Catalog
{
    /// <summary>
    /// Represents a REST API endpoint for operations on catalog entities.
    /// </summary>
    [Cache]
    public sealed class Index : RestApiCrud<Model.CatalogItem>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Index()
        {
        }

        /// <summary>
        /// Retrieves a collection of index items of type TIndexItem.
        /// </summary>
        /// <returns>
        /// An enumerable collection of TIndexItem objects. The collection is empty if 
        /// no items are available.
        /// </returns>
        protected override IEnumerable<Model.CatalogItem> Retrieve()
        {
            return ViewModel.Catalog;
        }

        /// <summary>
        /// Retrieves a workspace identified by the specified key for update operations.
        /// </summary>
        /// <param name="id">
        /// The unique identifier that identifies the workspace to retrieve. Cannot be null or empty.
        /// </param>
        /// <param name="request">
        /// The request context containing additional information for the retrieval operation.
        /// </param>
        /// <returns>
        /// An object containing the workspace associated with the specified key.
        /// </returns>
        protected override IRestApiCrudResultRetrieve<Model.CatalogItem> RetrieveForUpdate(string id, IRequest request)
        {
            return new RestApiCrudResultRetrieve<Model.CatalogItem>()
            {
                Title = I18N.Translate(request, "webexpress.tutorial.webindex:setting.catalog.edit.header"),
                Data = ViewModel.Catalog.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault()
            };
        }

        /// <summary>
        /// Retrieves the workspace entity identified by the specified ID in preparation for deletion.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the workspace to retrieve for deletion. Cannot 
        /// be null or empty.
        /// </param>
        /// <param name="request">
        /// The request context containing additional information for 
        /// the retrieval operation.
        /// </param>
        /// <returns>
        /// An object containing the workspace entity and related information required 
        /// for the delete operation.
        /// </returns>
        protected override IRestApiCrudResultRetrieveDelete<Model.CatalogItem> RetrieveForDelete(string id, IRequest request)
        {
            var data = ViewModel.Catalog.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
            return new RestApiCrudResultRetrieveDelete<Model.CatalogItem>()
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
