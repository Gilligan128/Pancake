using System;
using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core
{
    public class ResourceCatalog : ResourceConfig
    {
        private readonly HashSet<Resource> _resources = new HashSet<Resource>();
        public Dictionary<Type, ResourceProvider> _providers = new Dictionary<Type, ResourceProvider>();
        public Resource[] Resources => _resources.ToArray();
        public ResourceProvider[] Providers => _providers.Values.ToArray();

        public void Resource(Resource resource)
        {
            _resources.Add(resource);
        }

        public void RegisterProvider<TResource>(ResourceProvider<TResource> resourceProvider) where TResource : Resource
        {
            _providers.Add(typeof (TResource), resourceProvider);
        }

        void ResourceConfig.Resources(params Resource[] resources)
        {
            foreach (var resource in resources)
            {
                _resources.Add(resource);
            }
        }

        public ResourceProvider ProviderFor(Type key)
        {
            return _providers[key];
        }

        public ResourceProvider<TResource> ProviderFor<TResource>() where TResource : Resource
        {
            return (ResourceProvider<TResource>) _providers[typeof (TResource)];
        }
    }
}