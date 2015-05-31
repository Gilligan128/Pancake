using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pancake.Core
{
    /// <summary>
    /// Represents resource registration.
    /// </summary>
    /// <typeparam name="T">The type of the resource.</typeparam>
    public class ResourceRegistration<T>
        where T : Resource, ICloneable
    {
        /// <summary>
        /// Creates an instance of ResourceRegistration&lt;T&gt;.
        /// </summary>
        /// <param name="resourceContexts">All of the available resource contexts.</param>
        /// <param name="resource">The resource to contextualize.</param>
        public ResourceRegistration(object[] resourceContexts, T resource)
        {
            AppliesTo = new AppliesTo<T>(this, resourceContexts, resource);
        }
        
        /// <summary>
        /// Gets the applies to definition.
        /// </summary>
        public AppliesTo<T> AppliesTo { get; private set; }

        /// <summary>
        /// Gets the resources registered.
        /// </summary>
        public Resource[] Resources {  get { return AppliesTo.Resources; } }
    }

    /// <summary>
    /// Represents the applies to definition.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public class AppliesTo<TResource>
        where TResource : Resource, ICloneable
    {
        private readonly TResource _resource;
        private object[] _resourceContexts;
        private readonly ResourceRegistration<TResource> _resourceRegistration;
        private readonly List<Resource> _resources = new List<Resource>();

        /// <summary>
        /// Creates an instance of AppliesTo&lt;TResource&gt;.
        /// </summary>
        /// <param name="resourceRegistration">The parent resource registration.</param>
        /// <param name="resourceContexts">The available resource contexts.</param>
        /// <param name="resource">The source resource.</param>
        public AppliesTo(ResourceRegistration<TResource> resourceRegistration, object[] resourceContexts, 
            TResource resource)
        {
            _resource = resource;
            _resourceContexts = resourceContexts;
            _resourceRegistration = resourceRegistration;
        }

        /// <summary>
        /// Contextualizes the resource.
        /// </summary>
        /// <typeparam name="TContext">The type of the context resource.</typeparam>
        /// <param name="propertyExpression">The property of the context resource.</param>
        /// <param name="contextualAction">The contextual setup.</param>
        /// <returns>The parent resource registration.</returns>
        public ResourceRegistration<TResource> Context<TContext>(
            Expression<Func<TResource, ResourceContext<TContext>>> propertyExpression
            , Action<ContextualRegistration> contextualAction)
            where TContext : Resource
        {
            var pi = propertyExpression.FindPropertyInfo();

            var applicableContexts = _resourceContexts.OfType<ResourceContext<TContext>>()
                .ToArray();

            var registration = new ContextualRegistration(applicableContexts
                .Select(x => x.Name)
                .ToArray());

            contextualAction.Invoke(registration);

            foreach (var context in applicableContexts.Where(r => registration.SpecifiedContextNames.Contains(r.Name)))
            {
                var resource = (TResource)_resource.Clone();

                resource.Name = string.Format("{0}-{1}", context.Name, resource.Name);

                pi.SetValue(resource, context);

                _resources.Add(resource);
            }
            
            return _resourceRegistration;
        }

        /// <summary>
        /// Gets the generated resources.
        /// </summary>
        public Resource[] Resources {  get { return _resources.ToArray(); } }
    }

    /// <summary>
    /// Represents contextual registration.
    /// </summary>
    public class ContextualRegistration : ContextRemover
    {
        protected List<string> AppliedContextNames = new List<string>();
        protected string[] ContextNames;

        /// <summary>
        /// Creates an instance of ContextualRegistration.
        /// </summary>
        /// <param name="contextNames">The available context names.</param>
        public ContextualRegistration(string[] contextNames)
        {
            ContextNames = contextNames;
        }

        /// <summary>
        /// Gets/sets the specified context names.
        /// </summary>
        public string[] SpecifiedContextNames {  get { return AppliedContextNames.ToArray(); } }

        /// <summary>
        /// Adds all applicable context names.
        /// </summary>
        /// <returns>An instance of <see cref="ContextRemover" />.</returns>
        public ContextRemover All()
        {
            AppliedContextNames.Clear();

            AppliedContextNames.AddRange(ContextNames);

            return this;
        }

        /// <summary>
        /// Adds the specified names.
        /// </summary>
        /// <returns>An instance of <see cref="ContextSpecifier" />.</returns>
        public ContextSpecifier Some(params string[] names)
        {
            var validNames = ContextNames
                .Intersect(names);

            AppliedContextNames.AddRange(validNames);

            return this;
        }

        /// <summary>
        /// Excludes a name (used in conjunction with .All().
        /// </summary>
        /// <returns>An instance of <see cref="ContextRemover" />.</returns>
        public ContextRemover Exclude(string name)
        {
            if (AppliedContextNames.Contains(name))
                throw new ArgumentException(string.Format("{0} not found in list of resource contexts", "name"));

            AppliedContextNames.Remove(name);

            return this;
        }
    }

    /// <summary>
    /// Represents context name specification.
    /// </summary>
    public interface ContextSpecifier
    {
        /// <summary>
        /// Adds all applicable context names.
        /// </summary>
        /// <returns>An instance of <see cref="ContextRemover" />.</returns>
        ContextRemover All();

        /// <summary>
        /// Adds the specified names.
        /// </summary>
        /// <returns>An instance of <see cref="ContextSpecifier" />.</returns>
        ContextSpecifier Some(params string[] names);
    }

    /// <summary>
    /// Represents context name removal specification.
    /// </summary>
    public interface ContextRemover : ContextSpecifier
    {
        /// <summary>
        /// Excludes a name (used in conjunction with .All().
        /// </summary>
        /// <returns>An instance of <see cref="ContextRemover" />.</returns>
        ContextRemover Exclude(string name);
    }
}
