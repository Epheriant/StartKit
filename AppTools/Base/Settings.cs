using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using AppTools.Util;
using System.Web.Script.Serialization;

namespace AppTools
{
    public static class Extensions
    {
        public static object ToEnum(this Enum en, String arg)
        {           
           return Enum.Parse(en.GetType(), arg, true);
        }

        
    }
    


    public static class Settings
    {
        #region Members...
        public static bool Loaded { get; private set; }
        private static char[] PropSeparators = { '-', '(', ')' };
        public static readonly char Delim = System.IO.Path.DirectorySeparatorChar;
        public static IDictionary<string, string> AssemblyDictionary { get; private set; }
        public static HashSet<string> Processed = new HashSet<string>();
      //  public static IDictionary<string, Configurable> DefaultInit = new Dictionary<string, Configurable>();
        private static readonly string EXT = ".json";
        #endregion


        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string GetAssemblyName(object obj)
        {
            if (obj == null)
                return "";
            string ret = obj.GetType().Assembly.GetName().Name;
            return ret;
        }

        public static string CheckOrMakeDir(string arg) {
            if(!Directory.Exists(arg) )
                Directory.CreateDirectory(arg);
            return arg;     
        }


        public static void ProcessConfig()
        {
            foreach (string fileName in Directory.GetFiles(Config.Default.ConfigDir))  {
                if (fileName.ToLower().EndsWith(EXT))
                    ProcessConfigFile(fileName);
            }

        }

        public static void Load()
        {
            Load(new string[0]);
        }

        public static void Load(string[] args)
        {
            if (Loaded)
                return;
            LoadAssemblies();
            ProcessArgs(args);
            ProcessConfig();
            Loaded = true;
            ProcessArgs(args);
        }

        private static void ProcessArgs(string[] args)
        {
         /*   char[] delim = new char[] {'-','.'};
            for (int i = 0; i < args.Length - 1; )
            {
                if (args[i][0] == '-')
                {
                    string[] Tok = args[i].Split(delim);

                    Configurable cfg = null;
                    var key = "";

                    if (Tok.Length < 3)
                    {
                        key = Tok[1].ToLower();
                        if( !key.Equals("log") && !key.Equals("cfg") && !key.Equals("name") )
                            cfg = App.GetRegistry(key);
                    }
                    else
                    {
                        var Pkg = Tok[1];
                        cfg = (Configurable)App.Configuration[Pkg];
                        key = Tok[2].ToLower();
                    }
                    ++i;
                    var val = args[i];
                    if(cfg != null)
                       cfg.SetValue(key, val);
                    else
                    {
                        if (key.Equals("log"))
                            App.LogDir = val;
                        else if (key.Equals("cfg"))
                            App.CfgDir = val;
                        else if (key.Equals("name"))
                            App.Name = val;
                    }
                    ++i;
                }
            } */
        }

        private static void LoadAssemblies()
        {
          /*
            var ps = Path.DirectorySeparatorChar;

            if (AssemblyDictionary == null)
                AssemblyDictionary = new Dictionary<string, string>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(r => !r.IsDynamic).ToArray<Assembly>();

            var name = Assembly.GetExecutingAssembly().GetName().Name;
         
         //   if (App.CfgDir == null)
          //      App.CfgDir = AssemblyDirectory;

          //  if (App.LogDir == null)
            //    App.LogDir = CheckOrMakeDir(Environment.GetEnvironmentVariable("TMP") + Delim + "." + App.Name);
//
            foreach (var a in assemblies)
            {
                var key = a.GetName().Name;
                if (!AssemblyDictionary.ContainsKey(key))
                 {
                    AssemblyDictionary[key] = a.GetName().ToString();
                    if (App.Name == null & !a.IsDynamic && a.Location.Contains(".exe") && !key.Contains("vshost32"))
                    {
                        App.Name = key;
                        if(App.LogDir == null)
                           App.LogDir = CheckOrMakeDir(Environment.GetEnvironmentVariable("TMP") + Delim + "." + App.Name);
                    }
                 }
            }

            if (App.LogDir[App.LogDir.Length - 1] != ps)
                App.LogDir += ps;

            if (App.CfgDir[App.CfgDir.Length - 1] != ps)
                App.CfgDir += ps;
            */

        }

        private static void ProcessConfigFile(string FileName)
        {
            var FName = Path.GetFileName(FileName);
            var Ext =   Path.GetExtension(FileName);
            var BaseName = FName.Replace(Ext, " ").Trim();
            string Text = File.ReadAllText(FileName);

            if (Ext.ToLower().Equals(EXT))
            {
                string[] Tokens = BaseName.Split(PropSeparators);
                string AssembName = "";
                string PropName = "Global";
                string ClassName = BaseName;

                if (Tokens.Length == 1)
                    AssembName = BaseName.Split('.')[0];

                else if (Tokens.Length == 2)
                {

                    if (Tokens[0].Contains("."))
                    {
                        ClassName = Tokens[0];
                        AssembName = BaseName.Split('.')[0];
                        PropName = Tokens[1];
                    }
                    else
                    {
                        AssembName = Tokens[0];
                        ClassName = Tokens[1];
                    }
                }

                else if (Tokens.Length >= 3)
                {
                    AssembName = Tokens[0];
                    ClassName = Tokens[1];
                    PropName = Tokens[2];
                }

                if (AssemblyDictionary.ContainsKey(AssembName) && !Processed.Contains(AssembName))
                {
                    string fullName = ClassName + ", " + AssemblyDictionary[AssembName];
                 //   Type t = Type.GetType(fullName);
                 //   object v = JSerializer.Deserialize(t, Text);

                    var serializer = new JavaScriptSerializer();
                 //   serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    dynamic obj = serializer.Deserialize(Text, typeof(object));
                    Processed.Add(AssembName);
                }
            }
        }
    }  

}
