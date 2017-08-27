using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine.StateMachines
{
    //this is an interface for the basic state for the state machines
    interface iState
    {
        void enter();
        void update();
    }
}
