using System;
using System.Linq;

namespace Pancake.Core
{
    public class PancakeApi
    {
        private readonly ResourceCatalog _catalog = new ResourceCatalog();
        private readonly IServe _defaultServe;

        public PancakeApi()
        {
            _defaultServe =
                new DefaultServe(
                    new DefaultServeFromProvider(new DefaultMatchResources(), new DefaultCreateResources(),
                        new DefaultSynchronizeResources(), new DefaultDestroyResources(),
                        new DefaultStartProviderLifecycle()), new DefaultGetResourceTypesByDependency(),
                    new DefaultGetResourceProvider());
        }

        public PancakeApi(IServe server)
        {
            _defaultServe = server;
        }

        public void Configure(Action<ResourceConfig> configure)
        {
            configure(_catalog);
        }

        public void Serve()
        {
            _defaultServe.Execute(_catalog);
        }

        public void Resources(Action<ResourceConfig> cfg)
        {
            cfg(_catalog);
        }
    }
}