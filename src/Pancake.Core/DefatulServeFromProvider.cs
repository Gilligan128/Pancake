using System;
using System.Collections.Generic;
using System.Linq;
using Pancake.Core.DependencyResolution;

namespace Pancake.Core
{
    public class DefatulServeFromProvider
    {
        private readonly DefaultGetResourceTypes _defaultGetResourceTypes;

        public DefatulServeFromProvider(DefaultGetResourceTypes defaultGetResourceTypes)
        {
            _defaultGetResourceTypes = defaultGetResourceTypes;
        }

        public void Execute(ResourceCatalog resourceCatalog)
        {
            var resourceTypes = _defaultGetResourceTypes.Execute(resourceCatalog);
            foreach (var resourceType in resourceTypes)
            {
                var resources = resourceCatalog.Resources.Where(x => x.GetType() == resourceType).ToArray();
                var provider = resourceCatalog.ProviderFor(resourceType);

                ServeFromProvider(provider, resources);
            }
        }

        private void ServeFromProvider(ResourceProvider provider, Resource[] resources)
        {
            var systemResources = provider.GetSystemResources(resources);
            var matchingResourcePairs = MatchResources(resources, systemResources);
            var missingResources = resources.Except(Enumerable.Select<ResourcePair, Resource>(matchingResourcePairs, m => m.Should));

            provider.Prefetch();
            CreateMissingResources(missingResources, provider);
            SynchronizeOutofSyncResources(matchingResourcePairs, provider);
            DestroyResourcesThatShouldBeAbsent(matchingResourcePairs, provider);
            provider.Flush();
        }

        private static ResourcePair[] MatchResources(Resource[] resources, Resource[] systemResources)
        {
            var matchingResourcePairs =
                resources.Join(systemResources, l => l.Name, r => r.Name,
                    (should, system) => new ResourcePair { Should = should, System = system })
                    .ToArray();
            return matchingResourcePairs;
        }

        private static void DestroyResourcesThatShouldBeAbsent(ResourcePair[] matchingResourcePairs, ResourceProvider provider)
        {
            foreach (
                var resource in matchingResourcePairs.Select(r => r.Should).Where(r => r.Ensure == Ensure.Absent))
            {
                provider.Destroy(resource);
            }
        }

        private static void SynchronizeOutofSyncResources(ResourcePair[] matchingResourcePairs, ResourceProvider provider)
        {
            foreach (
                var matchingResource in
                    matchingResourcePairs.Where(x => x.Should.Ensure == Ensure.Present)
                        .Where(x => !x.Should.GetSynchronizationComponents().SequenceEqual(x.System.GetSynchronizationComponents())))
            {
                provider.Synchronize(matchingResource.Should, matchingResource.System);
            }
        }

        private static void CreateMissingResources(IEnumerable<Resource> missingResources, ResourceProvider provider)
        {
            foreach (var missingResource in missingResources.Where(r => r.Ensure == Ensure.Present))
            {
                provider.Create(missingResource);
            }
        }
    }
}