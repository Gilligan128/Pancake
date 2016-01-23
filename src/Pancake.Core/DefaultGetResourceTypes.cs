﻿using System;
using Pancake.Core.DependencyResolution;

namespace Pancake.Core
{
    public class DefaultGetResourceTypes
    {
        public Type[] Execute(ResourceCatalog resourceCatalog)
        {
            var graph = new CreateTypeGraph().Execute(resourceCatalog.Resources);
            var resourceTypes = graph.ResolveDependencies();
            return resourceTypes;
        }
    }
}