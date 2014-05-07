using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OstBot_2_
{
    class PressurePlate : PowerSource
    {
        bool enabled = false;

        public override float getOutput(Stopwatch currentRedTime)
        {
            if (enabled)
                return 1.0F;
            else
                return 0.0F;
        }

        public override void Update(Stopwatch currentRedTime, BlockPos pos)
        {
            enabled = false;
            lock (OstBot.playerList)
            {
                foreach (var p in OstBot.playerList)
                {
                    if (p.Value.blockX == pos.x && p.Value.blockY == pos.y)
                    {
                        enabled = true;
                        break;
                    }
                }
            }

            if (!enabled)
            {
                if (OstBot.room.getBotMapBlock(pos.l, pos.x, pos.y).blockId == Skylight.BlockIds.Decorative.Sand.WHITE)
                {
                    OstBot.room.DrawBlock(Block.CreateBlock(pos.l, pos.x, pos.y, Skylight.BlockIds.Decorative.Cloud.BOTTOM, -2));
                }
            }
            else if (OstBot.room.getBotMapBlock(pos.l, pos.x, pos.y).blockId == Skylight.BlockIds.Decorative.Cloud.BOTTOM)
            {
                OstBot.room.DrawBlock(Block.CreateBlock(pos.l, pos.x, pos.y, Skylight.BlockIds.Decorative.Sand.WHITE, -2));
            }
        }

        public override object Create()
        {
            return new PressurePlate();
        }

    }
}
