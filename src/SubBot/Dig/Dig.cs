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

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            return;
            if (!OstBot.hasCode)
                return;
            //return;
            try
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
                                lock (OstBot.playerList)
                                {
                                    if (OstBot.playerList.ContainsKey(userId))
                                        name = OstBot.playerList[userId].name;
                                }

                                switch (arg[0])
                                {
                                    case "!digcommands":
                                        OstBot.connection.Send(PlayerIOClient.Message.Create("say", "commands: !xp, !level, !inventory, !xpleft, !buy <item> <amount>, !sell <item> <amount>, "));
                                        break;

                                    case "!generate":
                                        if (name == "ostkaka" || name == "gustav9797")
                                        {
                                            new Thread(() =>
                                                {
                                                    try
                                                    {
                                                        Generate(OstBot.room.width, OstBot.room.height);//lock(OstBot.playerList
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        OstBot.shutdown();
                                                        throw new Exception("wtf", e);
                                                    }
                                                }).Start();
                                        }
                                        break;

                                    case "!givexp":
                                        if (name == "ostkaka" || name == "gustav9797" && arg.Length > 2)
                                        {
                                            BotPlayer receiver;
                                            lock (OstBot.playerList)
                                            {
                                                if (OstBot.nameList.ContainsKey(arg[1]))
                                                {
                                                    receiver = OstBot.playerList[OstBot.nameList[arg[1]]];
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }

                                            int xp = Int32.Parse(arg[2]);

                                            receiver.digXp += xp;

                                        }
                                        break;

                                    //case "!cheat":

                                    case "!xp":
                                        lock (OstBot.playerList)
                                            OstBot.connection.Send("say", name + ": Your xp: " + OstBot.playerList[userId].digXp);
                                        break;
                                    case "!xpleft":
                                        lock (OstBot.playerList)
                                            OstBot.connection.Send("say", name + ": You need" + (OstBot.playerList[userId].xpRequired_ - OstBot.playerList[userId].digXp).ToString() + " for level " + OstBot.playerList[userId].digLevel.ToString());
                                        break;
                                    case "!level":
                                        lock (OstBot.playerList)
                                            OstBot.connection.Send("say", name + ": Level: " + OstBot.playerList[userId].digLevel);
                                        break;
                                    case "!inventory":
                                        {
                                            lock (OstBot.playerList)
                                                OstBot.connection.Send("say", name + ": " + OstBot.playerList[userId].inventory.ToString());
                                        }
                                        break;
                                    case "!save":
                                        {
                                            lock (OstBot.playerList)
                                                OstBot.playerList[userId].Save();
                                        }
                                        break;

                                    case "!setshop":
                                        {
                                            lock (OstBot.playerList)
                                            {
                                                int x = OstBot.playerList[userId].blockX;
                                                int y = OstBot.playerList[userId].blockY;
                                                Shop.SetLocation(x, y);
                                                OstBot.connection.Send("say", "Shop set at " + x + " " + y);
                                                OstBot.room.DrawBlock(Block.CreateBlock(0, x, y, 9, 0));
                                            }
                                        }
                                        break;
                                    case "!money":
                                        {
                                            lock (OstBot.playerList)
                                                OstBot.connection.Send("say", name + ": Money: " + OstBot.playerList[userId].digMoney);
                                        }
                                        break;
                                    case "!setmoney":
                                        {
                                        }
                                        break;
                                    case "!buy":
                                        {
                                            lock (OstBot.playerList)
                                            {
                                                BotPlayer p = OstBot.playerList[userId];
                                                if (p.blockX > Shop.xPos - 2 && p.blockX < Shop.xPos + 2)
                                                {
                                                    if (p.blockY > Shop.yPos - 2 && p.blockY < Shop.yPos + 2)
                                                    {
                                                        if (arg.Length > 1)
                                                        {
                                                            if (DigBlockMap.itemTranslator.ContainsKey(arg[1].ToLower()))
                                                            {
                                                                InventoryItem item = DigBlockMap.itemTranslator[arg[1].ToLower()];
                                                                int itemPrice = Shop.GetBuyPrice(item);

                                                                int amount = 1;
                                                                if (arg.Length >= 3)
                                                                    int.TryParse(arg[2], out amount);
                                                                if (p.digMoney >= (itemPrice * amount))
                                                                {
                                                                    p.digMoney -= itemPrice;
                                                                    p.inventory.AddItem(new InventoryItem(item.GetData()), amount);
                                                                    OstBot.connection.Send("say", "Item bought!");
                                                                }
                                                                else
                                                                    OstBot.connection.Send("say", name + ": You do not have enough money.");
                                                            }
                                                            else
                                                                OstBot.connection.Send("say", name + ": The requested item does not exist.");
                                                        }
                                                        else
                                                            OstBot.connection.Send("say", name + ": Please specify what you want to buy.");
                                                    }
                                                }
                                                OstBot.connection.Send("say", name + ": You aren't near the shop.");
                                            }
                                        }
                                        break;
                                    case "!sell":
                                        {
                                            lock (OstBot.playerList)
                                            {
                                                BotPlayer p = OstBot.playerList[userId];
                                                if (p.blockX > Shop.xPos - 2 && p.blockX < Shop.xPos + 2)
                                                {
                                                    if (p.blockY > Shop.yPos - 2 && p.blockY < Shop.yPos + 2)
                                                    {
                                                        if (arg.Length > 1)
                                                        {
                                                            string itemName = arg[1].ToLower();
                                                            if (DigBlockMap.itemTranslator.ContainsKey(itemName))
                                                            {
                                                                InventoryItem item = DigBlockMap.itemTranslator[itemName];
                                                                int itemSellPrice = Shop.GetSellPrice(item);
                                                                int amount = 1;
                                                                if (arg.Length >= 3)
                                                                    int.TryParse(arg[2], out amount);
                                                                if (p.inventory.Contains(item) != -1 && p.inventory.GetItemCount(item) >= amount)
                                                                {
                                                                    p.digMoney += itemSellPrice * amount;
                                                                    if (!p.inventory.RemoveItem(item, amount))
                                                                        throw new Exception("Could not remove item?D:");
                                                                    OstBot.connection.Send("say", name + ": Item sold! You received " + (itemSellPrice * amount) + " money.");
                                                                }
                                                                else
                                                                    OstBot.connection.Send("say", name + ": You do not have enough of that item.");
                                                            }
                                                            else
                                                                OstBot.connection.Send("say", name + ": The item does not exist.");
                                                        }
                                                        else
                                                            OstBot.connection.Send("say", name + ": Please specify what you want to sell.");
                                                    }
                                                }
                                                OstBot.connection.Send("say", name + ": You aren't near the shop.");
                                            }
                                        }
                                        break;

                                }
                            }
                        }
                        break;

                    case "m":
                        {
                            return;


                            new Thread(() =>
                                {
                                    try
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



                                        lock (OstBot.playerList)
                                        {
                                            if (!OstBot.playerList.ContainsKey(userId))
                                                return;
                                            else
                                                player = OstBot.playerList[userId];
                                        }

                                        //if (player.isgod)
                                        //    return;

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
                                                            if (distance <= 1.41421357 * (player.digRange - 1) || distance < 1.4142)
                                                                DigBlock(blockX + x + (int)Math.Ceiling(horizontal), blockY + y + (int)Math.Ceiling(vertical), player, (player.digRange - distance)*player.digStrength, false);
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
                                    }
                                    catch (Exception e)
                                    {
                                        OstBot.shutdown();
                                        throw e;
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
            catch (Exception e)
            {
                OstBot.shutdown();
                Console.WriteLine(e.ToString());//throw e;
            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void Update()
        {
            lock (dugBlocksToPlaceQueueLock)
            {
                while (dugBlocksToPlaceQueue.Count > OstBot.room.width * OstBot.room.height / 20)
                {
                    OstBot.room.DrawBlock(dugBlocksToPlaceQueue.Dequeue());
                }
            }
        }

        private bool isDigable(int blockId)
        {
            if (blockId >= Skylight.BlockIds.Blocks.Sand.BROWN - 5 && blockId <= Skylight.BlockIds.Blocks.Sand.BROWN)
                return true;
            else if (blockId >= 16 && blockId <= 21)
                return true;
            else if (blockId == BlockIds.Blocks.JungleRuins.BLUE)
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

                case BlockIds.Blocks.JungleRuins.BLUE:
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
