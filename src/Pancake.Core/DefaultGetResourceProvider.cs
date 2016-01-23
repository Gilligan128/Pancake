using System;

namespace Pancake.Core
{
    public class DefaultGetResourceProvider : IGetResourceProvider
    {
        public ResourceProvider Execute(ResourceCatalog resourceCatalog, Type resourceType)
        {
            return resourceCatalog.ProviderFor(resourceType);
        }
    }
}