
namespace Pancake.Core
{

    /// <summary>
    /// Represents a resource context for assigning the "same" 
    /// resouce under multiple contexts.
    /// </summary>
    /// <remarks>
    /// This can be useful if you want to create the same entity under
    /// multiple tenants.
    /// </remarks>
    /// <typeparam name="TResource">The resource type of the context.</typeparam>
    public class ResourceContext<TResource>
        where TResource : Resource
    {
        /// <summary>
        /// Creates an instance of ResourceContext.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        public ResourceContext(string name)
        {
            Name = name;
            Resource = new Dependency<TResource>(name);
        }

        /// <summary>
        /// Gets/sets the name of the resource.
        /// </summary>
        public string Name { get; set; }

        public static implicit operator ResourceContext<TResource>(string input)
        {
            return new ResourceContext<TResource>(input);
        }

        /// <summary>
        /// Gets/sets the resource the context correlates to.
        /// </summary>
        public Dependency<TResource> Resource { get; set; }
    }
}
