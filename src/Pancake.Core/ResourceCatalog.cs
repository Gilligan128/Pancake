using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Pancake.Core
{
    public class ResourceCatalog : ResourceConfig
    {
        private readonly HashSet<Resource> _resources = new HashSet<Resource>();
        public Resource[] Resources => _resources.ToArray();
        public ServingBehavior[] ServingBehaviors => _behaviors.ToArray();
        public ResourceProvider[] Providers => _providers.Values.ToArray();

        public Dictionary<Type, ResourceProvider> _providers  =  new Dictionary<Type, ResourceProvider>();
        private readonly HashSet<ServingBehavior> _behaviors = new HashSet<ServingBehavior>();

        public ResourceProvider ProviderFor(Type key)
        {
            return _providers[key];
        }

        public void Resource(Resource resource)
        {
            _resources.Add(resource);
        }


        public void RegisterProvider<TResource>(ResourceProvider<TResource> resourceProvider) where TResource : Resource
        {
            _providers.Add(typeof(TResource), resourceProvider);
        }

        public void RegisterBehavior(ServingBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        void ResourceConfig.Resources(params Resource[] resources)
        {
            foreach (var resource in resources)
            {
                _resources.Add(resource);
            }
        }

        public ResourceProvider<TResource> ProviderFor<TResource>() where TResource : Resource
        {
            return (ResourceProvider<TResource>) _providers[typeof (TResource)];
        }
        
    }
}