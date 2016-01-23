using System.Linq;

namespace Pancake.Core
{
    public class DefaultSynchronizeResources : ISynchronizeResources
    {
        public void Execute(ResourcePair[] matchingResourcePairs, ResourceProvider provider)
        {
            foreach (
                var matchingResource in
                    matchingResourcePairs.Where(x => x.Should.Ensure == Ensure.Present)
                        .Where(x => provider.ShouldSynchronize(x.Should, x.System)))
            {
                provider.Synchronize(matchingResource.Should, matchingResource.System);
            }
        }
    }
}