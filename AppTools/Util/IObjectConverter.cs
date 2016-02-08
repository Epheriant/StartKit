using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.Util
{
    public interface IObjectConverter<T> 
    {
        T ConvertTo(object arg);
        object ConvertFrom(T arg, Type tp);
    }
}
