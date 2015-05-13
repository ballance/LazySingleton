using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonTest
{
    public class SingletonThing
    {
        private static readonly Lazy<SingletonThing> _singetonThing = new Lazy<SingletonThing>(() => new SingletonThing(), LazyThreadSafetyMode.ExecutionAndPublication);
        private static string _name1;
        private static string _name2;
        
        private SingletonThing()
        {
            _name1 = "Chris";
            _name2 = "Ballance";
        }

        public static SingletonThing Instance
        {
            get
            {
                return _singetonThing.Value;
            }
        }

        public string Name1
        {
            get { return _name1; }
            set { _name1 = value; }
        }

        public string Name2
        {
            get { return _name2; }
            set { _name2 = value; }
        }
    }
}
