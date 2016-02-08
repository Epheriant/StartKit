using AppTools;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AppTools {

    public enum Level {
        PROD,
        AUDIT,
        DEVEL,
        TRACE
    }
    
    public class Triple  {
        public string First { get; set; }
        public string Second { get; set; }
        public string Third { get; set; }
    }

     public class Config  {

      public static Config Default { get; private set;  }
        
      public void Configure(dynamic obj)   {
            this.Name      = obj.Name    ?? "DefName";
            this.LogCons   = obj.LogCons != null ? obj.LogCons : true;
            this.LogDir    = obj.LogDir ?? "DefDir";
            this.ConfigDir = obj.ConfigDir ?? "ConfigDir";
            this.LogFile   = obj.LogFile != null ? obj.LogFile : true;
            this.RunLevel = obj.RunLevel != null ? (Level) obj.RunLevel : Level.PROD;
            Default = this;
        }
               
        public string  Name      { get; set; }      
        public string  LogDir   { get; set; }
        public string  ConfigDir { get; set; }
        public bool    LogCons   { get; set; }    
        public bool    LogFile   { get; set; }           
        public Level RunLevel { get; set; }   
        public Triple TestTriple { get; set; }
        public Triple[] TripleArray { get; set; }

        public Config() {
            Name = "MyApp";
            LogCons = true;
            LogFile = true;
            ConfigDir = @"C:\Temp\";
            LogDir = "C:/Logs/";
            RunLevel = Level.DEVEL;
            TestTriple = new Triple() { First = "Duarte", Second = "Sanchez", Third = "Mella" };
            var temp = new List<Triple>();
            for (int i = 0; i < 10; i++)
                temp.Add(new Triple() { First = "Duarte" + i, Second = "Sanchez" + i, Third = "Mella" + i });
            TripleArray = temp.ToArray();
        }
    }
}
