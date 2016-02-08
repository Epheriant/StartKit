using AppTools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AppTools
{
    public interface Configurable  {
        void Configure(dynamic memento);
    }

}
