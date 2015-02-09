namespace Pancake.Core
{
    public interface ResourceConfig
    {
        void Resource(Resource resource);
        void OptimizeWith(ResourceOptimizer resourceOptimizer);
        void RegisterProvider<TResource>(ResourceProvider<TResource> resourceProvider) where TResource : Resource;
    }
}