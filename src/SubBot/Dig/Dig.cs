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
    public class Dig : SubBot
    {
        protected Queue<Block> dugBlocksToPlaceQueue = new Queue<Block>();
        protected object dugBlocksToPlaceQueueLock = 0;
        protected float[,] digHardness;

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

                    Console.WriteLine(distanceFromCenter.ToString());

                    if (noise.GetValue(x * 0.0625F, y * 0.0625F, 0) > 1-0.25*distanceFromCenterPow)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, 21, -1);

                    else if (noise.GetValue(x * 0.015625F, y * 0.015625F, 32) > 1 - 0.75 * distanceFromCenter)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, 21, -1);

                    else if (noise.GetValue(x * 0.0078125F, y * 0.0078125F, 64) > 1 - 0.25 * distanceFromCenterPow)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, Skylight.BlockIds.Blocks.Sand.GRAY, -1);

                    else if (noise.GetValue(x * 0.0625F, y * 0.0625F, 96) > 1 - 0.75 * distanceFromCenter)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, Skylight.BlockIds.Blocks.Sand.GRAY, -1);

                    else if (noise.GetValue(x * 0.015625F, y * 0.015625F, 128) > 1 - 0.75 * distanceFromCenter)
                        blockMap[x, y] = Block.CreateBlock(0, x, y, (int)Blocks.Stone, -1);

                    else
                        blockMap[x, y] = Block.CreateBlock(0, x, y, Skylight.BlockIds.Blocks.Sand.BROWN, -1);
                }
            }

            Queue<Block> blockQueue = new Queue<Block>();

            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Stone, -1));
            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Copper, -1));
            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Iron, -1));
            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Gold, -1));
            for (int i = 0; i < 32; i++)
                blockQueue.Enqueue(Block.CreateBlock(0, random.Next(1, width - 1), random.Next(1, height - 1), (int)Blocks.Emerald, -1));

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

            blockMap[width / 2, height / 2 - 1] = Block.CreateBlock(0, width << 1, (height << 1) - 1, 255, -1);

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

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    //Generate(m.GetInt(10), m.GetInt(11));
                    digHardness = new float[OstBot.room.width, OstBot.room.height];

                    resetDigHardness();
                    break;

                case "reset":
                    resetDigHardness();
                    break;

                case "say":
                    {
                        int userId = m.GetInt(0);
                        string text = m.GetString(1);
                        if (text.StartsWith("!"))
                        {
                            string[] arg = text.ToLower().Split(' ');
                            string name = "";
                            lock (OstBot.playerListLock)
                            {
                                if (OstBot.playerList.ContainsKey(userId))
                                    name = OstBot.playerList[userId].name;
                            }

                            switch (arg[0])
                            {
                                case "!generate":
                                    if (name == "ostkaka" || name == "gustav9797")
                                    {
                                        new Thread(() =>
                                            {
                                                Generate(OstBot.room.width, OstBot.room.height);//lock(OstBot.playerListLock
                                            }).Start();
                                    }
                                    break;

                                //case "!cheat":

                            }
                        }
                    }
                    break;

                case "m":
                    {
                        

                        new Thread(() =>
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

                                int blockX = (int)(playerPosX / 16 + 0.5);
                                int blockY = (int)(playerPosY / 16 + 0.5);

                                BotPlayer player;

                                lock (OstBot.playerListLock)
                                {
                                    if (!OstBot.playerList.ContainsKey(userId))
                                        return;
                                    else
                                        player = OstBot.playerList[userId];
                                }
                                if (player.name == "ostkaka")
                                    Console.WriteLine(horizontal.ToString() + " " + vertical.ToString());

                                int blockId = (OstBot.room.getMapBlock(0, blockX + (int)horizontal, blockY + (int)vertical, 0).blockId);
                                if (isDigable(blockId))//(blockId >= Skylight.BlockIds.Blocks.Sand.BROWN - 5 && blockId <= Skylight.BlockIds.Blocks.Sand.BROWN)
                                {

                                    if (player.digRange > 1)
                                    {
                                        for (int x = (horizontal == 1) ? -1 : -player.digRange + 1; x < ((horizontal == -1) ? 2 : player.digRange); x++)
                                        {
                                            for (int y = (vertical == 1) ? -1 : -player.digRange + 1; y < ((vertical == -1) ? 2 : player.digRange); y++)
                                            {
                                                Console.WriteLine("snor är :" + x.ToString() + "    och skit är: " + y.ToString());


                                                if (true)//(blockId >= Skylight.BlockIds.Blocks.Sand.BROWN - 5 && blockId <= Skylight.BlockIds.Blocks.Sand.BROWN)
                                                {
                                                    float distance = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                                                    //float distanceB = (float)Math.Sqrt(Math.Pow(x - horizontal, 2) + Math.Pow(y - vertical, 2))*1.5F;

                                                    //float distance = (distanceA < distanceB)? distanceA:distanceB;

                                                    //if (distance == 0)
                                                    //    DigBlock(blockX + x + (int)Math.Ceiling(horizontal), blockY + y + (int)Math.Ceiling(vertical), player, player.digStrength, false);
                                                    if (distance <= 1.41421357 * (player.digRange-1) || distance < 1.4142)
                                                        DigBlock(blockX + x + (int)Math.Ceiling(horizontal), blockY + y + (int)Math.Ceiling(vertical), player, player.digRange-distance, false);
                                                }
                                            }
                                        }
                                        return;
                                    }
                                }
                                {
                                    if (horizontal == 0 || vertical == 0)
                                        DigBlock(blockX + (int)horizontal, blockY + (int)vertical, player, player.digStrength, true);

                                    blockId = OstBot.room.getMapBlock(0, blockX, blockY, 0).blockId;
                                    DigBlock(blockX, blockY, player, player.digStrength, true);

                                }
                            }).Start();
                    }
                    break;

                case "b":
                    {
                        int blockId = m.GetInt(3);
                        int x = m.GetInt(1);
                        int y = m.GetInt(2);

                        resetBlockHardness(x, y, blockId);
                    }
                    break;

            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void Update()
        {
            lock (dugBlocksToPlaceQueueLock)
            {
                while (dugBlocksToPlaceQueue.Count > OstBot.room.width * OstBot.room.height / 10)
                {
                    OstBot.room.DrawBlock(dugBlocksToPlaceQueue.Dequeue());
                    Console.WriteLine("jag surar!");
                }
            }
        }

        private bool isDigable(int blockId)
        {
            if (blockId >= Skylight.BlockIds.Blocks.Sand.BROWN - 5 && blockId <= Skylight.BlockIds.Blocks.Sand.BROWN)
                return true;
            else if (blockId >= 16 && blockId <= 21)
                return true;
            else
                return false;
        }

        private void DigBlock(int x, int y, BotPlayer player, float digStrength, bool mining)
        {
            if (digHardness == null)
                return;

            if (!(x > 0 && y > 0 && x < OstBot.room.width && y < OstBot.room.height))
                return;

            if (digHardness[x,y] <= 0)
                return;

            Block block = OstBot.room.getMapBlock(0, x, y, 0);

            int blockId = -1;

            if (mining)
            {
                if (DigBlockMap.blockTranslator.ContainsKey(block.blockId))
                {
                    blockId = 4;

                    InventoryItem temp = DigBlockMap.blockTranslator[block.blockId];

                    if (player.digLevel>= Convert.ToInt32(temp.GetDataAt(5)))
                    {
                        //Shop.shopInventory[DigBlockMap.blockTranslator[block.blockId]].GetDataAt(3)//för hårdhet
                        if (digHardness[x, y] <= digStrength)
                        {

                            InventoryItem newsak = new InventoryItem(temp.GetData());
                            player.inventory.AddItem(newsak, 1);
                            player.digXp += Convert.ToInt32(temp.GetDataAt(1));
                        }
                    }
                    else
                    {
                        return;
                    }
                    
                }
            }

            switch (block.blockId)
            {
                case BlockIds.Blocks.Sand.BROWN:
                    blockId = 4;
                    break;

                case BlockIds.Blocks.Sand.GRAY:
                    blockId = BlockIds.Action.Liquids.WATER;
                    break;

                case 21:
                    blockId = 369;//BlockIds.Action.Liquids.MUD;
                    break;

                default:
                    if (blockId == -1)
                        return;
                    else
                        break;
            }

            digHardness[x, y] -= digStrength;

            if (digHardness[x, y] <= 0)
            {
                OstBot.room.DrawBlock(Block.CreateBlock(0, x, y, blockId, -1));
                lock (dugBlocksToPlaceQueueLock)
                    dugBlocksToPlaceQueue.Enqueue(block);
            }
        }

        private void resetDigHardness()
        {
            for (int y = 0; y < OstBot.room.height; y++)
            {
                for (int x = 0; x < OstBot.room.width; x++)
                {
                    resetBlockHardness(x, y, OstBot.room.getMapBlock(0, x, y ,0).blockId);
                }
            }
        }

        private void resetBlockHardness(int x, int y, int blockId)
        {
            if (isDigable(blockId))
            {
                digHardness[x, y] = 1F;
            }
            else if (DigBlockMap.blockTranslator.ContainsKey(blockId))
            {
                digHardness[x, y] = Convert.ToInt32(Shop.shopInventory[DigBlockMap.blockTranslator[blockId].GetName()].GetDataAt(4));
            }
        }

    }
}
