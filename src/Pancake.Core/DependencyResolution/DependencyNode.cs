using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core.DependencyResolution
{
    public class DependencyNode<T>
    {
        public T Element { get; private set; }
        private readonly List<DependencyNode<T>> _dependencies = new List<DependencyNode<T>>();

        public DependencyNode(T element)
        {
            Element = element;
        }

        public DependencyNode<T>[] Dependencies => _dependencies.ToArray();


        public void AddDependency(DependencyNode<T> node)
        {
            _dependencies.Add(node);
        }


        public static DependencyNode<T> RootNode(DependencyNode<T>[] nodes)
        {
            var root = new DependencyNode<T>(default(T));
            foreach (var resourceNode in nodes)
            {
                root.AddDependency(resourceNode);
            }
            return root;
        }
    }

}