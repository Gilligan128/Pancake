using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core.DependencyResolution
{
    public class CreateTypeGraph 
    {
        public DependencyGraph<Type> Execute(Resource[] resources)
        {
            var resourceGraph = new DependencyGraph<Type>();

            foreach (var resource in resources)
            {
                var resourceNode = new DependencyNode<Type>(resource.GetType());
                resourceGraph.AddNode(resourceNode);
                AddDependencies(resourceNode, resource, resources);
            }

            return resourceGraph;
        }

        private void AddDependencies(DependencyNode<Type> node, Resource resource, Resource[] resources)
        {
            var dependencyKeys = new FindDependencyProperties().Execute(resource);
            foreach (var dependencyKey in dependencyKeys)
            {
                var matchingResource =
                    resources.FirstOrDefault(
                        x => x.GetType() == dependencyKey.ResourceType && x.Name == dependencyKey.Name);
                if (matchingResource == null)
                    throw new MissingDependencyException(dependencyKey.Name, resource.Name);
                node.AddDependency(new DependencyNode<Type>(matchingResource.GetType()));
            }
        }
    }
}
