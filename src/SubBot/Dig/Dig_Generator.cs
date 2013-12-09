using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Graphics.Tools.Noise;
using Skylight;

namespace OstBot_2_
{
    public partial class Dig : SubBot
    {
        private void Generate(int width, int height)
        {
            while (width == 0 || height == 0)
                Thread.Sleep(100);

            Random random = new Random();
            Graphics.Tools.Noise.Primitive.SimplexPerlin noise = new Graphics.Tools.Noise.Primitive.SimplexPerlin(random.Next(), NoiseQuality.Best);
            //f.Heightmap.
            Block[,] blockMap = new Block[width, height];

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    double distanceFromCenter = Math.Sqrt(Math.Pow(x - width / 2, 2) + Math.Pow(y - height / 2, 2))/((width>height)? width:height)*2;
                    double distanceFromCenterPow = Math.Pow(distanceFromCenter, 1.5);

                    if (noise.GetValue(x * 0.015625F, y * 0.015625F, 0) > 1 - 0.25F * distanceFromCenterPow)                 // slimy mud
                        blockMap[x, y] = Block.CreateBlock(0, x, y, 21, -1);

                    else if (noise.GetValue(x * 0.03125F, y * 0.03125F, 32) > 1 - 0.75 * distanceFromCenter)      // slimy mud
                        blockMap[x, y] = Block.CreateBlock(0, x, y, 21, -1);

                    else if (noise.GetValue(x * 0.015625F, y * 0.015625F, 48) > 1 - 0.5 * distanceFromCenter) // Water
                        blockMap[x, y] = Block.CreateBlock(0, x, y, BlockIds.Blocks.JungleRuins.BLUE, -1);

                    else if (noise.GetValue(x * 0.03125F, y * 0.03125F, 64) > 1 - 0.75 * distanceFromCenter) //wet stones
                        blockMap[x, y] = Block.CreateBlock(0, x, y, (int)BlockIds.Blocks.JungleRuins.BLUE, -1);

                    else if (noise.GetValue(x * 0.0078125F, y * 0.0078125F, 96) > 1 - 0.75 * distanceFromCenterPow)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, (int)Blocks.Stone, -1);

                    else if (noise.GetValue(x * 0.015625F, y * 0.015625F, 128) > 1 - 0.75 * distanceFromCenter)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, (int)Blocks.Stone, -1);

                    else// if (noise.GetValue(x * 0.015625F, y * 0.015625F, 160) > 0)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, Skylight.BlockIds.Blocks.Sand.BROWN, -1);

                }
            }

            Queue<Block> blockQueue = new Queue<Block>();

            for (int i = 0; i < 64; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Stone, -1));
            for (int i = 0; i < 64; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Copper, -1));
            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Iron, -1));
            for (int i = 0; i < 16; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Gold, -1));
            for (int i = 0; i < 8; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Emerald, -1));

            int amount = 1536;//2048 later

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

            blockMap[width / 2 - 1, height / 2 - 1] = Block.CreateBlock(0, width / 2 - 1, height / 2 - 1, 255, -1);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (blockMap[x, y] != null)
                    {
                        OstBot.room.DrawBlock(blockMap[x, y]);
                        resetBlockHardness(x, y, blockMap[x, y].blockId);
                    }

                    
                }
            }
        }

    }
}
