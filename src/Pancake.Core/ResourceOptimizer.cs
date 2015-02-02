namespace Pancake.Core
{
    public interface ResourceOptimizer
    {
        ResourceProvider[] Prefetch(Resource[] resource);
        void Flush(ResourceProvider[] providers);
    }
}