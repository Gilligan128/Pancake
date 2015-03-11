using System.Collections.Generic;
using System.Linq;
using Pancake.Core;

namespace Pancake.Tests
{
    public class TestResourceProvider<TResource> : ResourceProvider<TResource> where TResource : Resource
    {
        private readonly List<TResource> _createdResources = new List<TResource>();
        private readonly List<TResource> _destroyedResources = new List<TResource>();
        private readonly List<TResource> _synchronizedResources = new List<TResource>();
        private readonly List<TResource> _systemResourcs = new List<TResource>();
        public bool WasCreated { get; protected set; }
        public bool IsDifferent => true;
        public bool WasDestroyed { get; protected set; }
        public bool WasSynchronized { get; protected set; }
        public TResource[] SynchronizedResources => _synchronizedResources.ToArray();
        public TResource[] DestroyedResources => _destroyedResources.ToArray();
        public TResource[] CreatedResources => _createdResources.ToArray();

        public void AddSystemResource(TResource resource)
        {
            _systemResourcs.Add(resource);
        }

        public void Synchronize()
        {
            WasSynchronized = true;
        }

        public override TResource[] GetSystemResources(TResource[] resources)
        {
            return _systemResourcs.Join(resources, l => l.Name, r => r.Name, (resource, testResource) => resource).ToArray();
        }

        public override void Create(TResource resouce)
        {
            _createdResources.Add(resouce);
        }

        public override void Destroy(TResource resource)
        {
            _destroyedResources.Add(resource);
        }

        public override void Synchronize(TResource resource, TResource systemResource)
        {
            _synchronizedResources.Add(resource);
        }

        public override void Flush()
        {
        }
    }
}