using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppTools
{
    public class App 
    {
        public static string Name
        {
            get { return Config.Default.Name;  }
            set { Config.Default.Name = value; }
        }

        public static string LogDir
        {
            get { return Config.Default.Name; }
            set { Config.Default.Name = value; }
        }

        public static string ConfigDir
        {
            get { return Config.Default.Name; }
            set { Config.Default.Name = value; }
        }

        public static string LogCons
        {
            get { return Config.Default.Name; }
            set { Config.Default.Name = value; }
        }

        public static string LogFile
        {
            get { return Config.Default.Name; }
            set { Config.Default.Name = value; }
        }
    }
}
