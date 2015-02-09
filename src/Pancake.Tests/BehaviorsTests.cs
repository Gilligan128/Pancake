using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pancake.Core;
using Should;

namespace Pancake.Tests
{
    public class BehaviorsTests 
    {
        public void runs_behaviors_in_order_of_registration()
        {
            var sut = new PancakeApi();
            var ranBehaviors = new HashSet<ServingBehavior>();
            var firstBehaior = new TestBehavior(ranBehaviors);
            var secondBehavior = new TestBehavior(ranBehaviors);
            sut.Configure(cfg =>
            {
                cfg.RegisterBehavior(firstBehaior);
                cfg.RegisterBehavior(secondBehavior);
            });

            sut.Serve();


            ranBehaviors.Count.ShouldEqual(2);
            ranBehaviors.First().ShouldEqual(firstBehaior);
            ranBehaviors.Skip(1).First().ShouldEqual(secondBehavior);
        }


        public class TestBehavior : ServingBehavior
        {
            private readonly HashSet<ServingBehavior> _ranBehaviors;

            public TestBehavior(HashSet<ServingBehavior> ranBehaviors)
            {
                _ranBehaviors = ranBehaviors;
            }


            public void Serve(ResourceCatalog context, Action next)
            {
                _ranBehaviors.Add(this);
                next();
            }
        }

    }
}
