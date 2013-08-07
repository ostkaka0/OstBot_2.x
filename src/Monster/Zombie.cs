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
                pathFinding = null;
                pathFinding = new PathFinding();

                Stack<Square> pathToGo = pathFinding.Begin(xBlock, yBlock, targetBotPlayer.blockX, targetBotPlayer.blockY);

                if (pathToGo != null && pathToGo.Count != 0)
                {
                    if (updateTimer.ElapsedMilliseconds >= 400)
                    {
                        updateTimer.Restart();
                        Square temp;
                        if (pathToGo.Count >= 2)
                            temp = pathToGo.Pop();
                        Square next = pathToGo.Pop();
                        xBlock = next.x;
                        yBlock = next.y;
                        zombieOldBlock = Block.CreateBlock(0, xOldBlock, yOldBlock, 4, 0);
                        OstBot.room.DrawBlock(zombieOldBlock);
                        zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
                        OstBot.room.DrawBlock(zombieBlock);
                    }
                }

                if (targetBotPlayer != null)
                {
                    if (GetDistanceBetween(targetBotPlayer, xBlock, yBlock) <= 1)
                        OstBot.connection.Send("say", "/kill " + targetBotPlayer.name);
                }
            }
            base.Update();
        }
    }
}
