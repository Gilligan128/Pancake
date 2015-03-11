using System.Collections.Generic;
using Pancake.Core;

namespace Pancake.Tests
{
    internal class TestResource : Resource
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}