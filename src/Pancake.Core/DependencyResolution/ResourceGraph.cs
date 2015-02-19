using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core.DependencyResolution
{
    public class ResourceGraph
    {

        private readonly ResourceNode _rootNode = ResourceNode.RootNode(new ResourceNode[] { });


        public void AddNode(ResourceNode node)
        {
            _rootNode.AddDependency(node);
        }


        public Resource[] ResolveDependencies()
        {
            var resolved = new HashSet<ResourceNode>();
            var seen = new HashSet<ResourceNode>();
            ResolveDependencies(_rootNode, resolved, seen);
            return resolved.Take(resolved.Count-1).Select(x => x.Resource).ToArray();
        }

      

        private void ResolveDependencies(ResourceNode node, ISet<ResourceNode> resolved, ISet<ResourceNode> seen)
        {
            //from http://www.electricmonk.nl/log/2008/08/07/dependency-resolving-algorithm/
            seen.Add(node);
            foreach (var dependency in node.Dependencies)
            {
                if (resolved.Contains(dependency))
                    continue;

                if (seen.Contains(dependency))
                    throw new CircularReferenceException(node, dependency);

                ResolveDependencies(dependency, resolved, seen);
            }

            resolved.Add(node);
        }


    }

    internal class CircularReferenceException : Exception
    {
        public CircularReferenceException(ResourceNode node, ResourceNode dependency) : base(string.Format("Circular reference detected: {0} -> {1}", node.Resource, dependency.Resource))
        {

        }
    }

    public class ResolvedResources : IGrouping<Type, Resource>
    {
        private readonly Type _resourceType;
        private readonly HashSet<Resource> _resources = new HashSet<Resource>();

        public ResolvedResources(Type resourceType)
        {
            _resourceType = resourceType;
        }

        public void AddResource(Resource resource)
        {
            if(resource.GetType() != _resourceType)
                throw new InvalidOperationException("Resource Type mismatch");

            _resources.Add(resource);
        }


        public IEnumerator<Resource> GetEnumerator()
        {
            return _resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type Key => _resourceType;
    }

}