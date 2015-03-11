﻿using System;
using System.Collections.Generic;
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
            Action action = ServeCore;
            var behaviorChain = _catalog.ServingBehaviors.Reverse().Aggregate(action,
                (next, behavior) => () => behavior.Serve(_catalog, next));
            behaviorChain();
        }

        private void ServeCore()
        {
            foreach (var resourceSet in _catalog.Resources.GroupBy(r => r.GetType())
                .Select(r => new { provider = _catalog.ProviderFor(r.Key), resources = r.ToArray()})
                .OrderBy(kvp => Array.IndexOf(_catalog.Providers, kvp.provider)))
            {
                var provider = resourceSet.provider;
                var resources = resourceSet.resources;
                var systemResources = provider.GetSystemResources(resources);
                var matchingResourcePairs = MatchResources(resources, systemResources);
                var missingResources = resources.Except(matchingResourcePairs.Select(m => m.Should));

                CreateMissingResources(missingResources, provider);
                SynchronizeOutofSyncResources(matchingResourcePairs, provider);
                DestroyResourcesThatShouldBeAbsent(matchingResourcePairs, provider);

                provider.Flush();
            }
        }

        private static ResourcePair[] MatchResources(Resource[] resources, Resource[] systemResources)
        {
            var matchingResourcePairs =
                resources.Join(systemResources, l => l.Name, r => r.Name,
                    (should, system) => new ResourcePair {Should = should, System = system})
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
                        .Where(x => !x.Should.Equals(x.System)))
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

        public void Resources(Action<ResourceConfig> cfg)
        {
            cfg(_catalog);
        }
    }

    internal class ResourcePair
    {
        public Resource Should { get; set; }
        public Resource System { get; set; }
    }
}