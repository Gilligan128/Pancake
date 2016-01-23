namespace Pancake.Core
{
    internal interface IMatchResources
    {
        ResourcePair[] Execute(ResourceProvider provider, Resource[] resources);
    }
}