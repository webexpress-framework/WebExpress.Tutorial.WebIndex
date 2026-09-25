using System.Reflection;

namespace HalloWorld.App
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Call arguments.</param>
        /// <returns>The exit code, not zero when the application could not start.</returns>
        static int Main(string[] args)
        {
            var app = new WebExpress.WebCore.WebEx()
            {
                Name = Assembly.GetExecutingAssembly().GetName().Name
            };

            return app.Execution(args);
        }
    }
}