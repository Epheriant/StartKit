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
        private static  object ToType(object ret)
        {
            if (ret is Enum)
                return ret.ToString();
            if (ret is Int32 ||
                ret is double ||
                ret is string ||
                ret is DateTime ||
                ret is bool)
                return ret;

            if (ret is IEnumerable)
            {
                List<object> temp = new List<object>();
                foreach (var t in ((IEnumerable)ret))
                    temp.Add(Convert(t));
                return ret;
            }
            return Convert(ret);
        }

        private static  object GetProperty(string Key, object Value)
        {
           var ret = Value.GetType().GetProperty(Key).GetValue(Value, null);
           return ToType(ret);
        }

        private static object GetField(string Key, object Value)
        {
            var ret = Value.GetType().GetField(Key).GetValue(Value);
            return ToType(ret);
        }

        public static  dynamic Convert(object arg)
        {
            if (arg == null)
                return null;
            ExpandoObject exp = new ExpandoObject();
            var map = exp as IDictionary<string, object>;
            foreach (MemberInfo mi in arg.GetType().GetMembers())
            {
                if (mi.MemberType == MemberTypes.Property )
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

        private static string SerializePrimitive(object ret)
        {
            StringBuilder buf = new StringBuilder();
            if (ret is Enum ||
                    ret is Int32 ||
                    ret is double ||
                    ret is string ||
                    ret is DateTime ||
                    ret is bool)
                buf.Append("\"").Append(ret.ToString()).Append("\"");

            else if (ret is IEnumerable)
            {
                buf.Append("[");
                foreach (var t in ((IEnumerable)ret))
                {
                    Serialize(t);
                }
                buf.Append("]");
            }
            return buf.ToString();
        }

        public static string Serialize(object entity)
        {
            StringBuilder buf = new StringBuilder();
            if (!(entity is ExpandoObject))
            {
                buf.Append(SerializePrimitive(entity));
            }
            else {
                var Map = entity as IDictionary<string, object>;
                buf.Append("{");
                foreach (var entry in Map)
                {
                    buf.Append("\"").Append(entry.Key).Append("\"").Append(" : ");
                    var ret = entry.Value;
                    buf.Append(Serialize(ret)).Append(",");
                }
                buf.Append("}");
            }
            string retVal = buf.ToString();
            retVal = retVal.Replace(",}", "}");
            retVal = retVal.Replace(",]", "]");
            return retVal;
        }
    }


    public class DynamicJsonObject : DynamicObject
    {
        private readonly IDictionary<string, object> _dictionary;

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            _dictionary = dictionary;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("{");
            ToString(sb);
            return sb.ToString();
        }

        private void ToString(StringBuilder sb)
        {
            var firstInDictionary = true;
            foreach (var pair in _dictionary)
            {
                if (!firstInDictionary)
                    sb.Append(",");
                firstInDictionary = false;
                var value = pair.Value;
                var name = pair.Key;
                if (value is string)
                {
                    sb.AppendFormat("{0}:\"{1}\"", name, value);
                }
                else if (value is IDictionary<string, object>)
                {
                    new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                }
                else if (value is ArrayList)
                {
                    sb.Append(name + ":[");
                    var firstInArray = true;
                    foreach (var arrayValue in (ArrayList)value)
                    {
                        if (!firstInArray)
                            sb.Append(",");
                        firstInArray = false;
                        if (arrayValue is IDictionary<string, object>)
                            new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb);
                        else if (arrayValue is string)
                            sb.AppendFormat("\"{0}\"", arrayValue);
                        else
                            sb.AppendFormat("{0}", arrayValue);

                    }
                    sb.Append("]");
                }
                else
                {
                    sb.AppendFormat("{0}:{1}", name, value);
                }
            }
            sb.Append("}");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_dictionary.TryGetValue(binder.Name, out result))
            {
                // return null to avoid exception.  caller can check for null this way...
                result = null;
                return true;
            }

            result = WrapResultObject(result);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] != null)
            {
                if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = WrapResultObject(result);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        private static object WrapResultObject(object result)
        {
            var dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
                return new DynamicJsonObject(dictionary);

            var arrayList = result as ArrayList;
            if (arrayList != null && arrayList.Count > 0)
            {
                return arrayList[0] is IDictionary<string, object>
                    ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                    : new List<object>(arrayList.Cast<object>());
            }

            return result;
        }
    }


    public sealed class DynamicJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) })); }
        }

        #region Nested type: DynamicJsonObject

        

        #endregion
    }
}
