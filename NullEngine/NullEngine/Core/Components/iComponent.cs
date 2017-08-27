using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine.Component
{
    public interface iComponent
    {
        void Run(nullEngine.Entity.renderable r);
    }
}
