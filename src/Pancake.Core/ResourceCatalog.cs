using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core
{
    public class ResourceCatalog : ResourceConfig
    {
        private readonly HashSet<Resource> _resources = new HashSet<Resource>();
        public Resource[] Resources => _resources.ToArray();
        public ResourceOptimizer Optimizer { get; protected set; }

        public void Resource(Resource resource)
        {
            _resources.Add(resource);
        }

        public void OptimizeWith(ResourceOptimizer resourceProvider)
        {
            Optimizer = resourceProvider;
        }
    }
}