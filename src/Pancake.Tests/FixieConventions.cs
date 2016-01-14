using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixie;

namespace Pancake.Tests
{
    class FixieConventions : Convention
    {
        public FixieConventions()
        {
            Classes.Where(x => !x.IsNested && x.Name.EndsWith("Tests"));
            Methods.Where(x => x.IsPublic && x.IsVoid() && !x.Name.Contains("WaitForPeriodicExport"));
            CaseExecution.Skip(x => !x.Method.IsPublic && x.Method.IsVoid());
        }
    }
}
