using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class Lamp : Destination
    {
        bool enabled = false;

        public override void onSignal(System.Diagnostics.Stopwatch currentRedTime, float power)
        {
            enabled = true;
            base.onSignal(currentRedTime, power);
        }

        public override void Update(System.Diagnostics.Stopwatch currentRedTime, BlockPos pos)
        {
            if (enabled)
            {
                if (OstBot.room.getBotMapBlock(pos.l, pos.x, pos.y).blockId == Skylight.BlockIds.Blocks.Special.GLOSSYBLACK)
                {
                    OstBot.room.DrawBlock(Block.CreateBlock(pos.l, pos.x, pos.y, Skylight.BlockIds.Blocks.Cloud.WHITE, -2));
                }
            }
            else if (OstBot.room.getBotMapBlock(pos.l, pos.x, pos.y).blockId == Skylight.BlockIds.Blocks.Cloud.WHITE)
            {
                OstBot.room.DrawBlock(Block.CreateBlock(pos.l, pos.x, pos.y, Skylight.BlockIds.Blocks.Special.GLOSSYBLACK, -2));
            }
            enabled = false;
            base.Update(currentRedTime, pos);
        }
    }
}
