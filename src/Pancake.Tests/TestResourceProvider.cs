using System.Collections.Generic;
using System.Linq;
using Pancake.Core;

namespace Pancake.Tests
{
    public class TestResourceProvider : ResourceProvider<TestResource>
    {
        private readonly List<TestResource> _createdResources = new List<TestResource>();
        private readonly List<TestResource> _destroyedResources = new List<TestResource>();
        private readonly List<TestResource> _synchronizedResources = new List<TestResource>();
        private readonly List<TestResource> _systemResourcs = new List<TestResource>();
        public bool WasCreated { get; protected set; }
        public bool IsDifferent => true;
        public bool WasDestroyed { get; protected set; }
        public bool WasSynchronized { get; protected set; }
        public TestResource SystemResource { get; set; }
        public TestResource[] SynchronizedResources => _synchronizedResources.ToArray();
        public TestResource[] DestroyedResources => _destroyedResources.ToArray();
        public TestResource[] CreatedResources => _createdResources.ToArray();

        public void AddSystemResource(TestResource resource)
        {
            _systemResourcs.Add(resource);
        }

        public void Synchronize()
        {
            WasSynchronized = true;
        }

        public override TestResource[] GetSystemResources(TestResource[] resources)
        {
            return _systemResourcs.Join(resources, l => l.Name, r => r.Name, (resource, testResource) => resource).ToArray();
        }

        public override void Create(TestResource resouce)
        {
            _createdResources.Add(resouce);
        }

        public override void Destroy(TestResource resource)
        {
            _destroyedResources.Add(resource);
        }

        public override void Synchronize(TestResource resource, TestResource systemResource)
        {
            _synchronizedResources.Add(resource);
        }

        public override void Flush()
        {
        }
    }
}