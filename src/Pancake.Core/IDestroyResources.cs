namespace Pancake.Core
{
    public interface IDestroyResources
    {
        void Execute(ResourcePair[] matchingResourcePairs, ResourceProvider provider);
    }
}