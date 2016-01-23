using NHibernate;
using Pancake.Core;

namespace Pancake.NHibernate
{
    public static class PancakeNHibernateExtensions
    {
        public static PancakeApi CreatePancakes(this ISession session)
        {
            var defaultServeFromProvider = new DefaultServeFromProvider(new DefaultMatchResources(),
                new DefaultCreateResources(), new DefaultSynchronizeResources(), new DefaultDestroyResources(),
                new DefaultStartProviderLifecycle());
            return
                new PancakeApi(new DefaultServe(new NHibernateServeFromProvider(defaultServeFromProvider, session),
                    new DefaultGetResourceTypesByDependency(), new DefaultGetResourceProvider()));
        }
    }
}