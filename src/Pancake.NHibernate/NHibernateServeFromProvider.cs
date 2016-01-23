using System.Linq;
using NHibernate;
using NHibernate.Util;
using Pancake.Core;

namespace Pancake.NHibernate
{
    public class NHibernateServeFromProvider : IServeFromProvider
    {
        private readonly IServeFromProvider _inner;
        private readonly ISession _session;

        public NHibernateServeFromProvider(IServeFromProvider inner, ISession session)
        {
            _inner = inner;
            _session = session;
        }

        public void Execute(ResourceProvider provider, Resource[] resources)
        {
            Enumerable.Repeat(provider, 1).OfType<NHibernateResourceProvider>()
                .ForEach(x => x.Session = _session);

            _inner.Execute(provider, resources);
        }
    }
}