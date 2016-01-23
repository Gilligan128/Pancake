namespace Pancake.Core
{
    public interface IServeFromProvider
    {
        void Execute(ResourceProvider provider, Resource[] resources);
    }
}