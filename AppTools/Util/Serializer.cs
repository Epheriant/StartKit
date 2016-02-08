using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace AppTools.Util
{

    public class JSerializer
    {
        private static JavaScriptSerializer Serializer = new JavaScriptSerializer();
       

        private JSerializer() {
            
        }


        public static  string Serialize(object entity)
        {
            return Serializer.Serialize(entity);
        }

        
        public static object StrToDynamic(string arg)
        {
            return Serializer.Deserialize(arg, typeof(object));
        }

        public static object ObjToDynamic(object entity)
        {
            var input = Serialize(entity);
            return Serializer.Deserialize(input, typeof(object));
        }

        public static string Ident = @"[a-zA-Z_$][0-9a-zA-Z_$]*[\s]*:";
        public static string RegularString = @"[\s]*:[\s]*[^\s\[\]\{\}:\']*";
        public static string NoSpaceString = @"[a-zA-Z_$][0-9a-zA-Z_/]*";


        public static string FormalJSON(string input)
        {
            if (input.Trim()[0] != '{')
                input = "{" + input.Trim() + "}";

            var result = Regex.Replace(input, Ident, m => String.Format("\'{0}\'", m.Value));


            result = Regex.Replace(result, ":\'", "\':");

            result = Regex.Replace(result, RegularString, m => String.Format("\'{0}\'", m.Value));
            result = Regex.Replace(result, @"\'\'[\s]*:[\s]*", "\':\'");

            result = Regex.Replace(result, "\'", "\"");
            result = result.Replace("\r\n", "\n");
            var lines = result.Split('\n');
            result = "";
            foreach (var line in lines)
            {
                var cspec = new char[] {'[',']','{','}',':',',','\"', '\''};

                bool found = false;
                foreach (char c in cspec)
                    if (line.Contains(c))
                        found = true;
            
                var l = line.Trim();
                double d;
                if (!found && !Double.TryParse(l, out d))
                    l = "\"" + l + "\"";
                result += l.Trim() + "|";

            }

            while (Regex.Matches(result, "\\s\"").Count > 0)
                result = Regex.Replace(result, "\\s\"", "\"");
            result = result.Replace(":|", ":");
            result = result.Replace("|]", "]");
            result = result.Replace("}|", "}");
            result = result.Replace("|}", "}");
            result = result.Replace("{|", "{");
            result = result.Replace("|{", "{");
            result = result.Replace("[|", "[");
            result = result.Replace("|[", "[");
            result = result.Replace("|", ",");
            result = Regex.Replace(result, ":\\s", ":");
            result = result.Replace(":\"\"[", ":[");
            result = result.Replace(":\"\"{", ":{");
            return result;
        }

        private static string Spaces(int howMany)
        {
            string ret = "";
            for (int i = 0; i < howMany; i++)
                ret += "  ";
            return ret;
        }

        public static string PretyfyJSON(string arg)
        {
            string ret = arg.Replace("{", "{\n");
            ret = ret.Replace("{", "\n{\n");
            ret = ret.Replace("}", "\n}\n");
            ret = ret.Replace("[", "\n[\n");
            ret = ret.Replace("]", "\n]\n");
            ret = ret.Replace(",", ",\n");
            ret = ret.Replace("]\n,", "],");
            ret = ret.Replace("}\n,", "},");
            ret = ret.Replace("\n\n", "\n");
            ret = ret.Replace("\n\n", "\n");
            string[] lines = ret.Split('\n');
            int open = 0;
            string ret2 = "";
            foreach (string l in lines)
            {
                var cur = "";
                if (l.Length > 0 && (l[0] == '}' || l[0] == ']'))
                    open--;
                if (l.Trim().EndsWith(":"))
                    cur = "\n";
                ret2 += cur + Spaces(open) + l + "\n";
                if (l.Equals("{") || l.Equals("["))
                    open++;

            }
            return ret2.Replace(",","");
        }

    }
}
