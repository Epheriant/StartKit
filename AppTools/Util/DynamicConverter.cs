using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;


namespace AppTools.Util
{
    public class DynamicConverter
    {
        public const string QTE = "\"";

        public static string IdPad(string arg, int argPadding = 15)
        {
            int Pad = (-1) * argPadding - 1;
            string Format = "{0," + String.Format("{0}", Pad) + "}";
            return String.Format(Format, arg);
        }

        public static bool IsBuiltIn(object obj)
        {
            return  obj is System.Boolean ||
                    obj is System.Byte ||
                    obj is System.Char ||
                    obj is System.Decimal ||
                    obj is System.Double ||
                    obj is System.Int16 ||
                    obj is System.Int32 ||
                    obj is System.Int64 ||
                    obj is System.SByte ||
                    obj is System.Single ||
                    obj is System.String ||
                    obj is System.UInt16 ||
                    obj is System.UInt32 ||
                    obj is System.UInt64 ||
                    obj is System.Enum ||
                    obj is DateTime;
        }


        private static object ToType(object ret)
        {
            if (IsBuiltIn(ret) )

                return ret.ToString();

            if (ret is ICollection)
            {
                List<object> temp = new List<object>();
                foreach (var t in ((IEnumerable)ret))
                    temp.Add(ToDynamic(t));
                return ret;
            }
            return ToDynamic(ret);
        }

        private static object GetProperty(string Key, object Value)
        {
            Console.Out.WriteLine("Getting Property: " + Key);
            var ret = Value.GetType().GetProperty(Key).GetValue(Value, null);
            return ToType(ret);
        }

        private static object GetField(string Key, object Value)
        {
            var ret = Value.GetType().GetField(Key).GetValue(Value);
            return ToType(ret);
        }

        public static dynamic ToDynamic(object arg)
        {
            if (arg == null)
                return null;

            Console.Out.WriteLine("ToDynamic: " + arg.GetType().Name);
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
                    var Obj = GetProperty(Key, arg);
                    map.Add(Key, Obj);
                }
            }
            return exp;
        }

        public static string Escape(string ret)
        {
            ret.Replace("\r\n", " | ");
            ret.Replace("\n", " | ");
            ret.Replace("\\", "/");
            if(ret.Contains(" ")  )
               return String.Format("{0}{1}{2}",QTE,ret,QTE);
            return ret;
        }

        private static string SerializePrimitive(object ret)
        {
            if (ret == null)
                return "";
            if (ret is string)
                ret = Escape((string) ret);

            if (IsBuiltIn(ret))
                return ret.ToString();

            if (ret is IEnumerable)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append("[");
                foreach (var t in ((IEnumerable)ret))
                {
                    SerializeDynamic(t);
                }
                buf.Append("]");
                return buf.ToString();
            }
             return ret.ToString();
             
        }


        public static string SerializeDynamic(object entity)
        {
            StringBuilder buf = new StringBuilder();
            if (!(entity is ExpandoObject))
            {
                buf.Append(SerializePrimitive(entity));
            }
            else
            {
                var Map = entity as IDictionary<string, object>;
                int Max = Map.Keys.Max<string>( (x) => x.Length);
                buf.Append("{");
                foreach (var entry in Map)
                {
                    if (entry.Value == null)
                        continue;
                    buf.Append(IdPad(entry.Key,Max)).Append(": ");
                    var ret = entry.Value;
                    buf.Append(SerializeDynamic(ret)).Append(",");
                }
                buf.Append("}");
            }
            buf = buf.Replace(",}", "}");
            buf = buf.Replace(",]", "]");

            return buf.ToString();
        }
    }
}