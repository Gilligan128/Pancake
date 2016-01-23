namespace Pancake.Core
{
    internal interface IDestroyResources
    {
        void Execute(ResourcePair[] matchingResourcePairs, ResourceProvider provider);
    }
}