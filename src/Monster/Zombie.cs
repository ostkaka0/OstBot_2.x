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
        Stack<Point> pathToGo = null;
        Block zombieBlock = null;
        Block zombieOldBlock = null;
        BotPlayer targetBotPlayer = null;
        int zombieDetectRadius = 16;
        Stopwatch updateTimer = new Stopwatch();
        Stopwatch drawTimer = new Stopwatch();

        public Zombie(int x, int y)
            : base(x, y)
        {
            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
            updateTimer.Start();
            drawTimer.Start();
        }

        ~Zombie()
        {

        }

        public override void Update()
        {
            if (targetBotPlayer == null) //Find a player to target
            {
                foreach (BotPlayer player in OstBot.playerList.Values)
                {
                    if (player.blockX < xBlock + zombieDetectRadius && player.blockX > xBlock - zombieDetectRadius)
                    {
                        if (player.blockY < yBlock + zombieDetectRadius && player.blockY > yBlock - zombieDetectRadius)
                        {
                            //Player is within detection radius
                            targetBotPlayer = player;
                            break;
                        }
                    }
                }
            }
            else if (targetBotPlayer != null)
            {
                //targetBotPlayer = OstBot.playerList[OstBot.nameList[targetPlayer]];
                //Console.WriteLine("Current position: X" + xBlock + " Y" + yBlock);
                //if (this.pathToGo == null || this.pathToGo.Count == 0 || targetBotPlayer.blockX != pathFinding.targetX || targetBotPlayer.blockY != pathFinding.targetY)
                //{
                //zombieOldBlock = Block.CreateBlock(0, (xOldPos / 16), (yOldPos / 16), 4, 0);
                //OstBot.room.DrawBlock(zombieOldBlock);

                pathFinding = null;
                pathFinding = new PathFinding();

                Stack<Point> pathToGo = pathFinding.Begin(xBlock, yBlock, targetBotPlayer.blockX, targetBotPlayer.blockY);
                this.pathToGo = pathToGo;
                //}
                //Console.WriteLine(targetBotPlayer.blockX + " target " + targetBotPlayer.blockY);

                if (pathToGo != null && pathToGo.Count != 0)
                {
                    if (updateTimer.ElapsedMilliseconds >= 400)
                    {
                        updateTimer.Restart();
                        Point temp;
                        if (pathToGo.Count >= 2)
                            temp = pathToGo.Pop();
                        Point next = pathToGo.Pop();
                        xBlock = next.X;
                        yBlock = next.Y;
                        //if (xOldPos != xPos || yOldPos != yPos)
                        {
                            zombieOldBlock = Block.CreateBlock(0, (xOldPos / 16), (yOldPos / 16), 4, 0);
                            OstBot.room.DrawBlock(zombieOldBlock);
                            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
                            OstBot.room.DrawBlock(zombieBlock);
                        }
                    }
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            /*if (drawTimer.ElapsedMilliseconds >= 400)
            {
                drawTimer.Restart();
                if (xOldPos != xPos || yOldPos != yPos)
                {
                    zombieOldBlock = Block.CreateBlock(0, (xOldPos / 16), (yOldPos / 16), 4, 0);
                    OstBot.room.DrawBlock(zombieOldBlock);
                    zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
                    OstBot.room.DrawBlock(zombieBlock);
                }
            }*/
            base.Draw();
        }
    }
}
