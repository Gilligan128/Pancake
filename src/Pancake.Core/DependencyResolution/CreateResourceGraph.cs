using System.Linq;

namespace Pancake.Core.DependencyResolution
{
    public class CreateResourceGraph
    {
        private readonly Resource[] _resources;

        public CreateResourceGraph(Resource[] resources)
        {
            _resources = resources;

        }

        public ResourceGraph Execute()
        {
            var resourceGraph = new ResourceGraph();
            var resourceNodes = _resources.Select(x => x.GetType()).Distinct();
            
            return resourceGraph;
        }
    }
}

//Resource dependency scenarios
//