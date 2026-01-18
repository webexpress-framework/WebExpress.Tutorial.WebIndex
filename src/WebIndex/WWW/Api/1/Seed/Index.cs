using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.Tutorial.WebIndex.Model;
using WebExpress.WebApp.WebRestApi;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebRestApi;

namespace WebExpress.Tutorial.WebIndex.WWW.Api._1.Seed
{
    /// <summary>
    /// Represents a REST API endpoint for operations on seed entities.
    /// </summary>
    [Cache]
    public sealed class Index : RestApiCrud<Model.Seed>
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
        protected override IEnumerable<Model.Seed> Retrieve()
        {
            return ViewModel.Seeds;
        }

        /// <summary>
        /// Retrieves the data required to create a new workspace entity.
        /// </summary>
        /// <param name="request">
        /// The request context containing parameters and metadata for the retrieval operation.
        /// </param>
        /// <returns>
        /// An object containing the information necessary to initialize a new workspace for creation.
        /// </returns>
        protected override IRestApiCrudResultRetrieve<Model.Seed> RetrieveForCreate(IRequest request)
        {
            return new RestApiCrudResultRetrieve<Model.Seed>()
            {
                Title = I18N.Translate(request, "webexpress.tutorial.webindex:setting.seed.add.header")
            };
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
        protected override IRestApiCrudResultRetrieve<Model.Seed> RetrieveForUpdate(string id, IRequest request)
        {
            return new RestApiCrudResultRetrieve<Model.Seed>()
            {
                Title = I18N.Translate(request, "webexpress.tutorial.webindex:setting.seed.edit.header"),
                Data = ViewModel.Seeds.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault()
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
        protected override IRestApiCrudResultRetrieveDelete<Model.Seed> RetrieveForDelete(string id, IRequest request)
        {
            var data = ViewModel.Seeds.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
            return new RestApiCrudResultRetrieveDelete<Model.Seed>()
            {
                Data = data,
                Title = I18N.Translate(request, "webexpress.tutorial.webindex:setting.seed.delete.header"),
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
        protected override IRestApiValidationResult Validate(Model.Seed existingItem, RestApiCrudFormData payload, IRequest request)
        {
            return base.Validate(existingItem, payload, request);
        }

        /// <summary>
        /// Persists the newly created resource.
        /// Override this method in derived classes to implement the actual
        /// persistence logic and return a result describing the creation.
        /// </summary>
        /// <param name="fieldMap">
        /// The dynamic payload containing the fields required to create the resource.
        /// </param>
        /// <param name="request">
        /// The HTTP request providing additional context for the creation process.
        /// </param>
        /// <param name="newItem">
        /// When the method returns, contains the newly created index item,
        /// or the default value if creation was not successful.
        /// </param>
        /// <returns>
        /// A result object containing information about the create operation,
        /// including the created resource.
        /// </returns>
        protected override IRestApiCrudResultCreate Create(RestApiCrudFormData fieldMap, IRequest request, out Model.Seed newItem)
        {
            newItem = new Model.Seed();
            fieldMap.BindTo(newItem);

            ViewModel.AddSeed(newItem);

            return new RestApiCrudResultCreate();
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
        protected override IRestApiCrudResultUpdate Update(Model.Seed existingItem, RestApiCrudFormData payload, IRequest request)
        {
            var res = base.Update(existingItem, payload, request);
            ViewModel.UpdateSeed(existingItem);

            return res;
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
        protected override IRestApiCrudResultDelete Delete(Model.Seed existingItem, IRequest request)
        {
            ViewModel.DeleteSeed(existingItem.Id);

            return base.Delete(existingItem, request);
        }
    }
}
