using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pancake.Core
{
    public class SynrchonizationEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T,IEnumerable<object>> _getSynchronizationComponents;

        public SynrchonizationEqualityComparer(Func<T, IEnumerable<object>> getSynchronizationComponents)
        {
            _getSynchronizationComponents = getSynchronizationComponents;
        }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(null, y)) return false;
            return _getSynchronizationComponents(x).SequenceEqual(_getSynchronizationComponents(y));
        }

        public int GetHashCode(T obj)
        {
             return HashCodeHelper.CombineHashCodes(_getSynchronizationComponents(obj));
        }
    }
}
