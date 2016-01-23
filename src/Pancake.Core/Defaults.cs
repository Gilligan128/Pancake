namespace Pancake.Core
{
    public static class Defaults
    {
        public static readonly IServe Serve = new DefaultServe(Defaults.ServeFromProvider, Defaults.GetResourceTypesByDependency, Defaults.GetResourceProvider);
        public static readonly IGetResourceProvider GetResourceProvider = new DefaultGetResourceProvider();
        public static readonly IGetResourceTypesByDependency GetResourceTypesByDependency = new DefaultGetResourceTypesByDependency();
        public static readonly IServeFromProvider ServeFromProvider = new DefaultServeFromProvider(MatchResources,
            CreateResources, SynchronizeResources, DestroyResources, StartProviderLifecycle);
        public static readonly IStartProviderLifecycle StartProviderLifecycle = new DefaultStartProviderLifecycle();
        public static readonly IDestroyResources DestroyResources = new DefaultDestroyResources();
        public static readonly ISynchronizeResources SynchronizeResources = new DefaultSynchronizeResources();
        public static readonly ICreateResources CreateResources = new DefaultCreateResources();
        public static readonly IMatchResources MatchResources = new DefaultMatchResources();
    }
}