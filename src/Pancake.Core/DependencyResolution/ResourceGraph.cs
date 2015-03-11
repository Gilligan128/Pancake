using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core.DependencyResolution
{
    public class ResourceGraph : DependencyGraph<Resource>
    {
       
    }

    public class DependencyGraph<T>
    {
        private readonly DependencyNode<T> _rootNode = DependencyNode<T>.RootNode(new DependencyNode<T>[] { });

        public void AddNode(DependencyNode<T> node)
        {
            _rootNode.AddDependency(node);
        }

        public T[] ResolveDependencies()
        {
            var resolved = new HashSet<T>();
            var seen = new HashSet<T>();
            ResolveDependencies(_rootNode, resolved, seen);
            return resolved.Take(resolved.Count-1).ToArray();
        }

        private void ResolveDependencies(DependencyNode<T> node, ISet<T> resolved, ISet<T> seen)
        {
            //from http://www.electricmonk.nl/log/2008/08/07/dependency-resolving-algorithm/
            seen.Add(node.Element);
            foreach (var dependency in node.Dependencies)
            {
                if (resolved.Contains(dependency.Element))
                    continue;

                if (seen.Contains(dependency.Element))
                    throw new CircularReferenceException(node.Element.ToString(), dependency.Element.ToString());

                ResolveDependencies(dependency, resolved, seen);
            }

            resolved.Add(node.Element);
        }

       
    }

    public class CircularReferenceException : Exception
    {
        public CircularReferenceException(string node, string dependency) : base(string.Format("Circular reference detected: {0} -> {1}", node, dependency))
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