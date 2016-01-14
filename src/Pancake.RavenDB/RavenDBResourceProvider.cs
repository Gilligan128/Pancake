using Pancake.Core;
using Raven.Client;

namespace Pancake.RavenDB
{
    /// <summary>
    /// Represents a base class for RavenDB document store resource providers.
    /// </summary>
    /// <typeparam name="TResource">The resource type of the provider.</typeparam>
    public abstract class RavenDBResourceProvider<TResource> : ResourceProvider<TResource>, RavenDBResourceProvider where TResource : Resource
    {
        public IDocumentSession Session { get; set; }
    }

    /// <summary>
    /// Represents an interface for RavenDB document store resource providers.
    /// </summary>
    public interface RavenDBResourceProvider : ResourceProvider
    {
        IDocumentSession Session { get; set; }
    }
}
