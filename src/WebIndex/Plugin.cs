using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.Tutorial.WebIndex
{
    /// <summary>
    /// Represents a plugin for the WebIndex application.
    /// </summary>
    [Name("webexpress.tutorial.webindex:plugin.name")]
    [Description("webexpress.tutorial.webindex:plugin.description")]
    [Icon("/assets/img/webindex.svg")]
    [Application<Application>]
    [Dependency("webexpress.webapp")]
    public sealed class Plugin : IPlugin
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Plugin()
        {
        }

        /// <summary>
        /// Called when the plugin starts working. Run is called concurrently.
        /// </summary>
        public void Run()
        {
        }
    }
}