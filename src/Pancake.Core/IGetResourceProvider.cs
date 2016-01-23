using System;

namespace Pancake.Core
{
    public interface IGetResourceProvider
    {
        ResourceProvider Execute(ResourceCatalog resourceCatalog, Type resourceType);
    }
}