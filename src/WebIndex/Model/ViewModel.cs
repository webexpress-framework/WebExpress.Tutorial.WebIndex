using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebExpress.WebApp.WebIndex;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPage;
using WebExpress.WebIndex.Wql.Predicate;
using WebExpress.WebUI.WebNotification;

namespace WebExpress.Tutorial.WebIndex.Model
{
    /// <summary>
    /// Provides methods for initializing and interacting with the index.
    /// </summary>
    internal static class ViewModel
    {
        private static IComponentHub _componentHub;
        private static IndexManager _indexManager;

        /// <summary>
        /// Gets all seeds.
        /// </summary>
        public static IEnumerable<Seed> Seeds => _indexManager.All<Seed>();

        /// <summary>
        /// Gets the catalog items.
        /// </summary>
        public static IEnumerable<CatalogItem> Catalog => _indexManager.All<CatalogItem>();

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="componentHub">The component hub used to manage components.</param>
        /// <param name="applicationContext">The application context.</param>
        public static void Initialization(IComponentHub componentHub, IApplicationContext applicationContext)
        {
            _componentHub = componentHub;
            _indexManager = _componentHub?.GetComponentManager<IndexManager>();

            // indexing the data
            _indexManager.Create<Seed>(CultureInfo.CurrentCulture, WebExpress.WebIndex.IndexType.Storage);
            _indexManager.Create<CatalogItem>(CultureInfo.CurrentCulture, WebExpress.WebIndex.IndexType.Storage);

            // site('<host>') stands for the pages of one site; the ids are collected when a
            // statement is read, so a search sees the catalog as it is at that moment
            _indexManager.RegisterWqlPredicate(new WqlPredicate<CatalogItem>("site", arguments =>
            {
                var host = arguments.Count == 1 ? arguments[0] as string : null;
                if (string.IsNullOrWhiteSpace(host))
                {
                    throw new ArgumentException("site expects the host of a site.");
                }

                var ids = _indexManager.All<CatalogItem>()
                    .Where(x => IsOnSite(x.Url, host))
                    .Select(x => x.Id)
                    .ToHashSet();

                return x => ids.Contains(x.Id);
            }));


            if (!_indexManager.All<Seed>().Any())
            {
#if DEBUG
                AddSeed(new Seed() { Id = Guid.NewGuid(), Url = "https://www.gutenberg.org/" });
                AddSeed(new Seed() { Id = Guid.NewGuid(), Url = "https://www.wikipedia.org/" });
#endif
            }
        }

        /// <summary>
        /// Clears all documents from the catalog associated with the specified request.
        /// </summary>
        /// <param name="context"> The render context containing the request information.</param>
        /// <summary>
        /// Tells whether an address belongs to a site, its subdomains included.
        /// </summary>
        /// <param name="url">The address of a page.</param>
        /// <param name="host">The host of the site, such as wikipedia.org.</param>
        /// <returns>True when the page is on the site.</returns>
        private static bool IsOnSite(string url, string host)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return false;
            }

            return uri.Host.Equals(host, StringComparison.OrdinalIgnoreCase)
                || uri.Host.EndsWith("." + host, StringComparison.OrdinalIgnoreCase);
        }

        public static void ClearCatalog(IRenderContext context)
        {
            _componentHub?.GetComponentManager<IndexManager>()?.Clear<CatalogItem>();

            _componentHub?.GetComponentManager<NotificationManager>()?.AddNotification
            (
                applicationContext: context?.PageContext?.ApplicationContext,
                message: I18N.Translate(context, "webexpress.tutorial.webindex:setting.catalog.cleared"),
                durability: 2000,
                icon: context?.PageContext?.Route.Concat("/assets/img/crawler.svg")?.ToString()
            );
        }

        /// <summary>
        /// Adds an initial page to the index.
        /// </summary>
        /// <param name="seed">The initial seed uri to add to the index.</param>
        public static void AddSeed(Seed seed)
        {
            _componentHub?.GetComponentManager<IndexManager>()?.Insert(seed);
        }

        /// <summary>
        /// Updates the specified seed in the system by inserting it into the index manager.
        /// </summary>
        /// <param name="seed">The seed to be updated. Cannot be null.</param>
        public static void UpdateSeed(Seed seed)
        {
            _componentHub?.GetComponentManager<IndexManager>()?.Update(seed);
        }

        /// <summary>
        /// Deletes a seed with the specified identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the seed to delete. Must be a valid GUID.
        /// </param>
        public static void DeleteSeed(Guid id)
        {
            _componentHub?.GetComponentManager<IndexManager>()?.Delete<Model.Seed>(id);
        }

        /// <summary>
        /// Updates the specified document in the system by inserting it into the index manager.
        /// </summary>
        /// <param name="document">The document to be updated. Cannot be null.</param>
        public static void UpdateDocument(CatalogItem document)
        {
            _componentHub?.GetComponentManager<IndexManager>()?.Update(document);
        }

        /// <summary>
        /// Deletes a document with the specified identifier from the index.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the document to delete. Must be a valid GUID.
        /// </param>
        public static void DeleteDocument(Guid id)
        {
            _componentHub?.GetComponentManager<IndexManager>()?.Delete<Model.CatalogItem>(id);
        }

        /// <summary>
        /// Retrieves a collection from the index that match the specified search string.
        /// </summary>
        /// <param name="search">The search string to match against the index.</param>
        /// <returns>An enumerable that match the search string.</returns>
        public static IEnumerable<CatalogItem> Retrieve(string search)
        {
            return _componentHub?.GetComponentManager<IndexManager>()?.Retrieve<CatalogItem>(search)?.Where(x => x is not null);
        }
    }
}
