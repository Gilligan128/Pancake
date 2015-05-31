using System;

namespace Pancake.Core
{
    public interface ResourceConfig
    {
        void Contexts<TResource>(params string[] names) where TResource : Resource;
        void ContextualResource<TResource>(TResource resource, Action<ResourceRegistration<TResource>> action) where TResource : Resource, ICloneable;
        void Resource(Resource resource);
        void RegisterProvider<TResource>(ResourceProvider<TResource> resourceProvider) where TResource : Resource;
        void RegisterBehavior(ServingBehavior behavior);
    }
}