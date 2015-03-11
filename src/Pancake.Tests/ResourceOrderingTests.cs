using System.Collections.Generic;
using System.Linq;
using Pancake.Core;
using Should;

namespace Pancake.Tests
{
    public class ResourceOrderingTests
    {
        public void serves_in_order_of_provider_registration_if_resources_are_registered_in_reverse()
        {
            var sut = new PancakeApi();
            var served = new List<Resource>();
            var firstProvider = new OrderedProvider<ResourceA>(served);
            var secondProvider = new OrderedProvider<ResourceB>(served);
            var resourceA = new ResourceA {Name = "A"};
            var resourceB = new ResourceB {Name = "B"};
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(firstProvider);
                cfg.RegisterProvider(secondProvider);
                cfg.Resource(resourceB);
                cfg.Resource(resourceA);
            });


            sut.Serve();

            served[0].ShouldEqual(resourceA);
            served[1].ShouldEqual(resourceB);
        }

        public void serves_in_order_of_provider_registration_if_resources_are_registered_in_order()
        {
            var sut = new PancakeApi();
            var servedResources = new List<Resource>();
            var firstProvider = new OrderedProvider<ResourceA>(servedResources);
            var secondProvider = new OrderedProvider<ResourceB>(servedResources);
            var resourceA = new ResourceA {Name = "A"};
            var resourceB = new ResourceB {Name = "B"};
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(firstProvider);
                cfg.RegisterProvider(secondProvider);
                cfg.Resource(resourceA);
                cfg.Resource(resourceB);
            });


            sut.Serve();

            servedResources[0].ShouldEqual(resourceA);
            servedResources[1].ShouldEqual(resourceB);
        }

        protected void serves_in_dependency_order()
        {
            var sut = new PancakeApi();
            var servedResources = new List<Resource>();
            var dependedResource1 = new DependedResource {Name = "First"};
            var dependedResource2 = new DependedResource {Name = "Second"};
            var dependingResource1 = new DependingResource
            {
                Name = "Second To Last",
                Dependency = dependedResource1.Name
            };
            var dependingResource2 = new DependingResource
            {
                Name = "Last",
                Dependency = dependedResource2.Name
            };
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(new OrderedProvider<DependingResource>(servedResources));
                cfg.RegisterProvider(new OrderedProvider<DependedResource>(servedResources));
            });

            sut.Resources(cfg =>
            {
                cfg.Resource(dependingResource1);
                cfg.Resource(dependingResource2);
                cfg.Resource(dependedResource1);
                cfg.Resource(dependedResource2);
            });

            sut.Serve();

            servedResources.Take(2).ShouldContain(dependedResource1);
            servedResources.Take(2).ShouldContain(dependedResource2);
            servedResources.Skip(2).ShouldContain(dependingResource1);
            servedResources.Skip(2).ShouldContain(dependingResource2);
        }
    }

    internal class DependingResource : TestResource
    {
        public Dependency<DependedResource> Dependency { get; set; }
    }

    internal class DependedResource : TestResource
    {
    }


    internal class ResourceA : Resource
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    internal class ResourceB : Resource
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }


    public class OrderedProvider<TResource> : ResourceProvider<TResource> where TResource : Resource
    {
        private readonly List<Resource> _served;

        public OrderedProvider(List<Resource> served)
        {
            _served = served;
        }

        public override TResource[] GetSystemResources(TResource[] resources)
        {
            return new TResource[] {};
        }

        public override void Create(TResource resource)
        {
            _served.Add(resource);
        }

        public override void Destroy(TResource resource)
        {
        }

        public override void Synchronize(TResource resource, TResource systemResource)
        {
        }
    }
}