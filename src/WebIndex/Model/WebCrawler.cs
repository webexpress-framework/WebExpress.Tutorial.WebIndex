using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebExpress.WebApp.WebIndex;
using WebExpress.WebCore;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebUI.WebNotification;

namespace WebExpress.Tutorial.WebIndex.Model
{
    /// <summary>
    /// WebCrawler class that crawls web pages and stores the data.
    /// </summary>
    internal static class WebCrawler
    {
        public static ConcurrentDictionary<Guid, string> Urls = new();
        private const uint MAX_COUNT = 100u;
        private static readonly HttpClient Client = new(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });

        /// <summary>
        /// Starts the web crawling process for the given request.
        /// </summary>
        /// <param name="request">The request object containing the necessary parameters for the crawling process.</param>
        public static void Crawl(IRequest request)
        {
            WebEx.ComponentHub.GetComponentManager<IndexManager>()?.Clear<CatalogItem>();
            var applicationContext = WebEx.ComponentHub.ApplicationManager.GetApplication<Application>();

            foreach (var initial in WebEx.ComponentHub.GetComponentManager<IndexManager>().All<Seed>())
            {
                Urls.TryAdd(initial.Id, initial.Url);
            }

            var notification = WebEx.ComponentHub.GetComponentManager<NotificationManager>()?.AddNotification
            (
                applicationContext: applicationContext,
                message: I18N.Translate(request, "webexpress.tutorial.webindex:crawl.start"),
                durability: -1,
                icon: applicationContext.Route.Concat("/assets/img/crawler.svg")?.ToString()
            );

            var task = new Task(() =>
            {
                while (!Urls.IsEmpty && Urls.TryRemove(Urls.Keys.FirstOrDefault(), out string url))
                {
                    Crawl(url);

                    var count = WebEx.ComponentHub.GetComponentManager<IndexManager>().Count<CatalogItem>();
                    var trimAndEllipsisUrl = url.Length > 80 ? string.Concat(url.AsSpan(0, 77), "...") : url;

                    notification.Message = I18N.Translate
                    (
                        request,
                        "webexpress.tutorial.webindex:crawl.add",
                        trimAndEllipsisUrl,
                        $"({count}/{MAX_COUNT})"
                    );
                    notification.Progress = (int)(count * 100 / MAX_COUNT);

                    if (count >= MAX_COUNT)
                    {
                        break;
                    }
                }
            });

            task.Start();
        }

        /// <summary>
        /// Crawls a specific URL and stores the data.
        /// </summary>
        /// <param name="url">The URL to crawl.</param>
        public static void Crawl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
                WebEx.ComponentHub.GetComponentManager<IndexManager>().Retrieve<CatalogItem>($"url='{url}'").Any())
            {
                return;
            }

            try
            {
                using var response = Client.GetAsync(uri).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                var page = WebCrawlerPage.ReadAsync(response.Content).GetAwaiter().GetResult();
                var title = page.Title;
                var language = page.Language;
                var content = page.Content;

                if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(content))
                {
                    var webPageData = new CatalogItem
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        Title = title,
                        Content = content,
                        MetaData = new MetaData()
                        {
                            Encoding = page.EncodingName,
                            Language = language,
                            ContentLength = content.Length,
                            Create = DateTime.Now,
                        }
                    };

                    WebEx.ComponentHub.GetComponentManager<IndexManager>().Insert(webPageData);

                    foreach (var link in page.Links)
                    {
                        Urls.TryAdd(Guid.NewGuid(), link);
                    }
                }

            }
            catch
            {
            }
        }
    }
}
