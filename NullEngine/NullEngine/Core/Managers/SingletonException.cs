using System;

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
