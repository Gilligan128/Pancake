using System.Collections.Generic;
using Pancake.Core;

namespace Pancake.Tests
{
    public class TestResource : Resource
    {
        public override IEnumerable<object> GetSynchronizationComponents()
        {
            yield return Name;
        }
    }
}