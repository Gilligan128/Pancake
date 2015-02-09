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
    }


    public abstract class ResourceProvider<TResource> : ResourceProvider where TResource : Resource
    {
        public Resource[] GetSystemResources(Resource[] resourceSet)
        {
            return GetSystemResources(resourceSet.OfType<TResource>().ToArray()).OfType<Resource>().ToArray();
        }

        public void Synchronize(Resource expectedResource, Resource systemResource)
        {
            Synchronize((TResource)expectedResource, (TResource)systemResource);
        }

        public virtual void Flush()
        {

        }


        public void Create(Resource missingResource)
        {
            Create((TResource)missingResource);
        }

        public void Destroy(Resource resource)
        {
            Destroy((TResource)resource);
        }

        public abstract TResource[] GetSystemResources(TResource[] resources);
        public abstract void Create(TResource resouce);
        public abstract void Destroy(TResource resource);
        public abstract void Synchronize(TResource resource, TResource systemResource);
    }
}