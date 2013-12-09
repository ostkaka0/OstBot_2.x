using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OstBot_2_
{
    abstract class PowerSource : Destination
    {
        public virtual float getOutput(Stopwatch currentRedTime)
        {
            return 1.0F;
        }
    }
}
