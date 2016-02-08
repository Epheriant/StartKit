using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTypes
{
    public abstract class Orderable: Equatable
    {
        public abstract int CompareTo(Orderable other);

        public static bool operator >(Orderable first, Orderable other)
        {
            return (first.CompareTo(other) > 0);
        }

        public static bool operator <(Orderable first, Orderable other)
        {
            return (first.CompareTo(other) < 0);
        }

        public static bool operator >=(Orderable first, Orderable other)
        {
            return (first.CompareTo(other) >= 0);
        }

        public static bool operator <=(Orderable first, Orderable other)
        {
            return (first.CompareTo(other) <= 0);
        }
    }
}
