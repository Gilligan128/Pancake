using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core
{
    public interface ServingBehavior
    {
        void Serve(ResourceCatalog context, Action next);
    }
}
