namespace Pancake.Core
{
    public interface ISynchronizeResources
    {
        void Execute(ResourcePair[] matchingResourcePairs, ResourceProvider provider);
    }
}