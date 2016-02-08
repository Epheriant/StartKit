using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace AppTools.Util
{
    public class DynamicConverter: IObjectConverter<ExpandoObject>
    {
        public ExpandoObject ConvertTo(object arg)
        {
            if (arg == null)
                return null;

            ExpandoObject exp = new ExpandoObject();
            var map = exp as IDictionary<string, object>;
            foreach (MemberInfo mi in arg.GetType().GetMembers())
            {
                if (mi.MemberType == MemberTypes.Property)
                {
                    var Key = mi.Name;
                    var Obj = GetProperty(Key, arg);
                    map.Add(Key, Obj);
                }
                else if (mi.MemberType == MemberTypes.Field)
                {
                    var Key = mi.Name;
                    var Obj = GetField(Key, arg);
                    map.Add(Key, Obj);
                }
            }
            return exp;
        }

        public object ConvertFrom(ExpandoObject arg, Type ty)
        {
            //TODO: 1. Implement
            throw new NotImplementedException(" ");
        }


        private object ToType(object ret)
        {
            if (Helper.IsBuiltIn(ret) )

                return ret.ToString();

            if (ret is ICollection || ret is Array)
            {
                List<object> temp = new List<object>();
                foreach (var t in ((IEnumerable)ret))
                    temp.Add(ConvertTo(t));
                if (ret is Array)
                    return temp.ToArray();
                return temp;
            }

            if (ret is Array)
            {
                List<object> temp = new List<object>();
                foreach (var t in ((IEnumerable)ret))
                    temp.Add(ConvertTo(t));
                return ret;
            }

            return ConvertTo(ret);
        }

        private object GetProperty(string Key, object Value)
        {
            var ret = Value.GetType().GetProperty(Key).GetValue(Value, null);
            return ToType(ret);
        }

        private object GetField(string Key, object Value)
        {
            var ret = Value.GetType().GetField(Key).GetValue(Value);
            return ToType(ret);
        }




    }   
}