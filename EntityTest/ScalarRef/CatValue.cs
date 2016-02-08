using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTypes
{
    public class CatValue : Equatable
    {
        private ValState _status = ValState.OK;

        public override ValState Status { get { return _status; }  }
        public int CategoryID { get; private set; }
        public int Index { get; private set; }

        public CatValue(ValState argState)
        {
            _status = argState;
        }

        public CatValue(ScalarCategory cat, string val)
        {
            if (cat[val] == -1)
                throw new Exception("Value: " + val + " is not part of the available options in the Category");
            CategoryID = cat.CatId;
            Index = cat[val];
        }

        public CatValue(int cat, int val)
        {
            var Cat = ScalarCategory.InnerMap[cat];
            if (Cat == null)
                throw new Exception("Category Index not found: " + cat);
            var str = Cat[val];
            if (str == null)
                throw new Exception("Value: " + val + " is not part of the available options in the Category");
            CategoryID = Cat.CatId;
            Index = val;
        }

        public override string ToString()
        {
            if (Status == ValState.Err)
                return "ERR";
            if (Status == ValState.Unk)
                return "UNK";

            var Cat = ScalarCategory.Get(CategoryID);
            if (Cat == null)
                return CategoryID + "." + Index;
            return Cat.Name + "." + Cat[Index];
        }

        public static CatValue Parse(string arg)
        {
            if (arg.Equals("ERR"))
            {
                return new CatValue(ValState.Err);
            }
            if (arg.Equals("UNK"))
            {
                return new CatValue(ValState.Unk);
            }
            string[] tokens = arg.Split('.');
            if (!ScalarCategory.NamedMap.ContainsKey(tokens[0]))
                throw new ArgumentException("String " + arg + " is not parseable to a Category");
            return new CatValue(ScalarCategory.NamedMap[tokens[0]], tokens[1]);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CatValue)
            {
                var other = (CatValue)obj;
                return other.Index == Index && other.CategoryID == CategoryID;
            }
            return false;
        }
       
    }
}
