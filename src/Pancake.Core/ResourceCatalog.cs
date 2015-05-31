using System;
using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core
{
    public class ResourceCatalog : ResourceConfig
    {
        private readonly HashSet<Resource> _resources = new HashSet<Resource>();
        public Resource[] Resources => _resources.ToArray();
        public ServingBehavior[] ServingBehaviors => _behaviors.ToArray();
        public ResourceProvider[] Providers => _providers.Values.ToArray();

        private readonly Dictionary<string, object> _contexts = new Dictionary<string, object>();
        public Dictionary<Type, ResourceProvider> _providers  =  new Dictionary<Type, ResourceProvider>();
        private readonly HashSet<ServingBehavior> _behaviors = new HashSet<ServingBehavior>();

        /// <summary>
        /// Registers a resource context.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="names">The names of the resource.</param>
        public void Contexts<TResource>(params string[] names) where TResource : Resource
        {
            var resourceType = typeof(TResource);

            var genericType = typeof(ResourceContext<>);

            var contextType = genericType.MakeGenericType(resourceType);

            foreach (var name in names)
            {
                var key = string.Format("{0}-{1}", resourceType.FullName, name);

                var context = (ResourceContext<TResource>)Activator.CreateInstance(contextType, new object[] { name });

                _contexts.Add(key, context);
            }
        }

        /// <summary>
        /// Registers a resource for each of the applied contexts.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="resource">The resource to be contextualized.</param>
        /// <param name="action">The resource context registration.</param>
        public void ContextualResource<TResource>(TResource resource, Action<ResourceRegistration<TResource>> action)
            where TResource : Resource, ICloneable
        {
            var genericType = typeof(TResource);

            var contextualType = typeof(ResourceContext<>).MakeGenericType(genericType);

            var contexts = _contexts.Values.ToArray();

            var registrationType = typeof(ResourceRegistration<>).MakeGenericType(genericType);

            var registration = (ResourceRegistration<TResource>)Activator.CreateInstance(registrationType, new object[] { contexts, resource });

            action.Invoke(registration);

            foreach (var r in registration.Resources)
            {
                Resource(r);
            }
        }

        public ResourceProvider ProviderFor(Type key)
        {
            return _providers[key];
        }

        public void Resource(Resource resource)
        {
            _resources.Add(resource);
        }

        public void RegisterProvider<TResource>(ResourceProvider<TResource> resourceProvider) where TResource : Resource
        {
            _providers.Add(typeof(TResource), resourceProvider);
        }

        public void RegisterBehavior(ServingBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public ResourceProvider<TResource> ProviderFor<TResource>() where TResource : Resource
        {
            return (ResourceProvider<TResource>) _providers[typeof (TResource)];
        }
    }
}