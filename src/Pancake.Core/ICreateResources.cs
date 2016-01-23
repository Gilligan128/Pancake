using System.Collections.Generic;

namespace Pancake.Core
{
    internal interface ICreateResources
    {
        void Execute(Resource[] missingResources, ResourceProvider provider);
    }
}