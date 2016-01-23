using System.Collections.Generic;

namespace Pancake.Core
{
    public interface ICreateResources
    {
        void Execute(Resource[] missingResources, ResourceProvider provider);
    }
}