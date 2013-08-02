using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OstBot_2_
{
    public class Dig : SubBot
    {
        private void Generate(int width, int height)
        {

            Console.WriteLine("sdfsdfsfdrgsadrgdsgsdfsdf");
            Block[,] blockMap = new Block[width, height];

            Random random = new Random();

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 30; y < height - 1; y++)
                {
                    blockMap[x, y] = Block.CreateBlock(0, x, y, 16, -1);
                }
            }

            Queue<Block> blockQueue = new Queue<Block>();

            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), Skylight.BlockIds.Blocks.Glass.RED, -1));

            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), Skylight.BlockIds.Blocks.Minerals.GREEN, -1));

            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), Skylight.BlockIds.Blocks.Minerals.ORANGE, -1));

            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), 9, -1));

            int amount = 2048;

            while (blockQueue.Count > 0 && amount > 0)
            {
                Block block = blockQueue.Dequeue();

                blockMap[block.x, block.y] = block;

                if (random.Next(8) == 0)
                {
                    Block block2 = Block.CreateBlock(block.layer, block.x, block.y, block.blockId, -1);

                    switch (random.Next(4))
                    {
                        case 0: block2.x = block2.x + 1; break;
                        case 1: block2.y = block2.y + 1; break;
                        case 2: block2.x = block2.x - 1; break;
                        case 3: block2.y = block2.y - 1; break;
                    }

                    Console.WriteLine("s");

                    if (!Block.Compare(blockMap[block2.x, block2.y], block2) && block2.x > 1 && block2.y > 1 && block2.x < width-1 && block2.y < height-1)
                    {
                        blockQueue.Enqueue(block2);
                        blockMap[block2.x, block2.y] = block2;
                        amount--;
                        Console.WriteLine(amount);
                    }
                }

                blockQueue.Enqueue(block);
            }

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 30; y < height - 1; y++)
                {
                    if (blockMap[x, y] != null)
                        OstBot.room.DrawBlock(blockMap[x, y]);
                }
            }
        }

        public void onMessage(object sender, PlayerIOClient.Message m)
        {
            Console.WriteLine("sfsddf");
            switch (m.Type)
            {
                case "init":
                    Generate(m.GetInt(10), m.GetInt(11));
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
                                    new Thread(() =>
                                        {
                                            Generate(OstBot.room.width, OstBot.room.height);//lock(OstBot.playerListLock
                                        }).Start();
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

        public void onDisconnect(object sender, string reason)
        {

        }
    }
}
