using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Pancake.Core;

namespace Pancake.NHibernate
{
    public class UseNHibernateSessionBehavior : ServingBehavior
    {
        private readonly ISession _session;

        public UseNHibernateSessionBehavior(ISession session)
        {
            _session = session;
        }

        public void Serve(ResourceCatalog context, Action next)
        {
            foreach (var provider in context.Providers.OfType<NHibernateResourceProvider>())
            {
                provider.Session = _session;
            }

            next();
        }
    }
}
