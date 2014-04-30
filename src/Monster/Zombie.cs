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
        PathFinding pathFinding = new PathFinding();
        Block zombieBlock = null;
        Block zombieOldBlock = null;
        BotPlayer targetBotPlayer = null;
        Stopwatch updateTimer = new Stopwatch();
        Stopwatch lagTimer = new Stopwatch();
        Stack<Square> pathToGo = null;

        public Zombie(int x, int y)
            : base(x, y)
        {
            updateTimer.Start();
            lagTimer.Start();
            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
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
            if (updateTimer.ElapsedMilliseconds >= 1000)
            {
                updateTimer.Restart();
                double lowestDistance = 0;
                BotPlayer lowestDistancePlayer = null;
                lock (OstBot.playerList)
                {
                    foreach (BotPlayer player in OstBot.playerList.Values)
                    {
                        if (player.isgod)
                            continue;
                        double currentDistance = GetDistanceBetween(player, xBlock, yBlock);
                        if (currentDistance < lowestDistance || lowestDistance == 0)
                        {
                            lowestDistance = currentDistance;
                            lowestDistancePlayer = player;
                        }
                    }
                }
                if (lowestDistancePlayer != null)
                {
                    targetBotPlayer = lowestDistancePlayer;

                }
            }

            if (targetBotPlayer != null && xBlock != targetBotPlayer.x && yBlock != targetBotPlayer.y)
            {
                //pathFinding = null;
                pathFinding = new PathFinding();
                //lagTimer.Restart();
                pathToGo = pathFinding.Begin(xBlock, yBlock, targetBotPlayer.blockX, targetBotPlayer.blockY, null);
                //Console.WriteLine("elapsed shitlagtime " + lagTimer.ElapsedMilliseconds + "MS");

                if (pathToGo != null && pathToGo.Count != 0)
                {
                    Square temp;
                    if (pathToGo.Count >= 2)
                        temp = pathToGo.Pop();
                    Square next = pathToGo.Pop();
                    xBlock = next.x;
                    yBlock = next.y;
                    zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, -1);
                    zombieOldBlock = Block.CreateBlock(0, xOldBlock, yOldBlock, 4, -1);
                    OstBot.room.DrawBlock(zombieBlock);
                    OstBot.room.DrawBlock(zombieOldBlock);
                }

                if (targetBotPlayer != null)
                {
                    if (!targetBotPlayer.isDead && GetDistanceBetween(targetBotPlayer, xBlock, yBlock) <= 1 && !targetBotPlayer.isgod)
                    {
                        targetBotPlayer.killPlayer();
                        OstBot.connection.Send("say", "/kill " + targetBotPlayer.name);
                    }
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
