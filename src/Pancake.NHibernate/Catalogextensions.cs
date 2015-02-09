using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Pancake.Core;

namespace Pancake.NHibernate
{
    public static class CatalogExtensions
    {
        public static void UseNHibernate(this ResourceCatalog catalog, ISession nhibernateSession)
        {
            catalog.RegisterBehavior(new UseNHibernateSessionBehavior(nhibernateSession));
        }

    }
}
