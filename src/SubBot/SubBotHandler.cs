using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OstBot_2_
{
    public class SubBotHandler
    {
        private static List<SubBot> subBotList = new List<SubBot>();

        public static void AddSubBot(SubBot subBot)
        {
            lock (subBotList)
                subBotList.Add(subBot);
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
                    new Thread(() =>
                        {
                            subBot.onMessage(sender, m);
                        }).Start();
                }
            }
        }

        public static void OnDisconnect(object sender, string reason)
        {
            lock (subBotList)
            {
                foreach (SubBot subBot in subBotList)
                {
                    new Thread(() =>
                    {
                        subBot.onDisconnect(sender, reason);
                    }).Start();
                }

                subBotList.Clear();
            }
        }
    }
}
