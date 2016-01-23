using System.Linq;

namespace Pancake.Core
{
    public class DefaultMatchResources : IMatchResources
    {
        public ResourcePair[] Execute(ResourceProvider provider, Resource[] resources)
        {
            var systemResources = GetSystemResources(provider, resources);
            var matchingResourcePairs = MatchResources(resources, systemResources);
            return matchingResourcePairs;
        }

        private  Resource[] GetSystemResources(ResourceProvider provider, Resource[] resources)
        {
            return provider.GetSystemResources(resources);
        }

        private  ResourcePair[] MatchResources(Resource[] resources, Resource[] systemResources)
        {
            var matchingResourcePairs =
                resources.Join(systemResources, l => l.Name, r => r.Name,
                    (should, system) => new ResourcePair { Should = should, System = system })
                    .ToArray();
            return matchingResourcePairs;
        }
    }
}