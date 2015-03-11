using System.Linq;

namespace Pancake.Core.DependencyResolution
{

    

    public class CreateResourceGraph
    {

        public CreateResourceGraph()
        {

        }

        public ResourceGraph Execute(Resource[] resources)
        {
            var resourceGraph = new ResourceGraph();
            var resourceNodes = resources.Select(r => new DependencyNode<Resource>(r));

            foreach (var resourceNode in resourceNodes)
            {
                resourceGraph.AddNode(resourceNode);
                AddDependencies(resourceNode, resources);
            }

            return resourceGraph;
        }

        private void AddDependencies(DependencyNode<Resource> node, Resource[] resources)
        {
            var dependencyKeys = new FindDependencyProperties().Execute(node.Element);
            foreach (var dependencyKey in dependencyKeys)
            {
                var matchingResource =
                    resources.FirstOrDefault(
                        x => x.GetType() == dependencyKey.ResourceType && x.Name == dependencyKey.Name);
                if(matchingResource == null)
                    throw new MissingDependencyException(dependencyKey.Name, node.Element.Name);
                node.AddDependency(new DependencyNode<Resource>(matchingResource));
            }
        }

    }
}

//Resource dependency scenarios
//