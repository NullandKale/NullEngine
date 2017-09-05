using System;

namespace NullEngine.Managers
{
    //this is an exception for whenever a singleton is instantiated twice
    [Serializable]
    class SingletonException : Exception
    {
        public SingletonException(object o)
        {
            Debug.Warning("Singleton Exception @ " + o.GetType().ToString());
        }
    }
}
