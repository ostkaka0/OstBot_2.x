using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OstBot_2_
{
    public class Commands : SubBot
    {
        private List<string> disabledPlayers = new List<string>();
        private List<string> protectedPlayers = new List<string>();
        private List<string> getPlacerPlayers = new List<string>();

        public Commands()
            : base()
        {
            enabled = true;
        }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            Console.WriteLine(m.Type);
            switch (m.Type)
            {
                case "b":
                    new Task(() =>
                    {
                        string name = "";
                        if (m.Count >= 5)
                        {
                            lock (OstBot.playerList)
                            {
                                if (OstBot.playerList.ContainsKey(m.GetInt(4)))
                                {
                                    name = OstBot.playerList[m.GetInt(4)].name;
                                }
                            }
                            if (name == "") // else if
                            {
                                lock (OstBot.leftPlayerList)
                                {
                                    if (OstBot.leftPlayerList.ContainsKey(m.GetInt(4)))
                                    {
                                        name = OstBot.leftPlayerList[m.GetInt(4)].name;
                                    }
                                }
                            }
                        }

                        if (disabledPlayers.Contains(name))
                        {
                            /*return;
                        }*/
                            Block trollBlock = new Block(m);
                            Block block;
                            for (int i = 0; true; i++)
                            {
                                block = OstBot.room.getMapBlock(trollBlock.layer, trollBlock.x, trollBlock.y, i);
                                string name2 = "";

                                lock (OstBot.playerList)
                                {
                                    if (OstBot.playerList.ContainsKey(block.b_userId))
                                    {
                                        name2 = OstBot.playerList[block.b_userId].name;
                                    }
                                }
                                if (name == "") //else if
                                {
                                    lock (OstBot.leftPlayerList)
                                    {
                                        if (OstBot.leftPlayerList.ContainsKey(block.b_userId))
                                        {
                                            name2 = OstBot.leftPlayerList[block.b_userId].name;
                                        }
                                    }
                                }

                                if (!disabledPlayers.Contains(name2))
                                    break;
                            }
                            OstBot.room.DrawBlock(block);
                        }
                        else if (!protectedPlayers.Contains(name))
                        {
                            Block trollBlock = new Block(m);
                            Block oldBlock = OstBot.room.getMapBlock(trollBlock.layer, trollBlock.x, trollBlock.y, 1);

                            lock (OstBot.playerList)
                            {
                                if (OstBot.playerList.ContainsKey(oldBlock.b_userId))
                                {
                                    if (protectedPlayers.Contains(OstBot.playerList[oldBlock.b_userId].name))
                                    {
                                        OstBot.room.DrawBlock(oldBlock);
                                    }
                                }
                            }
                        }
                        if (getPlacerPlayers.Contains(name))
                        {
                            getPlacerPlayers.Remove(name);
                            string name2 = "[undefined]";
                            Block block = new Block(m);
                            Block oldBlock = OstBot.room.getMapBlock(block.layer, block.x, block.y, 1);

                            lock(OstBot.playerList)
                            {
                                if (OstBot.playerList.ContainsKey(oldBlock.b_userId))
                                {
                                    name2 = OstBot.playerList[oldBlock.b_userId].name;
                                }
                            }
                            if (name2 == "[undefined]")
                            {
                                lock(OstBot.leftPlayerList)
                                {
                                    if (OstBot.leftPlayerList.ContainsKey(oldBlock.b_userId))
                                    {
                                        name2 = OstBot.leftPlayerList[oldBlock.b_userId].name;
                                    }
                                }
                            }

                            OstBot.connection.Send("say", name + ": Block placed by " + name2);
                            OstBot.room.DrawBlock(oldBlock);
                        }
                    }).Start();
                    break;
            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
            

            switch (args[0])
            {
                case "woot":
                    if (isBotMod)
                        OstBot.connection.Send("wootup");
                    break;
                case "reset":
                    if (isBotMod)
                        OstBot.connection.Send("say", "/reset");
                    break;
                case "loadlevel":
                    if (isBotMod)
                        OstBot.connection.Send("say", "/loadlevel");
                    break;
                case "save":
                    if (isBotMod)
                        OstBot.connection.Send("save");
                    break;
                case "clear":
                    if (isBotMod)
                        OstBot.connection.Send("clear");
                    break;
                case "kick":
                    break;
                case "ban":
                    break;
                case "fill":       //<blocktyp><data> / <blocktyp><lager> / <blocktyp><pengar till pengardörr>..   //med arean mellan 2 block
                    new Task(() =>
                    {
                        if (args.Length > 2 && isBotMod)
                        {
                            int blockId;
                            int layer;
                            Int32.TryParse(args[1], out blockId);
                            Int32.TryParse(args[2], out layer);
                            for (int y = 1; y < OstBot.room.height - 1; y++)
                            {
                                for (int x = 1; x < OstBot.room.width - 1; x++)
                                {
                                    OstBot.room.DrawBlock(Block.CreateBlock(layer, x, y, blockId, -1));
                                }
                            }
                        }
                        else if (args.Length > 1 && isBotMod)
                        {
                            int blockId;
                            Int32.TryParse(args[1], out blockId);
                            int layer = (blockId >= 500) ? 1 : 0;
                            for (int y = 1; y < OstBot.room.height - 1; y++)
                            {
                                for (int x = 1; x < OstBot.room.width - 1; x++)
                                {
                                    OstBot.room.DrawBlock(Block.CreateBlock(layer, x, y, blockId, -1));
                                }
                            }
                        }
                    }).Start();
                    break;
                case "fillworld":  //<blocktyp><data>
                    break;
                case "fillarea":   //<x1><y1><x2><y2><blocktyp><data>
                    break;
                case "replace":        //med arean mellan 2 block
                    new Task(() =>
                    {
                        if (args.Length > 2 && isBotMod)
                        {
                            int blockId1, blockId2;
                            Int32.TryParse(args[1], out blockId1);
                            Int32.TryParse(args[2], out blockId2);
                            int layer1 = (blockId1 >= 500) ? 1 : 0;
                            int layer2 = (blockId2 >= 500) ? 1 : 0;
                            for (int y = 1; y < OstBot.room.height - 1; y++)
                            {
                                for (int x = 1; x < OstBot.room.width - 1; x++)
                                {
                                    if (OstBot.room.getBotMapBlock(layer1, x, y).blockId == blockId1)
                                        OstBot.room.DrawBlock(Block.CreateBlock(layer2, x, y, blockId2, -1));
                                }
                            }
                        }
                    }).Start();
                    break;
                case "replaceworld":
                    break;
                case "replacearea":
                    break;

                case "getplacer":
                    if (!getPlacerPlayers.Contains(name))
                        getPlacerPlayers.Add(name);
                    break;
                case "protect":         //<name>
                    if (args.Length > 1 && isBotMod)
                    {
                        if (!protectedPlayers.Contains(args[1]))
                            protectedPlayers.Add(args[1]);
                    }
                    break;
                case "unprotect":         //<name>
                    if (args.Length > 1 && isBotMod)
                    {
                        if (protectedPlayers.Contains(args[1]))
                            protectedPlayers.Remove(args[1]);
                    }
                    break;
                case "repair":          //<name>
                    if (args.Length > 1 && isBotMod)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            for (int y = 0; y < OstBot.room.height; y++)
                            {
                                for (int x = 0; x < OstBot.room.width; x++)
                                {
                                    Block block;

                                    //if (OstBot.room.getBotMapBlock(l, x, y).b_userId == -1)
                                    //    continue;


                                    for (int i = 0; true; i++)
                                    {
                                        block = OstBot.room.getMapBlock(l, x, y, i);
                                        if (block == null)
                                            break;
                                        string userName = "";
                                        lock (OstBot.playerList)
                                        {
                                            if (OstBot.playerList.ContainsKey(block.b_userId))
                                            {
                                                userName = OstBot.playerList[block.b_userId].name;
                                            }
                                        }
                                        if (userName == "") //else if
                                        {
                                            lock (OstBot.leftPlayerList)
                                            {
                                                if (OstBot.leftPlayerList.ContainsKey(block.b_userId))
                                                {
                                                    userName = OstBot.leftPlayerList[block.b_userId].name;
                                                }
                                            }
                                        }

                                        //if (protectedPlayers.Contains(userName))
                                        //    break;

                                        if (userName == args[1])
                                        {
                                            OstBot.room.DrawBlock(block);
                                            break;
                                        }
                                        else
                                        {
                                            //Console.WriteLine("block borttaget från" + userName);
                                        }


                                    }
                                }
                            }
                        }
                    }
                    break;
                case "repairprotected":          //<name>
                    if (args.Length > 1 && isBotMod)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            for (int y = 0; y < OstBot.room.height; y++)
                            {
                                for (int x = 0; x < OstBot.room.width; x++)
                                {
                                    Block block;

                                    //if (OstBot.room.getBotMapBlock(l, x, y).b_userId == -1)
                                    //    continue;


                                    for (int i = 0; true; i++)
                                    {
                                        block = OstBot.room.getMapBlock(l, x, y, i);
                                        if (block == null)
                                            break;
                                        string userName = "";
                                        lock (OstBot.playerList)
                                        {
                                            if (OstBot.playerList.ContainsKey(block.b_userId))
                                            {
                                                userName = OstBot.playerList[block.b_userId].name;
                                            }
                                        }
                                        if (userName == "") //else if
                                        {
                                            lock (OstBot.leftPlayerList)
                                            {
                                                if (OstBot.leftPlayerList.ContainsKey(block.b_userId))
                                                {
                                                    userName = OstBot.leftPlayerList[block.b_userId].name;
                                                }
                                            }
                                        }

                                        //
                                        //    break;

                                        if (protectedPlayers.Contains(userName))//if (userName == args[1])
                                        {
                                            OstBot.room.DrawBlock(block);
                                            break;
                                        }
                                        else
                                        {
                                            //Console.WriteLine("block borttaget från" + userName);
                                        }


                                    }
                                }
                            }
                        }
                    }
                    break;
                case "disableedit":    //<spelarnamn>
                    if (args.Length > 1 && isBotMod)
                    {
                        if (!disabledPlayers.Contains(args[1]))
                            disabledPlayers.Add(args[1]);
                    }
                    break;
                case "enableedit":    //<spelarnamn>
                    if (args.Length > 1 && isBotMod)
                    {
                        if (disabledPlayers.Contains(args[1]))
                            disabledPlayers.Remove(args[1]);
                    }
                    break;
                case "rollback":   //<spelarnamn>
                    if (args.Length > 1 && isBotMod)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            for (int y = 0; y < OstBot.room.height; y++)
                            {
                                for (int x = 0; x < OstBot.room.width; x++)
                                {
                                    Block block;

                                    if (OstBot.room.getBotMapBlock(l, x, y).b_userId == -1)
                                        continue;


                                    for (int i = 0; true; i++)
                                    {
                                        block = OstBot.room.getMapBlock(l, x, y, i);

                                        if (block == null)
                                            break;

                                        string userName = "";
                                        lock (OstBot.playerList)
                                        {
                                            if (OstBot.playerList.ContainsKey(block.b_userId))
                                            {
                                                userName = OstBot.playerList[block.b_userId].name;
                                            }
                                        }
                                        if (userName == "") //else if
                                        {
                                            lock (OstBot.leftPlayerList)
                                            {
                                                if (OstBot.leftPlayerList.ContainsKey(block.b_userId))
                                                {
                                                    userName = OstBot.leftPlayerList[block.b_userId].name;
                                                }
                                            }
                                        }

                                        if (userName != args[1])
                                        {
                                            OstBot.room.DrawBlock(block);
                                            break;
                                        }
                                        else
                                        {
                                            //Console.WriteLine("block borttaget från" + userName);
                                        }


                                    }
                                }
                            }
                        }
                    }
                    break;

                case "votedisable":
                    if (args.Length > 1)
                    {
                        int playerId;
                        lock (OstBot.nameList)
                        {
                            if (OstBot.nameList.ContainsKey(args[1]))
                            {
                                playerId = OstBot.nameList[args[1]];
                            }
                            else
                            {
                                break;
                            }
                        }
                        Player playerToDisable;
                        lock (OstBot.playerList)
                        {
                            if (OstBot.playerList.ContainsKey(playerId))
                            {
                                playerToDisable = OstBot.playerList[playerId];
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        public override void Update()
        {

        }

    }
}
