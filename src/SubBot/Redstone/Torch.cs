using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OstBot_2_
{
    class Torch : PowerSource
    {
        bool willEnable = true;
        bool enabled = true;

        public override float getOutput(Stopwatch currentRedTime)
        {
            if (enabled)
                return 1.0F;
            else
                return 0.0F;
        }


        public override void onSignal(System.Diagnostics.Stopwatch currentRedTime, float power)
        {
            willEnable = false;
            //enabled = false;
            base.onSignal(currentRedTime, power);
        }

        public override void Update(System.Diagnostics.Stopwatch currentRedTime, BlockPos pos)
        {
            enabled = willEnable;
            willEnable = true;
            base.Update(currentRedTime, pos);
        }
    }
}
