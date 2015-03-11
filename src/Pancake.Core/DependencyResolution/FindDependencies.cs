using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core.DependencyResolution
{
    public class FindDependencyProperties
    {
        public ResourceDependencyReference[] Execute(Resource resource) {

            var dependencyProperties = resource.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType)
                .Where(p => typeof(Dependency<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()));
            var dependencyKeyProp = dependencyProperties.Select(p => new
            {
                DependencyProperty = p.GetValue(resource),
                ResourceType = p.PropertyType.GetGenericArguments()[0]
            });
            var dependencyKeys = dependencyKeyProp
                .Where(x => x.DependencyProperty != null)
                .Select(p => new ResourceDependencyReference{ Name = (string)p.DependencyProperty.GetType().GetProperty("Name").GetValue(p.DependencyProperty), ResourceType = p.ResourceType })
                .Where(k => !string.IsNullOrEmpty(k.Name));
            return dependencyKeys.ToArray();
        }

    }

    public class ResourceDependencyReference
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        
    }

}
