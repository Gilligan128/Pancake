using Pancake.Core;
using Pancake.RavenDB;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Tests.Helpers;
using Should;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Pancake.Tests.RavenDB
{
    public class RavenDBResourceProviderTests : RavenTestBase
    {
        public void creates_missing_resource()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    var api = new PancakeApi();

                    api.Configure(cfg =>
                    {
                        cfg.UseRavenDB(session);
                        cfg.Resource(new TestResource { Name = "bar" });
                        cfg.RegisterProvider(new TestResourceProvider());
                        cfg.Resource(new TestResource { Name = "foo" });
                    });

                    api.Serve();
                }

                using (var session = store.OpenSession())
                {
                    var tests = session.Query<Test>().ToArray();
                    tests.Length.ShouldEqual(2);
                    tests[0].Name.ShouldEqual("bar");
                    tests[1].Name.ShouldEqual("foo");
                }
            }
        }

        protected override void ModifyStore(EmbeddableDocumentStore documentStore)
        {
            base.ModifyStore(documentStore);

            documentStore.Conventions.DefaultQueryingConsistency =
                ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
        }

        public class Test
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }

        public class TestResource : Resource
        {
            public override IEnumerable<object> GetSynchronizationComponents()
            {
                yield return Name;
            }
        }

        public class TestResourceProvider : RavenDBResourceProvider<TestResource>
        {
            public override void Create(TestResource missingResource)
            {
                var test = new Test { Name = missingResource.Name };

                Session.Store(test);
            }

            public override void Destroy(TestResource resource)
            {
                var test = Session.Query<Test>().First(t => t.Name == resource.Name);

                Session.Delete(test);
            }

            public override void Flush()
            {
                Session.SaveChanges();
            }

            public override TestResource[] GetSystemResources(TestResource[] resourceSet)
            {
                return new TestResource[0];
            }


            public override bool ShouldSynchronize(TestResource desiredResource, TestResource systemResource)
            {
                return false;
            }


            public override void Synchronize(TestResource desiredResource, TestResource systemResource)
            {
                throw new NotImplementedException();
            }
        }
    }
}
