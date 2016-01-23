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
                        .Where(x => !x.Should.GetSynchronizationComponents().SequenceEqual(x.System.GetSynchronizationComponents())))
            {
                provider.Synchronize(matchingResource.Should, matchingResource.System);
            }
        }
    }
}