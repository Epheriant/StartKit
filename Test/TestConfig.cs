using AppTools;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Test
{

    public enum Order
    {
        FIRST,
        SECOND,
        THIRD
    }


    [DataContract]  public class TestConfig: Configurable
    {
        public static TestConfig Default { get; private set;  }

        public void Configure(dynamic obj)
        {
            Default = this;
        }

        [DataMember]   public string       Prop1 { get; set; }       
        [DataMember]   public double       Prop2 { get; set; }        
        [DataMember]   public int          Prop3 { get; set; }       
        [DataMember]   public List<string> Prop4 { get; set; }

        public Order TestOrder { get; set; }        

        public TestConfig()
        {
            Prop1 = "Initial";
            Prop2 = 1.0;
            Prop3 = 2;
            Prop4 = new List<String>() { "A", "B", "C" };
            TestOrder = Order.THIRD;
        }
    }
}
