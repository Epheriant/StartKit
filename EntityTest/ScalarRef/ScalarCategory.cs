using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTypes
{
    public class ScalarCategory : DynamicObject
    {
        public const int NOT_FOUND = -1;
        public static object ClassMonitor = new object();
        public static readonly Dictionary<int, ScalarCategory> InnerMap = new Dictionary<int, ScalarCategory>();
        public static readonly Dictionary<string, ScalarCategory> NamedMap = new Dictionary<string, ScalarCategory>();

        public static ScalarCategory Get(int idx)
        {
            if (!InnerMap.ContainsKey(idx))
                return null;
            return InnerMap[idx];
        }

        protected Dictionary<string, int> NameToId = new Dictionary<string, int>();
        protected List<string> IdToName = new List<string>();
        protected Dictionary<string, CatValue> Samples = new Dictionary<string, CatValue>();
        internal List<CatValue> SampleList = new List<CatValue>();

        private object Monitor = new object();
        public int CatId { get; private set; }
        public string Name { get; private set; }


        public ScalarCategory()
        {
            Name = "Type_" + (InnerMap.Count + 1);
            AddInstance(this);
        }

        public CatValue After(int idx)
        {
            idx++;
            if (idx < 0 || idx >= IdToName.Count)
                return null;
            return SampleList[idx];
        }

        public CatValue Before(int idx)
        {
            idx--;
            if (idx < 0 || idx >= IdToName.Count)
                return null;
            return SampleList[idx];
        }

        public ScalarCategory(string name)
        {
            Name = name;
            AddInstance(this);
        }

        private static void AddInstance(ScalarCategory inst)
        {
            lock (ClassMonitor)
            {
                if (NamedMap.ContainsKey(inst.Name))
                    return;
                int Idx = InnerMap.Count();
                InnerMap.Add(Idx, inst);
                inst.CatId = Idx;
                NamedMap[inst.Name] = inst;
            }
        }

        public int Count { get { return SampleList.Count; } }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            CatValue ret = null;
            var ans = Samples.TryGetValue(name, out ret);
            result = ret;
            return ans;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }



        public void Add(string arg)
        {
            if (NameToId.ContainsKey(arg))
                return;
            lock (Monitor)
            {
                int idx = IdToName.Count();
                NameToId[arg] = idx;
                IdToName.Add(arg);
                var tmp = New(arg);
                Samples[arg] = tmp;
                SampleList.Add(tmp);
            }
        }

        public int this[string arg]
        {
            get
            {
                int ret = NOT_FOUND;
                lock (Monitor)
                {
                    if (NameToId.ContainsKey(arg))
                        ret = NameToId[arg];
                }
                return ret;
            }
        }


        public string this[int idx]
        {
            get
            {
                string ret = null;
                lock (Monitor)
                {
                    if (idx >= 0 && idx < IdToName.Count)
                        ret = IdToName[idx];
                }
                return ret;
            }
        }

        public static ScalarCategory operator +(ScalarCategory arg1, string arg2)
        {
            arg1.Add(arg2);
            return arg1;
        }

        public CatValue New(string arg)
        {
            return new CatValue(this, arg);
        }

        public CatValue New(int arg)
        {
            return new CatValue(CatId, arg);
        }

    }
}
