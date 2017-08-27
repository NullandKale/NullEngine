using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine.Managers
{
    //this is an exception for whenever a singleton is instantiated twice
    [Serializable]
    class SingletonException : Exception
    {
        public SingletonException(object o)
        {
            Console.WriteLine("Singleton Exception @ " + o.GetType().ToString());
        }
    }
}
