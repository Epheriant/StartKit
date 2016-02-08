using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTypes
{
    public abstract class Equatable: Value
    {
        public override abstract int  GetHashCode();
        public override abstract bool Equals(Object obj);
        public abstract ValState Status { get; }

        public static bool operator ==(Equatable first, Equatable other)
        {
            return first.Equals(other);
        }
        public static bool operator !=(Equatable first, Equatable other)
        {
            return !first.Equals(other);
        }
    }
}
