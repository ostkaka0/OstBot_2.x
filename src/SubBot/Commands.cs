using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Commands : SubBot
    {
        private List<string> disabledPlayers = new List<string>();

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "say":
                    {
                        int userId = m.GetInt(0);
                        string text = m.GetString(1);
                        if (text.StartsWith("!"))
                        {
                            string[] arg = text.ToLower().Split(' ');
                            string name = "";
                            Player player;

                            lock (OstBot.playerList)
                            {
                                if (OstBot.playerList.ContainsKey(userId))
                                {
                                    player = OstBot.playerList[userId];
                                    name = player.name;
                                }
                                else
                                {
                                    player = new Player(-1, "", 0, 0, 0, false, false, false, 0, false, false, 0);
                                }
                            }
                             bool isBotAdmin = (name == "ostkaka " || name == "gustav9797" || player.ismod);
                            

                            switch (arg[0])
                            {
                                case "!reset":
                                    OstBot.connection.Send("say", "/reset");
                                    break;
                                case "!loadlevel":
                                    OstBot.connection.Send("say", "/loadlevel");
                                    break;
                                case "!clear":
                                    OstBot.connection.Send("clear");
                                    break;
                                case "!kick":
                                    break;
                                case "!ban":
                                    break;
                                case "!fill":       //<blocktyp><data> / <blocktyp><lager> / <blocktyp><pengar till pengardörr>..   //med arean mellan 2 block
                                    break;
                                case "!fillworld":  //<blocktyp><data>
                                    break;
                                case "!fillarea":   //<x1><y1><x2><y2><blocktyp><data>
                                    break;
                                case "!replace":        //med arean mellan 2 block
                                    break;
                                case "!replaceworld":
                                    break;
                                case "!replacearea":
                                    break;
                                case "!disableedit":    //<spelarnamn>
                                    if (arg.Length > 1)
                                    {
                                        if (!disabledPlayers.Contains(arg[1]))
                                            disabledPlayers.Add(arg[1]);
                                    }
                                    break;
                                case "!enableedit":    //<spelarnamn>
                                    if (arg.Length > 1)
                                    {
                                        if (disabledPlayers.Contains(arg[1]))
                                            disabledPlayers.Remove(arg[1]);
                                    }
                                    break;
                                case "!rollback":   //<spelarnamn>
                                    if (arg.Length > 1)
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

                                                        if (userName != arg[1])
                                                        {
                                                            OstBot.room.DrawBlock(block);
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("block borttaget från" + userName);
                                                        }


                                                    }
                                                }
                                            }
                                        }

                                    }
                                    break;

                            }
                        }
                    }
                    break;
            }
        }

        public override void onDisconnect(object sender, string reason)
        {

        }

        public override void Update()
        {

        }

    }
}
