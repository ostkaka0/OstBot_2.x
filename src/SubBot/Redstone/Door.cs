using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class Door : Lamp
    {
        public override void Update(System.Diagnostics.Stopwatch currentRedTime, BlockPos pos)
        {
            if (enabled)
            {
                if (OstBot.room.getBotMapBlock(pos.l, pos.x, pos.y).blockId == Skylight.BlockIds.Blocks.Scifi.GRAY)
                {
                    OstBot.room.DrawBlock(Block.CreateBlock(pos.l, pos.x, pos.y, Skylight.BlockIds.Blocks.Secrets.NONSOLID, -2));
                }
            }
            else if (OstBot.room.getBotMapBlock(pos.l, pos.x, pos.y).blockId == Skylight.BlockIds.Blocks.Secrets.NONSOLID)
            {
                OstBot.room.DrawBlock(Block.CreateBlock(pos.l, pos.x, pos.y, Skylight.BlockIds.Blocks.Scifi.GRAY, -2));
            }
            enabled = false;
        }

        public override object Create()
        {
            return new Door();
        }
    }
}
