using Pancake.Core.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;

namespace Pancake.Core
{
    public class PancakeApi
    {
        private readonly ResourceCatalog _catalog = new ResourceCatalog();
        private readonly IServe _defaultServe;

        public PancakeApi()
        {
            _defaultServe = new DefaultServe();
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
            Action action = ServeCore;
            var behaviorChain = _catalog.ServingBehaviors.Reverse().Aggregate(action,
                (next, behavior) => () => behavior.Serve(_catalog, next));
            behaviorChain();
        }

        private void ServeCore()
        {
            _defaultServe.Execute(_catalog);
        }

        public void Resources(Action<ResourceConfig> cfg)
        {
            cfg(_catalog);
        }
    }

    public interface IServe
    {
        void Execute(ResourceCatalog resourceCatalog);
    }

    public class ResourcePair
    {
        public Resource Should { get; set; }
        public Resource System { get; set; }
    }
}