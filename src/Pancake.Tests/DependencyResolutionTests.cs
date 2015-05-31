using System;
using System.Collections.Generic;
using System.Linq;
using Pancake.Core;
using Pancake.Core.DependencyResolution;
using Should;
using Should.Core.Assertions;

namespace Pancake.Tests
{
    public class DependencyResolutionTests
    {
        public void should_resolve_resources_based_on_dependencies()
        {
            var sut = new ResourceGraph();
            var resourceA = new ResourceA
            {
                Name = "A"
            };
            var resourceB = new ResourceB
            {
                Name = "B"
            };
            var resourceC = new ResourceC
            {
                Name = "C"
            };
            var resourceANode = new DependencyNode<Resource>(resourceA);
            var resourceBNode = new DependencyNode<Resource>(resourceB);
            var resourceCNode = new DependencyNode<Resource>(resourceC);
            resourceANode.AddDependency(resourceBNode);
            resourceBNode.AddDependency(resourceCNode);
            sut.AddNode(resourceANode);

            var resolved = sut.ResolveDependencies();
            resolved[0].ShouldEqual(resourceC);
            resolved[1].ShouldEqual(resourceB);
            resolved[2].ShouldEqual(resourceA);
        }

        public void should_detect_circular_dependencies()
        {
            var sut = new ResourceGraph();
            var nodeA = new DependencyNode<Resource>(new TestResource {Name = "A"});
            var nodeB = new DependencyNode<Resource>(new TestResource {Name = "B"});
            var nodeC = new DependencyNode<Resource>(new TestResource {Name = "C"});
            nodeA.AddDependency(nodeB);
            nodeB.AddDependency(nodeC);
            nodeC.AddDependency(nodeA);
            sut.AddNode(nodeA);

            Assert.Throws<CircularReferenceException>(() => sut.ResolveDependencies());
        }

        public void should_throw_exception_if_dependency_is_not_in_catalog()
        {
            var sut = new CreateResourceGraph();
            Resource[] resources =
            {
                new DependingResource
                {
                    Dependency = "depended"
                }
            };

            Assert.Throws<MissingDependencyException>(() => sut.Execute(resources));
        }

        public void should_create_dependency_graph_from_dependency_properties()
        {
            var sut = new CreateResourceGraph();

            var resources = new Resource[]
            {new DependingResource {Name = "A", Dependency = "B"}, new DependedResource {Name = "B"}};
            var graph = sut.Execute(resources);

            graph.ResolveDependencies().ShouldContain(resources[0]);
            graph.ResolveDependencies().ShouldContain(resources[1]);
        }
        
        public void should_create_dependency_graph_from_contextual_dependency_property()
        {
            var sut = new CreateResourceGraph();

            var resources = new Resource[]
            {
                new ResourceD { Name = "D" },
                new ContextedResource { Name = "Context", ContextD = new ResourceContext<ResourceD>("D") }
            };

            var graph = sut.Execute(resources);

            var resolved = graph.ResolveDependencies();

            resolved.Count().ShouldEqual(2);
            resolved.ShouldContain(resources[0]);
            resolved.ShouldContain(resources[1]);
        }

        public void ignores_blank_dependencies()
        {
            var resources = new[]
            {new DependingResource {Name = "A", Dependency = ""}, new DependingResource {Name = "B", Dependency = null}};
            var sut = new CreateResourceGraph();

            var graph = sut.Execute(resources);

            var resolved = graph.ResolveDependencies();
            resolved.Count().ShouldEqual(2);
            resolved.ShouldContain(resources[0]);
            resolved.ShouldContain(resources[1]);
        }

        public void should_create_type_graph()
        {     
            var sut = new CreateTypeGraph();

            var graph = sut.Execute(new Resource[] { new DependingResource { Name = "A", Dependency = "B" }, new DependedResource { Name = "B" } });

            var resolved = graph.ResolveDependencies();
            resolved.ShouldContain(typeof(DependedResource));
            resolved.ShouldContain(typeof(DependingResource));
        }

        private class ContextedResource : TestResource
        {
            public ResourceContext<ResourceD> ContextD { get; set; }
        }

        private class ResourceA : TestResource
        {
            public Dependency<ResourceB> Dependency { get; set; }
        };

        private class ResourceB : TestResource
        {
            public Dependency<ResourceC> DependencyC { get; set; }
            public Dependency<ResourceD> DependencyD { get; set; }
        };
    }

    public class DependingArrayResource : TestResource
    {
        public Dependency<DependedResource>[] Dependencies { get; set; }
    }

    internal class ResourceC : TestResource
    {
        public Dependency<ResourceD> DependencyD { get; set; }
    }

    internal class ResourceD : TestResource
    {
    }

    internal class CircularA : TestResource
    {
        public Dependency<CircularB> Dependency { get; set; }
    }

    internal class CircularB : TestResource
    {
        public Dependency<CircularC> Dependency { get; set; }
    }

    internal class CircularC : TestResource
    {
        public Dependency<CircularA> Dependency { get; set; }
    }
}