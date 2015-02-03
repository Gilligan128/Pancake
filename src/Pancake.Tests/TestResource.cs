using System.Collections.Generic;
using Pancake.Core;

namespace Pancake.Tests
{
    public class TestResource : Resource
    {
        public string Description { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Description;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}