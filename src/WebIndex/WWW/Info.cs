using System.Linq;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebScope;
using WebExpress.WebCore;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPage;
using WebExpress.WebUI.WebControl;
using WebExpress.WebUI.WebIcon;

namespace WebExpress.Tutorial.WebIndex.WWW
{
    /// <summary>
    /// Represents the info page for the tutorial.
    /// </summary>
    [WebIcon<IconInfoCircle>]
    [Title("webexpress.tutorial.webindex:infopage.label")]
    [Scope<IScopeGeneral>]
    public sealed class Info : IPage<VisualTreeWebApp>, IScopeGeneral
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class.
        /// </summary>
        public Info()
        {
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="renderContext">The context for rendering the page.</param>
        /// <param name="visualTree">The visual tree of the web application.</param>
        public void Process(IRenderContext renderContext, VisualTreeWebApp visualTree)
        {
            var webexpress = WebEx.ComponentHub.PluginManager.Plugins.Where(x => x.PluginId.ToString() == "webexpress.webapp").FirstOrDefault();
            var webindex = WebEx.ComponentHub.PluginManager.Plugins.Where(x => x.Assembly == GetType().Assembly).FirstOrDefault();

            visualTree.Content.MainPanel.AddPrimary(new ControlImage()
            {
                Uri = renderContext.PageContext.ApplicationContext
                    .Route.Concat("assets/img/webindex.svg")
                    .ToUri(),
                Width = 200,
                Height = 200,
                HorizontalAlignment = TypeHorizontalAlignment.Right
            });

            var card = new ControlPanelCard()
            {
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Null, PropertySpacing.Space.Two)
            };

            card.Add(new ControlText()
            {
                Text = I18N.Translate(renderContext, "webexpress.tutorial.webindex:app.name"),
                Format = TypeFormatText.H3
            });

            card.Add(new ControlText()
            {
                Text = I18N.Translate(renderContext, "webexpress.tutorial.webindex:app.description"),
                Format = TypeFormatText.Paragraph
            });

            card.Add(new ControlText()
            {
                Text = I18N.Translate(renderContext, "webexpress.tutorial.webindex:app.about"),
                Format = TypeFormatText.H3
            });

            card.Add(new ControlText()
            {
                Text = string.Format
                (
                    I18N.Translate(renderContext, "webexpress.tutorial.webindex:app.version.label"),
                    I18N.Translate(renderContext, webindex?.PluginName),
                    webindex?.Version,
                    webexpress?.PluginName,
                    webexpress?.Version
                ),
                TextColor = new PropertyColorText(TypeColorText.Primary)
            });

            visualTree.Content.MainPanel.AddPrimary(card);
        }
    }
}
