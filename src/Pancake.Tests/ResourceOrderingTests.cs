using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pancake.Core;
using Should;
using Should.Core.Assertions;

namespace Pancake.Tests
{
    public class ResourceOrderingTests
    {
        public void serves_in_order_of_provider_registration_if_resources_are_registered_in_reverse()
        {
            var sut = new PancakeApi();
            List<Resource> served = new List<Resource>();
            var firstProvider = new OrderedProvider<ResourceA>(served);
            var secondProvider = new OrderedProvider<ResourceB>(served);
            var resourceA = new ResourceA() { Name = "A"};
            var resourceB = new ResourceB() { Name = "B"};
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
            var resourceA = new ResourceA() { Name = "A" };
            var resourceB = new ResourceB() { Name = "B" };
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
    }

    class ResourceA : Resource
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }

    class ResourceB : Resource
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
            return new TResource[] { };
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
