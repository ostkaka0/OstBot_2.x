using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class Dig : SubBot
    {
        public class Generator
        {
            public static void Generate(int width, int height)
            {
                Block[,] blockMap = new Block[width,height];

                Random random = new Random();

                for (int x = 1; x < width-1; x++)
                {
                    for (int y = 30; y < height-1; y++)
                    {
                        blockMap[x,y] = Block.CreateBlock(0, x, y, 16, -1);
                    }
                }

                Queue<Block> blockQueue = new Queue<Block>();

                for (int i = 0; i < 64; i++)
                    blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), 14, -1));

                int amount = 1024;

                while (blockQueue.Count > 0 && amount > 0)
                {
                    Block block = blockQueue.Dequeue();

                    blockMap[block.x, block.y] = block;

                    if (random.Next(8) == 0)
                    {
                        Block block2 = Block.CreateBlock(block.layer, block.x, block.y, block.blockId, -1);

                        switch (random.Next(4))
                        {
                            case 0: block2.x++; break;
                            case 1: block2.y++; break;
                            case 2: block2.x--; break;
                            case 3: block2.y--; break;
                        }

                        if (!Block.Compare(blockMap[block2.x, block2.y], block2))
                        {
                            blockQueue.Enqueue(block);
                            amount--;
                        }
                    }

                    blockQueue.Enqueue(block);
                }

                for (int x = 1; x < width - 1; x++)
                {
                    for (int y = 30; y < height - 1; y++)
                    {
                        OstBot.room.DrawBlock(blockMap[x, y]);
                    }
                }
            }
        }

        void SubBot.onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    Generator.Generate(m.GetInt(10), m.GetInt(11));
                    break;

                case "say":
                    {
                        int userId = m.GetInt(0);
                        string text = m.GetString(1);
                        if (text.StartsWith("!"))
                        {
                            string[] arg = text.ToLower().Split(' ');

                            switch (arg[0])
                            {
                                case "!betadig":
                                    //lock(OstBot.playerListLock
                                    break;
                            }
                        }
                    }
                    break;

                case "m":
                    {
                        int userId = m.GetInt(0);
                        float playerPosX = m.GetFloat(1);
                        float playerPosY = m.GetFloat(2);
                        float speedX = m.GetFloat(3);
                        float speedY = m.GetFloat(4);
                        float modifierX = m.GetFloat(5);
                        float modifierY = m.GetFloat(6);
                        float horizontal = m.GetFloat(7);
                        float vertical = m.GetFloat(8);
                        int Coins = m.GetInt(9);
                        bool purple = m.GetBoolean(10);
                        bool hasLevitation = m.GetBoolean(11);
                    }
                    break;

            }
        }

        void SubBot.onDisconnect(object sender, string reason)
        {

        }
    }
}
