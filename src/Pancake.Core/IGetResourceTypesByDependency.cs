using System;

namespace Pancake.Core
{
    public interface IGetResourceTypesByDependency
    {
        Type[] Execute(ResourceCatalog resourceCatalog);
    }
}