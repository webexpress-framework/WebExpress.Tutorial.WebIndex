using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Linq;
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

        /// <summary>
        /// Starts the web crawling process for the given request.
        /// </summary>
        /// <param name="request">The request object containing the necessary parameters for the crawling process.</param>
        public static void Crawl(IRequest request)
        {
            WebEx.ComponentHub.GetComponentManager<IndexManager>()?.Clear<Document>();
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

                    var count = WebEx.ComponentHub.GetComponentManager<IndexManager>().Count<Document>();
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
            if (url is null || url.StartsWith("mailto") || url.StartsWith("tel") || WebEx.ComponentHub.GetComponentManager<IndexManager>().Retrieve<Document>($"url='{url}'").Apply().Any())
            {
                return;
            }

            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var title = doc.DocumentNode.SelectSingleNode("//head/title")?.InnerText;
                var language = doc.DocumentNode.SelectSingleNode("//html[@lang]")?.GetAttributeValue("lang", "unbekannt");
                var content = doc.DocumentNode?.InnerText;

                if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(content))
                {
                    var webPageData = new Document
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        Title = title,
                        Content = content,
                        MetaData = new MetaData()
                        {
                            Encoding = doc.Encoding.WebName,
                            Language = language,
                            ContentLength = content.Length,
                            Create = DateTime.Now,
                        }
                    };

                    WebEx.ComponentHub.GetComponentManager<IndexManager>().Insert(webPageData);

                    var links = doc.DocumentNode.SelectNodes("//a[@href]");
                    foreach (var link in links ?? Enumerable.Empty<HtmlNode>())
                    {
                        var hrefValue = link.GetAttributeValue("href", string.Empty);
                        if (!string.IsNullOrEmpty(hrefValue) && Uri.IsWellFormedUriString(hrefValue, UriKind.Absolute))
                        {
                            Urls.TryAdd(Guid.NewGuid(), hrefValue);
                        }
                    }
                }

            }
            catch
            {
            }

            //ComponentManager.GetComponent<LogManager>()?.DefaultLog.Info($"add url '{url}' to index");
        }
    }
}
