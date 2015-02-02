using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core
{
    public class Resource 
    {
        public Resource()
        {
            Ensure = Ensure.Present;
        }

        public Ensure Ensure { get; set; }
    }
}
