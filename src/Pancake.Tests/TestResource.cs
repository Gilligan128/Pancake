using System.Collections.Generic;
using Pancake.Core;
using System;

namespace Pancake.Tests
{
    public class TestContextualResource : TestResource, ICloneable
    {
        public ResourceContext<TestResource> Context { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override IEnumerable<object> GetSynchronizationComponents()
        {
            yield return Name;
            yield return Context.Name;
        }
    }

    public class TestResource : Resource
    {
        public override IEnumerable<object> GetSynchronizationComponents()
        {
            yield return Name;
        }
    }
}