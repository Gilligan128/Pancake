using System.Linq;

namespace Pancake.Core
{
    public class DefaultDestroyResources : IDestroyResources
    {
        public void Execute(ResourcePair[] matchingResourcePairs, ResourceProvider provider)
        {
            foreach (
                var resource in matchingResourcePairs.Select(r => r.Should).Where(r => r.Ensure == Ensure.Absent))
            {
                provider.Destroy(resource);
            }
        }
    }
}