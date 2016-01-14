using Pancake.Core;
using Raven.Client;
using System;
using System.Linq;

namespace Pancake.RavenDB
{
    /// <summary>
    /// Represents a serving behaviour to apply a document session to applicable resource providers.
    /// </summary>
    public class UseRavenDBSessionBehavior : ServingBehavior
    {
        private readonly IDocumentSession _session;

        /// <summary>
        /// Creates an instance of UseRavenDBSessionBehavior.
        /// </summary>
        /// <param name="session">The document session to apply.</param>
        public UseRavenDBSessionBehavior(IDocumentSession session)
        {
            _session = session;
        }

        public void Serve(ResourceCatalog context, Action next)
        {
            foreach (var provider in context.Providers.OfType<RavenDBResourceProvider>())
            {
                provider.Session = _session;
            }

            next();
        }
    }
}
