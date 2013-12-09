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
        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
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
                        digHardness = new float[OstBot.room.width, OstBot.room.height];
                        resetDigHardness();
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
                            bool purple = (OstBot.isBB) ? false : m.GetBoolean(10);
                            bool hasLevitation = (OstBot.isBB) ? false : m.GetBoolean(11);

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
                                                    DigBlock(blockX + x + (int)Math.Ceiling(horizontal), blockY + y + (int)Math.Ceiling(vertical), player, (player.digRange - distance) * player.digStrength, false);
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

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
            switch (args[0])
            {
                case "digcommands":
                    OstBot.connection.Send(PlayerIOClient.Message.Create("say", "commands: !xp, !level, !inventory, !xpleft, !buy <item> <amount>, !sell <item> <amount>, "));
                    break;

                case "generate":
                    if (isBotMod)
                    {
                        new Task(() =>
                        {
                            try
                            {
                                digHardness = new float[OstBot.room.width, OstBot.room.height];
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

                case "givexp":
                    if (isBotMod && args.Length > 2)
                    {
                        BotPlayer receiver;
                        lock (OstBot.playerList)
                        {
                            if (OstBot.nameList.ContainsKey(args[1]))
                            {
                                receiver = OstBot.playerList[OstBot.nameList[args[1]]];
                            }
                            else
                            {
                                break;
                            }
                        }

                        int xp = Int32.Parse(args[2]);

                        receiver.digXp += xp;

                    }
                    break;

                //case "cheat":

                case "xp":
                    lock (OstBot.playerList)
                        OstBot.connection.Send("say", name + ": Your xp: " + OstBot.playerList[userId].digXp);
                    break;
                case "xpleft":
                    lock (OstBot.playerList)
                        OstBot.connection.Send("say", name + ": You need" + (OstBot.playerList[userId].xpRequired_ - OstBot.playerList[userId].digXp).ToString() + " for level " + OstBot.playerList[userId].digLevel.ToString());
                    break;
                case "level":
                    lock (OstBot.playerList)
                        OstBot.connection.Send("say", name + ": Level: " + OstBot.playerList[userId].digLevel);
                    break;
                case "inventory":
                    {
                        lock (OstBot.playerList)
                            OstBot.connection.Send("say", name + ": " + OstBot.playerList[userId].inventory.ToString());
                    }
                    break;
                case "save":
                    {
                        lock (OstBot.playerList)
                            OstBot.playerList[userId].Save();
                    }
                    break;

                case "setshop":
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
                case "money":
                    {
                        lock (OstBot.playerList)
                            OstBot.connection.Send("say", name + ": Money: " + OstBot.playerList[userId].digMoney);
                    }
                    break;
                case "setmoney":
                    {
                    }
                    break;
                case "buy":
                    {
                        lock (OstBot.playerList)
                        {
                            BotPlayer p = OstBot.playerList[userId];
                            if (p.blockX > Shop.xPos - 2 && p.blockX < Shop.xPos + 2)
                            {
                                if (p.blockY > Shop.yPos - 2 && p.blockY < Shop.yPos + 2)
                                {
                                    if (args.Length > 1)
                                    {
                                        if (DigBlockMap.itemTranslator.ContainsKey(args[1].ToLower()))
                                        {
                                            InventoryItem item = DigBlockMap.itemTranslator[args[1].ToLower()];
                                            int itemPrice = Shop.GetBuyPrice(item);

                                            int amount = 1;
                                            if (args.Length >= 3)
                                                int.TryParse(args[2], out amount);
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
                case "sell":
                    {
                        lock (OstBot.playerList)
                        {
                            BotPlayer p = OstBot.playerList[userId];
                            if (p.blockX > Shop.xPos - 2 && p.blockX < Shop.xPos + 2)
                            {
                                if (p.blockY > Shop.yPos - 2 && p.blockY < Shop.yPos + 2)
                                {
                                    if (args.Length > 1)
                                    {
                                        string itemName = args[1].ToLower();
                                        if (DigBlockMap.itemTranslator.ContainsKey(itemName))
                                        {
                                            InventoryItem item = DigBlockMap.itemTranslator[itemName];
                                            int itemSellPrice = Shop.GetSellPrice(item);
                                            int amount = 1;
                                            if (args.Length >= 3)
                                                int.TryParse(args[2], out amount);
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

                case "!placestones":
                    {

                    }
                    break;

            }
        }

    }
}
