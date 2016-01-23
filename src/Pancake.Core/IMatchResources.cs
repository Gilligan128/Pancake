namespace Pancake.Core
{
    public interface IMatchResources
    {
        ResourcePair[] Execute(ResourceProvider provider, Resource[] resources);
    }
}