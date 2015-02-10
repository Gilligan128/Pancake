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
            var servedProviders = new List<ResourceProvider>();
            var firstProvider = new OrderedProvider<ResourceA>(servedProviders);
            var secondProvider = new OrderedProvider<ResourceB>(servedProviders);
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(firstProvider);
                cfg.RegisterProvider(secondProvider);
                cfg.Resource(new ResourceB() { Name = "B"});
                cfg.Resource(new ResourceA() { Name = "A"});
            });


            sut.Serve();

            servedProviders[0].ShouldEqual(firstProvider);
            servedProviders[1].ShouldEqual(secondProvider);
        }

        public void serves_in_order_of_provider_registration_if_resources_are_registered_in_order()
        {
            var sut = new PancakeApi();
            var servedProviders = new List<ResourceProvider>();
            var firstProvider = new OrderedProvider<ResourceA>(servedProviders);
            var secondProvider = new OrderedProvider<ResourceB>(servedProviders);
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(firstProvider);
                cfg.RegisterProvider(secondProvider);
                cfg.Resource(new ResourceA() { Name = "A" });
                cfg.Resource(new ResourceB() { Name = "B" });
            });


            sut.Serve();

            servedProviders[0].ShouldEqual(firstProvider);
            servedProviders[1].ShouldEqual(secondProvider);
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
        private readonly List<ResourceProvider> _servedProviders;

        public OrderedProvider(List<ResourceProvider> servedProviders)
        {
            _servedProviders = servedProviders;
        }

        public override TResource[] GetSystemResources(TResource[] resources)
        {
            _servedProviders.Add(this);
            return new TResource[] { };
        }

        public override void Create(TResource resouce)
        {
        }

        public override void Destroy(TResource resource)
        {
        }

        public override void Synchronize(TResource resource, TResource systemResource)
        {
        }
    }
}
