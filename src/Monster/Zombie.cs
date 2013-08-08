using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Zombie : Monster
    {
        ZombiePathFinding pathFinding = new ZombiePathFinding();
        Block zombieBlock = null;
        Block zombieOldBlock = null;
        BotPlayer targetBotPlayer = null;
        Stopwatch updateTimer = new Stopwatch();
        Stopwatch ostkakaTimer = new Stopwatch();

        public Zombie(int x, int y)
            : base(x, y)
        {
            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
            updateTimer.Start();
        }

        public static double GetDistanceBetween(BotPlayer player, int targetX, int targetY)
        {
            double a = player.blockX - targetX;
            double b = player.blockY - targetY;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        public override void Update()
        {
            double lowestDistance = 0;
            BotPlayer lowestDistancePlayer = null;
            lock (OstBot.playerList)
            {
                foreach (BotPlayer player in OstBot.playerList.Values)
                {
                    double currentDistance = GetDistanceBetween(player, xBlock, yBlock);
                    if (currentDistance < lowestDistance || lowestDistance == 0)
                    {
                        lowestDistance = currentDistance;
                        lowestDistancePlayer = player;
                    }
                }
            }
            if (lowestDistancePlayer != null)
                targetBotPlayer = lowestDistancePlayer;
            if (targetBotPlayer != null)
            {
                Stack<Square> pathToGo = pathFinding.Begin(xBlock, yBlock, targetBotPlayer.blockX, targetBotPlayer.blockY);

                if (pathToGo != null && pathToGo.Count != 0)
                {
                    if (updateTimer.ElapsedMilliseconds >= 100)
                    {
                        updateTimer.Restart();
                        Square next = pathToGo.Peek();
                        while (xBlock == next.x && yBlock == next.y && pathToGo.Count > 0)
                        {
                            next = pathToGo.Pop();
                        }

                        if (xBlock != next.x || yBlock != next.y)
                        {
                            xBlock = next.x;
                            yBlock = next.y;
                            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, -1);
                            //Room.blockSet.Add(zombieBlock);
                            //Room.blockQueue.Enqueue(zombieBlock);
                            //OstBot.room.blockMap[0][xBlock, yBlock].Add(zombieBlock);
                            OstBot.room.DrawBlock(zombieBlock);
                            zombieOldBlock = Block.CreateBlock(0, xOldBlock, yOldBlock, 4, -1);
                            //Room.blockSet.Add(zombieOldBlock);
                            //Room.blockQueue.Enqueue(zombieOldBlock);
                            //OstBot.room.blockMap[0][xOldBlock, yOldBlock].Add(zombieOldBlock);
                            OstBot.room.DrawBlock(zombieOldBlock);
                            Console.WriteLine("WALKED X:" + xBlock + " Y: " + yBlock + " REMOVED X:" + xOldBlock + " Y:" + yOldBlock + " TIME:" + ostkakaTimer.ElapsedMilliseconds);
                            ostkakaTimer.Restart();
                        }
                    }
                }

                if (targetBotPlayer != null)
                {
                    if (!targetBotPlayer.isDead && GetDistanceBetween(targetBotPlayer, xBlock, yBlock) <= 1)
                    {
                        targetBotPlayer.killPlayer();
                        OstBot.connection.Send("say", "/kill " + targetBotPlayer.name);
                    }
                }
            }
            base.Update();
        }
    }
}
