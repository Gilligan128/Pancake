using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core
{
    public class DefaultCreateResources : ICreateResources
    {
        public void Execute(Resource[] missingResources, ResourceProvider provider)
        {
            foreach (var missingResource in missingResources.Where(r => r.Ensure == Ensure.Present))
            {
                provider.Create(missingResource);
            }
        }
    }
}