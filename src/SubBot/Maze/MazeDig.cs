using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class MazeDig : SubBot
    {
        Queue<Block> repairBlockQueue = new Queue<Block>();

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "m":
                    {
                        int userId = m.GetInt(0);
                        float playerPosX = m.GetFloat(1);
                        float playerPosY = m.GetFloat(2);
                        float speedX = m.GetFloat(3);
                        float speedY = m.GetFloat(4);
                        float modifierX = m.GetFloat(5);
                        float modifierY = m.GetFloat(6);
                        float horizontal = (float)Math.Ceiling(m.GetFloat(7));
                        float vertical = (float)Math.Ceiling(m.GetFloat(8));
                        int Coins = m.GetInt(9);
                        bool purple = (OstBot.isBB) ? false : m.GetBoolean(10);
                        bool hasLevitation = (OstBot.isBB) ? false : m.GetBoolean(11);

                        int blockX = (int)(playerPosX / 16 + 0.5);
                        int blockY = (int)(playerPosY / 16 + 0.5);


                        //if (player.isgod)
                        //    return;


                        if (horizontal != 0)
                            Dig(blockX, blockY, (int)horizontal, 0);

                        if (vertical != 0)
                            Dig(blockX, blockY, 0, (int)vertical);
                        
                    }
                    break;
            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
        }

        public override void Update()
        {
            while (repairBlockQueue.Count > OstBot.room.width * OstBot.room.height / 32)
            {
                OstBot.room.DrawBlock(repairBlockQueue.Dequeue());
            }
        }

        private void Dig(int x, int y, int mx, int my)
        {
            Block block = OstBot.room.getBotMapBlock(0, x, y);
            while (block.blockId == 196 || block.blockId == 4)
            {
                if (block.blockId != 4)
                {
                    repairBlockQueue.Enqueue(OstBot.room.getBotMapBlock(0, x, y));
                    OstBot.room.DrawBlock(Block.CreateBlock(0, x, y, 4, -1));
                }

                x += mx;
                y += my;

                if (x > 0 && y > 0 && x < OstBot.room.width - 1 && y < OstBot.room.height - 1)
                {
                    block = OstBot.room.getBotMapBlock(0, x, y);
                    continue;
                }
                else
                    break;
            }
        }
    }
}
