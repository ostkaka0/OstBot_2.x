using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Zombies : SubBot
    {
        public static List<Zombie> zombieList = new List<Zombie>();
        public static Stopwatch zombieUpdateStopWatch = new Stopwatch();
        public static Stopwatch zombieDrawStopWatch = new Stopwatch();
        Random r = new Random();

        public Zombies() : base()
        {
            zombieUpdateStopWatch.Start();
            zombieDrawStopWatch.Start();
        }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            //throw new NotImplementedException();
        }

        public override void onDisconnect(object sender, string reason)
        {
            //throw new NotImplementedException();
        }

        public override void onCommand(object sender, string text, string[] args, int playerId, Player player, string name, bool isBotMod)
        {
            switch (args[0])
            {
                case "zombie":
                    {
                        Zombie zombie = new Zombie(OstBot.playerList[playerId].blockX * 16, OstBot.playerList[playerId].blockY * 16);
                        lock (zombieList)
                        {
                            zombieList.Add(zombie);
                        }
                        OstBot.room.DrawBlock(Block.CreateBlock(0, OstBot.playerList[playerId].blockX, OstBot.playerList[playerId].blockY, 32, 0));
                    }
                    break;
                case "zombies":
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            int x = r.Next(1, OstBot.room.width - 1);
                            int y = r.Next(1, OstBot.room.height - 1);
                            Zombie zombie = new Zombie(x * 16, y * 16);
                            lock (zombieList)
                            {
                                zombieList.Add(zombie);
                            }
                        }
                    }
                    break;
                case "removezombies":
                    {
                        lock (zombieList)
                        {
                            zombieList.Clear();
                        }
                    }
                    break;
            }
        }

        public override void Update()
        {
            if (OstBot.connected)
            {
                long lag = 0;
                lock (zombieList)
                {
                    foreach (Zombie zombie in zombieList)
                    {
                        zombie.Update();
                        zombie.Draw();
                        System.Threading.Thread.Sleep((int)(200 / zombieList.Count) - (int)lag);
                    }
                }
                lag = zombieUpdateStopWatch.ElapsedMilliseconds;
                zombieUpdateStopWatch.Restart();
                //Console.WriteLine(lag);
            }
        }
    }
}
