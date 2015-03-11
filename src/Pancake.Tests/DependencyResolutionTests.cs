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


        private void ExpectException<TException>(Action doStuff) where TException : Exception
        {
            try
            {
                doStuff();

                throw new InvalidOperationException(string.Format("Expected {0}", typeof (TException).Name));
            }
            catch (TException ex)
            {
            }
        }

        private class ResourceA : Resource
        {
            public Dependency<ResourceB> Dependency { get; set; }

            public override IEnumerable<object> GetEqualityComponents()
            {
                yield return Name;
            }
        };

        private class ResourceB : Resource
        {
            public Dependency<ResourceC> DependencyC { get; set; }
            public Dependency<ResourceD> DependencyD { get; set; }

            public override IEnumerable<object> GetEqualityComponents()
            {
                yield return Name;
            }
        };
    }

    internal class ResourceC : Resource
    {
        public Dependency<ResourceD> DependencyD { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    internal class ResourceD : Resource
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    internal class CircularA : Resource
    {
        public Dependency<CircularB> Dependency { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    internal class CircularB : Resource
    {
        public Dependency<CircularC> Dependency { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    internal class CircularC : Resource
    {
        public Dependency<CircularA> Dependency { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}