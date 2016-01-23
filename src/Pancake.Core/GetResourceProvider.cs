using System;

namespace Pancake.Core
{
    public class GetResourceProvider
    {
        public ResourceProvider Execute(ResourceCatalog resourceCatalog, Type resourceType)
        {
            return resourceCatalog.ProviderFor(resourceType);
        }
    }
}