using System;
using System.Linq;

namespace Pancake.Core
{
    public interface ResourceProvider
    {
        Resource[] GetSystemResources(Resource[] resourceSet);
        void Synchronize(Resource expectedResource, Resource systemResource);
        void Flush();
        void Create(Resource missingResource);
        void Destroy(Resource resource);
        bool ShouldSynchronize(Resource desiredResource, Resource systemResource);
        void Prefetch();
    }


    public abstract class ResourceProvider<TResource> : ResourceProvider where TResource : Resource
    {
        public Resource[] GetSystemResources(Resource[] resourceSet)
        {
            return GetSystemResources(resourceSet.OfType<TResource>().ToArray()).OfType<Resource>().ToArray();
        }

        public void Synchronize(Resource expectedResource, Resource systemResource)
        {
            Synchronize((TResource) expectedResource, (TResource)systemResource);
        }

        public virtual void Flush()
        {
        }

        public void Create(Resource missingResource)
        {
            Create((TResource) missingResource);
        }

        public void Destroy(Resource resource)
        {
            Destroy((TResource) resource);
        }

        public bool ShouldSynchronize(Resource desiredResource, Resource systemResource)
        {
            return ShouldSynchronize((TResource) desiredResource,(TResource)systemResource);
        }

        public abstract TResource[] GetSystemResources(TResource[] resources);
        public abstract void Create(TResource resource);
        public abstract void Destroy(TResource resource);
        public abstract void Synchronize(TResource desiredResource, TResource systemResource);
        public abstract bool ShouldSynchronize(TResource desiredResource, TResource systemResource);

        public virtual void Prefetch()
        {
            
        }
    }
}