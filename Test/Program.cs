using ValueTypes;
using System;
using System.Collections.Generic;
using AppTools.Util;


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
            string Data = serial.Serialize(tc);
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
            var t2 = serial.Deserialize(tc.GetType(), d2);

        }

        public static ITextFormatSerializer serial = new ConfigSerializer();
        public static DynamicConverter objConv = new DynamicConverter();

        static void Main(string[] args)
        {
            var cfg   = new AppTools.Config();
            var input = serial.Serialize(cfg);
            Console.Out.WriteLine(input);

            dynamic v = objConv.ConvertTo(cfg);
            var input2 = serial.Serialize(v);
            Console.Out.WriteLine("\n\n" + input2);

            var s2 = serial.FormatJson(input2);

            Console.Out.WriteLine("\n\n" + s2);
        }
    }
}
