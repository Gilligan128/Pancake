using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core
{
    public class Dependency<TResource> where TResource : Resource
    {
        public Dependency(string name)
        {
            Name = name;
        }
        public string Name { get; protected set; }
    }
}
