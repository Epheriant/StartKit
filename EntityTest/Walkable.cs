using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTypes
{
    public abstract class Walkable: Equatable
    {
        public abstract Walkable Next();
        public abstract Walkable Previous();
    }
}
