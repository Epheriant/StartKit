using ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTools;
using AppTools.Util;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Test
{
    public class Unit
    {
        private Int32 Count;

        public Unit(int arg)
        {
            Count = arg;
        }

        public override string ToString()
        {
            return "[" + Count + "]";
        }

        public static implicit operator Unit(Int32 value)
        {
            return new Unit(value);
        }

        public static explicit operator Int32(Unit value)
        {
            return value.Count;
        }

        public static Boolean operator >(Unit l, Unit r)
        {
            return l.Count > r.Count;
        }

        public static bool operator <(Unit l, Unit r)
        {
            return l.Count < r.Count;
        }

        public static Boolean operator >=(Unit l, Unit r)
        {
            return l.Count >= r.Count;
        }

        public static bool operator <=(Unit l, Unit r)
        {
            return l.Count <= r.Count;
        }

        public static Boolean operator ==(Unit l, Unit r)
        {
            return l.Count == r.Count;
        }

        public static bool operator !=(Unit l, Unit r)
        {
            return l.Count != r.Count;
        }

    }

    public static class UnitsExtensions
    {
       public static Unit Unit(this Int32 value)
        {
            return new Unit(value);
        }

    }

    class Program
    {

        public static void TestUnits()
        {
            var u = new Unit(3);
            Console.Out.WriteLine(u);
            Console.Out.WriteLine( 1.Unit() );
            Console.ReadKey();
        }

        public static void TestVars()
        {
            dynamic Col = new ScalarCategory("Season");
            Col += "Spring";
            Col += "Spring";
            Col += "Summer";
            Col += "Fall";
            Col += "Winter";
            Console.Out.WriteLine(Col.Spring);
            Console.Out.WriteLine(Col["Summer"]);
            Console.Out.WriteLine(Col[0]);
            Console.Out.WriteLine(Col[1]);
            Console.Out.WriteLine(Col[2]);
            Console.Out.WriteLine(Col.CatId);
            List<OrderableCatValue> tmp = new List<OrderableCatValue>();
            for (int i = 0; i < 100; i++)
            {
                tmp.Add(new OrderableCatValue(Col,"Spring"));
                tmp.Add(new OrderableCatValue(Col,"Summer"));
                tmp.Add(new OrderableCatValue(Col,"Fall"));
            }
            var x = tmp[0];
            Console.Out.WriteLine(x);
            Console.Out.WriteLine(x.Next());
            Console.Out.WriteLine(x.Next().Next());
            x = x.Next().Next();
            Console.Out.WriteLine(x.HasNext());
            x = x.Next();
            Console.Out.WriteLine(x);
            Console.Out.WriteLine(x.Previous());
            Console.Out.WriteLine(x.Previous().Previous());
            Console.Out.WriteLine(CatValue.Parse("Season.Spring").Index); 
       }

        public static void TestSerializer()
        {
            TestConfig tc = new TestConfig();
            string Data = JSerializer.Serialize(tc);
            Console.Out.WriteLine("Serialized:  " + Data);
            var d2 =
                    "{" +
                    "  \"Prop1\":\"Initialdsaf\"," +
                    "  \"Prop2\":4," +
                    "  \"Prop3\":5," +
                    "  \"Prop4\":" +
                    "  [" +
                    "    \"XA\"," +
                    "    \"XB\"," +
                    "   \"XC\"" +
                    "  ]" +
                    "  ," +
                    "  \"TestOrder\":\"SECOND\"" +
                    "}";
          //  var t2 = JSerializer.Deserialize(tc.GetType(), d2);
          //  var t3 = (TestConfig)t2;
           // Console.Out.WriteLine(t3.Prop1);
           // Console.Out.WriteLine(t3.Prop2);
           // Console.Out.WriteLine(t3.Prop3);
           // Console.Out.WriteLine(t3.TestOrder);
           // Console.In.ReadLine();
        }



       


        static void Main(string[] args)
        {

            var cfg   = new AppTools.Config();
            var input = JSerializer.PretyfyJSON(JSerializer.Serialize(cfg));
            Console.Out.WriteLine(input);

            dynamic v = DynamicConverter.ToDynamic(cfg);
            var input2 = JSerializer.PretyfyJSON(DynamicConverter.SerializeDynamic(v));
            Console.Out.WriteLine("\n\n" + input2);

          //  var cfg2 = JSerializer.Deserialize(cfg.GetType(), input);
            Console.Out.WriteLine(cfg);


            //var serializer = new JavaScriptSerializer();
            //serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            //dynamic obj = serializer.Deserialize(JSONSerializer.FormalJSON(input), typeof(object));
            //dynamic o2 = obj.Prob3b;

            //Console.Out.WriteLine(serializer.Serialize(obj.Prob3b.Pr1));
            //Console.Out.WriteLine(o2);

            //  input = "{\"Prop1\":\"Initialdsaf\",\"Prop2\":4,\"Prop3\":5,\"Prop4\":[\"XA\",\"XB\",\"XC\"],\"TestOrder\":\"SECOND\"}";

            //  Console.Out.WriteLine("In: " + input);
            //  Console.WriteLine("\n\nOut: \n" + JSONSerializer.FormalJSON(input));


            // TestUnits();
            //   TestSerializer();

            /* App.Name = "ConsoleTest";
             App.CfgDir = @"C:\tmp\test\init";
             App.LogDir   = @"C:\tmp\test\logs";

             Console.Out.WriteLine("Prop1: " + App.Config.Test.Prop1);
             Console.Out.WriteLine("Prop2: " + App.Config.Test.Prop2);
             Console.Out.WriteLine("Prop3: " + App.Config.Test.Prop3);
             Console.Out.WriteLine("Cfg: " + App.CfgDir);

             //Simmulating command line arguments

             //string[] vec = new string[] {"-Log",@"c:\tmp\test\log",  "-cfg", @"c:\tmp\test\init",  "-name",
             //                            "TestApp", "-Test.p1", "WHATEVER", "-p2", "0.3333" };
             //Settings.Load(vec);
             Settings.Load();

              Console.Out.WriteLine("Prop1:\t" + App.Config.Test.Prop1);
              Console.Out.WriteLine("Prop2:\t" + App.Config.Test.Prop2);
              Console.Out.WriteLine("Prop3:\t" + App.Config.Test.Prop3);
              Console.Out.WriteLine("TestOrder:\t" + App.Config.Test.TestOrder);

              Console.Out.WriteLine("Name:\t" + App.Name);
              Console.Out.WriteLine("CfgDir:\t" + App.CfgDir);
              Console.Out.WriteLine("LogDir:\t" + App.LogDir);

               Console.Out.WriteLine("\n\n"); */


        }
    }
}
