using NHibernate;
using Pancake.Core;

namespace Pancake.NHibernate
{
    public abstract class NHibernateResourceProvider<TResource> : ResourceProvider<TResource>, NHibernateResourceProvider where TResource : Resource
    {
        public ISession Session { get; set; }
    }

    public interface NHibernateResourceProvider : ResourceProvider
    {
        ISession Session { get; set; }
    }

}