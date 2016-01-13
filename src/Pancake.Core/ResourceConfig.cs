namespace Pancake.Core
{
    public interface ResourceConfig
    {
        void Resource(Resource resource);
        void RegisterProvider<TResource>(ResourceProvider<TResource> resourceProvider) where TResource : Resource;
        void RegisterBehavior(ServingBehavior behavior);
        void Resources(params Resource[] resources);
    }
}