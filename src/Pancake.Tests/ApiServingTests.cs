using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pancake.Core;
using Should;

namespace Pancake.Tests
{
    public class ApiServingTests
    {
        public void should_destroy_resources_that_exist_but_ensured_absent()
        {
            var sut = new PancakeApi();
            var testResource = new TestResource
            {
                Name = "test",
                Ensure = Ensure.Absent
            };
            var resourceProvider = new TestResourceProvider<TestResource>();
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(resourceProvider);
                cfg.Resource(testResource);
            });
            resourceProvider.AddSystemResource(testResource);

            sut.Serve();

            resourceProvider.DestroyedResources.ShouldContain(testResource);
        }

        public void should_create_resources_that_do_not_exist_but_ensured_present()
        {
            var sut = new PancakeApi();
            var resourceProvider = new TestResourceProvider<TestResource>();
            var testResource = new TestResource
            {
                Name = "test",
                Ensure = Ensure.Present
            };
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(resourceProvider);
                cfg.Resource(testResource);
            });

            sut.Serve();

            resourceProvider.CreatedResources.ShouldContain(testResource);
        }

        public void should_not_destroy_resource_that_does_not_exist_and_ensured_absent()
        {
            var sut = new PancakeApi();
            var resourceProvider = new TestResourceProvider<TestResource>();
            var testResource = new TestResource
            {
                Name = "test",
                Ensure = Ensure.Absent
            };
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(resourceProvider);
                cfg.Resource(testResource);
            });

            sut.Serve();

            resourceProvider.DestroyedResources.ShouldNotContain(testResource);
        }

        public void should_not_create_resource_that_exists_and_ensured_present()
        {
            var sut = new PancakeApi();
            var testResource = new TestResource
            {
                Name = "different",
                Ensure = Ensure.Present
            };
            var resourceProvider = new TestResourceProvider<TestResource>();
            resourceProvider.AddSystemResource(testResource);
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(resourceProvider);
                cfg.Resource(testResource);
            });

            sut.Serve();

            resourceProvider.CreatedResources.ShouldNotContain(testResource);
        }

        public void should_synchronize_resource_that_is_different()
        {
            var sut = new PancakeApi();
            var sysResource = new DescriptiveResource
            {
                Name = "test",
                Description = "original",
                Ensure = Ensure.Present
            };
            var resourceProvider = new TestResourceProvider<DescriptiveResource>();
            resourceProvider.AddSystemResource(sysResource);
            var testResource = new DescriptiveResource
            {
                Name = "test",
                Description = "different"
            };
            sut.Configure(cfg =>
            {
                cfg.RegisterProvider(resourceProvider);
                cfg.Resource(testResource);
            });

            sut.Serve();

            resourceProvider.SynchronizedResources.ShouldContain(testResource);
        }

        public void should_not_synchronize_when_resource_exists_but_is_ensured_absent()
        {
            var sut = new PancakeApi();
            var resourceProvider = new TestResourceProvider<DescriptiveResource>();
            var systemResource = new DescriptiveResource()
            {
                Name = "test",
                Description = "test"
            };
            resourceProvider.AddSystemResource(systemResource);
            var testResource = new DescriptiveResource
            {
                Name = "test",
                Ensure = Ensure.Absent,
                Description = "different"
            };
            sut.Configure(cfg => 
            {
                cfg.RegisterProvider(resourceProvider);
                cfg.Resource(testResource);
            });

            sut.Serve();

            resourceProvider.SynchronizedResources.ShouldNotContain(testResource);
        }

    }

    public class DescriptiveResource : Resource
    {
        public string Description { get; set; }
    }
}
