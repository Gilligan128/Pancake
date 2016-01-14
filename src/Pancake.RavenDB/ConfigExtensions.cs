using Pancake.Core;
using Raven.Client;

namespace Pancake.RavenDB
{
    /// <summary>
    /// Represents config extension methods.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Registers a RavenDB session behaviour on the catalog.
        /// </summary>
        /// <param name="config">The resource config.</param>
        /// <param name="documentSession">The document session.</param>
        public static void UseRavenDB(this ResourceConfig config, IDocumentSession documentSession)
        {
            config.RegisterBehavior(new UseRavenDBSessionBehavior(documentSession));
        }
    }
}
