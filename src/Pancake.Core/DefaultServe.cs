using System.Linq;

namespace Pancake.Core
{
    public class DefaultServe : IServe
    {
        private readonly IGetResourceProvider _getResourceProvider;
        private readonly IGetResourceTypesByDependency _defaultGetResourceTypesByDependency;
        private readonly IServeFromProvider _serveFromProvider;

        public DefaultServe(IServeFromProvider serveFromProvider,
            IGetResourceTypesByDependency defaultGetResourceTypesByDependency, IGetResourceProvider getResourceProvider)
        {
            _serveFromProvider = serveFromProvider;
            _defaultGetResourceTypesByDependency = defaultGetResourceTypesByDependency;
            _getResourceProvider = getResourceProvider;
        }

        public void Execute(ResourceCatalog resourceCatalog)
        {
            var resourceTypes = _defaultGetResourceTypesByDependency.Execute(resourceCatalog);
            foreach (var resourceType in resourceTypes)
            {
                var resources = resourceCatalog.Resources.Where(x => x.GetType() == resourceType).ToArray();
                var provider = _getResourceProvider.Execute(resourceCatalog, resourceType);

                _serveFromProvider.Execute(provider, resources);
            }
        }
    }
}