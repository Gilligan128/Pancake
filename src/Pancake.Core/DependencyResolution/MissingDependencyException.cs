using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core.DependencyResolution
{
    public class MissingDependencyException : Exception
    {
        public MissingDependencyException( string depended, string dependent) : base(string.Format("Resource {0} required by Resource {1} was not found in the catalog", dependent, depended))
        {
        }
    }
}
