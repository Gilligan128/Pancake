using Pancake.Core;
using Raven.Client;

namespace Pancake.RavenDB
{
    public static class PancakeRavenExtensions
    {
        public static PancakeApi CreatePancakes(this IDocumentSession session)
        {
            var defaultServeFromProvider = new DefaultServeFromProvider(new DefaultMatchResources(),
                new DefaultCreateResources(), new DefaultSynchronizeResources(), new DefaultDestroyResources(),
                new DefaultStartProviderLifecycle());
            return
                new PancakeApi(new DefaultServe(new RavenServeFromProvider(defaultServeFromProvider, session),
                    new DefaultGetResourceTypesByDependency(), new DefaultGetResourceProvider()));
        }
    }
}