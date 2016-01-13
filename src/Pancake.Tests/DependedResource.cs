using Pancake.Core;

namespace Pancake.Tests
{
    public class DependedResource : TestResource
    {
        public Dependency<FinalDependendResource> Dependency { get; set; }
    }

    public class FinalDependendResource : TestResource
    {
    }
}