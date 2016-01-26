using System.Collections.Generic;
using System.Linq;
using Pancake.Core;
using Should;

namespace Pancake.Tests
{
    public class ResourceOrderingTests
    {

        public void serves_in_dependency_order()
        {
            var sut = new PancakeApi();
            var servedResources = new List<Resource>();
            var dependedResource1 = new DependedResource { Name = "First" };
            var dependedResource2 = new DependedResource { Name = "Second" };
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

        public void serves_in_dependency_order_three_levels_deep()
        {
            var sut = new PancakeApi();
            var servedResources = new List<Resource>();
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(new OrderedProvider<DependingResource>(servedResources));
                cfg.RegisterProvider(new OrderedProvider<DependedResource>(servedResources));
                cfg.RegisterProvider(new OrderedProvider<FinalDependendResource>(servedResources));
            });
            sut.Resources(r => r.Resources(new DependingResource
            {
                Name = "D",
                Dependency = "C"
            }, new DependingResource
            {
                Name = "C",
                Dependency = "B"
            }, new DependedResource
            {
                Name = "B",
                Dependency = "A"
            }, new FinalDependendResource()
            {
                Name = "A"
            }));

        }
    }

    internal class DependingResource : TestResource
    {
        public Dependency<DependedResource> Dependency { get; set; }
    }


    internal class ResourceA : Resource
    {
    }

    internal class ResourceB : Resource
    {
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

        public override bool ShouldSynchronize(TResource resource, TResource systemResource)
        {
            return true;
        }
    }
}