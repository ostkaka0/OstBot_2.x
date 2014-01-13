using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OstBot_2_
{
    abstract class Destination
    {
        public abstract object Create();

        public virtual void Update(Stopwatch currentRedTime, BlockPos pos)
        {

        }

        public virtual void onSignal(Stopwatch currentRedTime, float power)
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
