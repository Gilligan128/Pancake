using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pancake.Core;
using Pancake.Core.DependencyResolution;
using Should;

namespace Pancake.Tests
{
    public class FindDependencyPropertiesTests
    {

        public void should_find_dependency_property_array()
        {

            var resource = new DependingArrayResource
            {
                Name = "A",
                Dependencies = new[] {new Dependency<DependedResource>("B"), new Dependency<DependedResource>("C")}
            };
            var sut = new FindDependencyProperties(); 
            
            var references = sut.Execute(resource);

            references.Count().ShouldEqual(2);
        }

    }
}
