using System.Linq;

namespace Pancake.Core
{
    public class DefaultServeFromProvider : IServeFromProvider
    {
        private readonly ICreateResources _createResources;
        private readonly IDestroyResources _destroyResources;
        private readonly IMatchResources _matchResources;
        private readonly ISynchronizeResources _synchronizeResources;
        private readonly IStartProviderLifecycle _providerLifcycle;

        public DefaultServeFromProvider(IMatchResources matchResources, ICreateResources createResources,
            ISynchronizeResources synchronizeResources, IDestroyResources destroyResources,
            IStartProviderLifecycle providerLifcycle)
        {
            _matchResources = matchResources;
            _createResources = createResources;
            _synchronizeResources = synchronizeResources;
            _destroyResources = destroyResources;
            _providerLifcycle = providerLifcycle;
        }

        public void Execute(ResourceProvider provider, Resource[] resources)
        {
            var matchingResourcePairs = _matchResources.Execute(provider, resources);
            var missingResources = resources.Except(matchingResourcePairs.Select(m => m.Should)).ToArray();
            using (_providerLifcycle.Execute(provider))
            {
                _createResources.Execute(missingResources, provider);
                _synchronizeResources.Execute(matchingResourcePairs, provider);
                _destroyResources.Execute(matchingResourcePairs, provider);
            }
            ;
        }
    }
}