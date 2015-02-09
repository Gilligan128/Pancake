using System;
using System.Linq;

namespace Pancake.Core
{
    public class PancakeApi
    {
        private readonly ResourceCatalog _catalog = new ResourceCatalog();

        public void Configure(Action<ResourceConfig> configure)
        {
            configure(_catalog);
        }

        public void Serve()
        {
            foreach (var resourceSet in _catalog.Resources.GroupBy(r => r.GetType()))
            {
                var provider = _catalog.ProviderFor(resourceSet.Key);
                var resources = resourceSet.ToArray();
                var systemResources = provider.GetSystemResources(resources);
                var matchingResourcePairs =
                    resources.Join(systemResources, l => l.Name, r => r.Name, (should, system) => new {should, system})
                        .ToArray();
                var missingResources = resources.Except(matchingResourcePairs.Select(m => m.should));

                foreach (var missingResource in missingResources.Where(r => r.Ensure == Ensure.Present))
                {
                    provider.Create(missingResource);
                }

                foreach (
                    var matchingResource in
                        matchingResourcePairs.Where(x => x.should.Ensure == Ensure.Present)
                            .Where(x => !x.should.Equals(x.system)))
                {
                    provider.Synchronize(matchingResource.should, matchingResource.system);
                }

                foreach (
                    var resource in matchingResourcePairs.Select(r => r.should).Where(r => r.Ensure == Ensure.Absent))
                {
                    provider.Destroy(resource);
                }

                provider.Flush();
            }
        }
    }
}