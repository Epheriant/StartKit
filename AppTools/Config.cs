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

    public struct Choice<T> 
    {
        private T _value;

        public Choice(T arg)
        {
            if (!(arg is Enum))
                throw new Exception("Choices can only contain Enums");
            _value = arg;
        }

        public static implicit operator Choice<T>(string arg)
        {
            return new Choice<T>( (T) Enum.Parse(typeof(T), arg));
        }

        public static implicit operator T(Choice<T> arg)
        {
            return arg._value;
        }

        public static implicit operator Choice<T>(T arg)
        {
            return new Choice<T>(arg);
        }

        public override string ToString()
        {
            return _value.ToString(); 
        }
    }

    public class Triple
    {
        public string First { get; set; }
        public string Second { get; set; }
        public string Third { get; set; }
    }

     public class Config: Configurable  {

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
        public Config() {
            Name = "MyApp";
            LogCons = true;
            LogFile = true;
            ConfigDir = @"C:\Temp\";
            LogDir = "C:/Logs/";
            RunLevel = Level.DEVEL;
            TestTriple = new Triple() { First = "Duarte", Second = "Sanchez", Third = "Mella" };
        }
    }
}
