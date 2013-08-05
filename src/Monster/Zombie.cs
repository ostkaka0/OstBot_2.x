using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Zombie : Monster
    {
        PathFinding pathFinding = new PathFinding();
        Queue<Point> pathToGo = null;
        Block zombieBlock = null;
        Block zombieOldBlock = null;
        BotPlayer targetBotPlayer = null;
        int zombieDetectRadius = 16;

        public Zombie(int x, int y)
            : base(x, y)
        {
            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
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
                Console.WriteLine("Current position: X" + xBlock + " Y" + yBlock);
                Queue<Point> pathToGo = pathFinding.Begin(xBlock, yBlock, targetBotPlayer.blockX, targetBotPlayer.blockY);
                Console.WriteLine(targetBotPlayer.blockX + " target " + targetBotPlayer.blockY);
                //if (this.pathToGo == null || this.pathToGo.Peek() != pathToGo.Peek())
                this.pathToGo = pathToGo;
                //if (pathToGo != null && pathToGo.Count == 0)
                //throw new Exception("Wtf no path?:o");
                if (this.pathToGo != null)
                {
                    Point next = pathToGo.Dequeue();
                    xBlock = next.X;
                    yBlock = next.Y;
                }
            }
            base.Update();
        }

        public override void Draw()
        {
            if (zombieOldBlock != null)
                OstBot.room.DrawBlock(zombieOldBlock);
            zombieOldBlock = OstBot.room.getMapBlock(0, xBlock, yBlock, 0);
            zombieBlock = Block.CreateBlock(0, xBlock, yBlock, 32, 0);
            OstBot.room.DrawBlock(zombieBlock);

            base.Draw();
        }
    }
}
