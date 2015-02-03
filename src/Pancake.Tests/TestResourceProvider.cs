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

        public TestResource[] GetSystemResources(TestResource[] resources)
        {
            return _systemResourcs.Join(resources, l => l.Name, r => r.Name, (resource, testResource) => resource).ToArray();
        }

        public void Create(TestResource resouce)
        {
            _createdResources.Add(resouce);
        }

        public void Destroy(TestResource resource)
        {
            _destroyedResources.Add(resource);
        }

        public void Synchronize(TestResource resource, TestResource systemResource)
        {
            _synchronizedResources.Add(resource);
        }

        public Resource[] GetSystemResources(Resource[] resourceSet)
        {
            return GetSystemResources(resourceSet.OfType<TestResource>().ToArray()).OfType<Resource>().ToArray();
        }

        public void Synchronize(Resource expectedResource, Resource systemResource)
        {
            Synchronize((TestResource)expectedResource, (TestResource)systemResource);
        }

        public void Flush()
        {
        }

        public void Create(Resource missingResource)
        {
            Create((TestResource)missingResource);
        }

        public void Destroy(Resource resource)
        {
            Destroy((TestResource)resource);
        }
    }
}