using System.Collections.Generic;
using System.Linq;

namespace Pancake.Core
{
    public abstract class Resource
    {
        protected Resource()
        {
            Ensure = Ensure.Present;
        }

        public Ensure Ensure { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            var vo = obj as Resource;
            return GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetEqualityComponents());
        }

        public virtual IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }


        public abstract IEnumerable<object> GetSynchronizationComponents();

        public override string ToString()
        {
            return string.Format("{0}['{1}']", GetType().Name, Name);
        }
    }
}

