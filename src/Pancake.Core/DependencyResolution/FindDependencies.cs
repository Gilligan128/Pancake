using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core.DependencyResolution
{
    public class FindDependencyProperties
    {
        private static Type DependencyType = typeof(Dependency<>);
        private static Type ResourceContextType = typeof(ResourceContext<>);

        public ResourceDependencyReference[] Execute(Resource resource)
        {
            var dependencyProperties = resource.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType)
                .Where(p =>
                {
                    var genericType = p.PropertyType.GetGenericTypeDefinition();
                    return DependencyType.IsAssignableFrom(genericType)
                    || ResourceContextType.IsAssignableFrom(genericType);
                });
            var dependencyKeyProp = dependencyProperties.Select(p => new
            {
                DependencyProperty = p.GetValue(resource),
                ResourceType = p.PropertyType.GetGenericArguments()[0]
            });
            var dependencyKeys = dependencyKeyProp
                .Where(x => x.DependencyProperty != null)
                .Select(
                    p =>
                        new ResourceDependencyReference
                        {
                            Name = GetDependencyName(p.DependencyProperty),
                            ResourceType = p.ResourceType
                        })
                .Where(k => !string.IsNullOrEmpty(k.Name));

            var dependencyEnumerables = resource.GetType().GetProperties()
                .Where(p => p.PropertyType.GetInterfaces().Any(t => t.IsGenericType
                                                                    &&
                                                                    t.GetGenericTypeDefinition() ==
                                                                    typeof (IEnumerable<>) &&
                                                                    t.GetGenericArguments()[0].IsGenericType &&
                                                                    typeof (Dependency<>).IsAssignableFrom(
                                                                        t.GetGenericArguments()[0]
                                                                            .GetGenericTypeDefinition())));
            var dependencyEnumerableProp =
                dependencyEnumerables.Select(
                    p =>
                        new
                        {
                            DependencyProperty = p.GetValue(resource),
                            ResourceType = p.PropertyType.GetGenericArguments()[0]
                        });
            var dependencyEnumerableKeys = dependencyEnumerableProp
                .Where(x => x.DependencyProperty != null)
                .SelectMany(
                    p =>
                        ((IEnumerable) p.DependencyProperty).OfType<object>()
                            .Select(
                                d =>
                                    new ResourceDependencyReference
                                    {
                                        Name = GetDependencyName(d),
                                        ResourceType = p.ResourceType
                                    }));

            return dependencyKeys.Union(dependencyEnumerableKeys).ToArray();
        }

        private static string GetDependencyName(object p)
        {
            return (string) p.GetType().GetProperty("Name").GetValue(p);
        }
    }

    public class ResourceDependencyReference
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
    }
}