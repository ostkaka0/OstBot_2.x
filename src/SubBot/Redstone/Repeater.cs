using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OstBot_2_
{
    class Repeater : PowerSource
    {
        List<bool> inputSequence = new List<bool>();

        public Repeater()
        {
            for (int i = 0; i < 25; i++)
                inputSequence.Add(false);
        }

        public override float getOutput(Stopwatch currentRedTime)
        {
            if (inputSequence[24])
                return 1.0F;
            else
                return 0.0F;
        }


        public override void onSignal(System.Diagnostics.Stopwatch currentRedTime, float power)
        {
            inputSequence[0] = true;
            //enabled = false;
            base.onSignal(currentRedTime, power);
        }

        public override void Update(System.Diagnostics.Stopwatch currentRedTime, BlockPos pos)
        {
            inputSequence.RemoveAt(24);
            inputSequence.Insert(0, false);
            base.Update(currentRedTime, pos);
        }

        public override object Create()
        {
            return new Repeater();
        }
    }
}
