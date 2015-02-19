using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core.DependencyResolution
{
    public class ResourceNode
    {
        public Resource Resource { get; private set; }
        private readonly List<ResourceNode> _dependencies = new List<ResourceNode>();

        public ResourceNode(Resource resource)
        {
            Resource = resource;
        }

        public ResourceNode[] Dependencies => _dependencies.ToArray();


        public void AddDependency(ResourceNode node)
        {
            _dependencies.Add(node);
        }


        public static ResourceNode RootNode(ResourceNode[] nodes)
        {
            var root = new ResourceNode(new RootResource());
            foreach (var resourceNode in nodes)
            {
                root.AddDependency(resourceNode);
            }
            return root;
        }

        private class RootResource : Resource
        {
            public RootResource()
            {
                Name = "Root.Root";
            }

            public override IEnumerable<object> GetEqualityComponents()
            {
                yield return Name;
            }
        }

    }

}