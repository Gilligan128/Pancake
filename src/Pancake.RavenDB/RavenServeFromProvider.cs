using System;
using System.Linq;
using Pancake.Core;
using Raven.Client;

namespace Pancake.RavenDB
{
    public class RavenServeFromProvider : IServeFromProvider
    {
        private readonly IServeFromProvider _inner;
        private readonly IDocumentSession _session;

        public RavenServeFromProvider(IServeFromProvider inner, IDocumentSession session)
        {
            _inner = inner;
            _session = session;
        }

        public void Execute(ResourceProvider provider, Resource[] resources)
        {
            foreach (var provide in Enumerable.Repeat(provider, 1).OfType<RavenDBResourceProvider>())
            {
                provide.Session = _session;
            }

            _inner.Execute(provider, resources);
        }
    }
}