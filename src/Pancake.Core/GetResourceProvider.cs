using System;

namespace Pancake.Core
{
    public class GetResourceProvider : IGetResourceProvider
    {
        public ResourceProvider Execute(ResourceCatalog resourceCatalog, Type resourceType)
        {
            return resourceCatalog.ProviderFor(resourceType);
        }
    }
}