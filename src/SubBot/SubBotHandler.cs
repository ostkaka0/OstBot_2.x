using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OstBot_2_
{
    public class SubBotHandler
    {
        private static List<SubBot> subBotList = new List<SubBot>();

        public static void AddSubBot(SubBot subBot)
        {
            lock (subBotList)
                subBotList.Add(subBot);

            Program.form1.Invoke(new Action(() =>
                {
                    Program.form1.checkedListBox_SubBots.Items.Add(subBot);
                    subBot.id = Program.form1.checkedListBox_SubBots.Items.Count - 1;
                }));
        }

        public static void RemoveSubBot(SubBot subBot)
        {
            lock (subBotList)
                subBotList.Remove(subBot);
        }

        public static void OnMessage(object sender, PlayerIOClient.Message m)
        {
            lock (subBotList)
            {
                foreach (SubBot subBot in subBotList)
                {
                    if (subBot.enabled)
                    {
                        new Task(() =>//new Thread(() =>
                            {
                                subBot.onMessage(sender, m);
                            }).Start();
                    }
                }
            }
        }

        public static void OnDisconnect(object sender, string reason)
        {
            lock (subBotList)
            {
                foreach (SubBot subBot in subBotList)
                {
                    if (subBot.enabled)
                    {
                        new Task(() =>
                        {
                            subBot.onDisconnect(sender, reason);
                        }).Start();
                    }
                }

                subBotList.Clear();

                Program.form1.Invoke(new Action(()=>
                    Program.form1.checkedListBox_SubBots.Items.Clear()
                    ));
            }
        }

        public static void onCommand(object sender, string text, int userId)
        {
            string[] args = text.Split(' ');

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
            bool isBotMod = (name == "ostkaka" || name == "botost" || name == "gustav9797" || name == "gbot" || player.ismod || userId == -1);

            lock (subBotList)
            {
                foreach (SubBot subBot in subBotList)
                {
                    if (subBot.enabled)
                    {
                        new Task(() =>//new Thread(() =>
                        {
                            subBot.onCommand(sender, text, args, userId, player, name, isBotMod);
                        }).Start();
                    }
                }
            }
        }
    }
}
