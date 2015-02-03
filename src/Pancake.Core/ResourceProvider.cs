using System;
using System.Linq;

namespace Pancake.Core
{
    public interface ResourceProvider {
        Resource[] GetSystemResources(Resource[] resourceSet);
        void Synchronize(Resource expectedResource, Resource systemResource);
        void Flush();
        void Create(Resource missingResource);
        void Destroy(Resource resource);
    }


    public interface ResourceProvider<TResource> : ResourceProvider where TResource : Resource 
    {
    }
}