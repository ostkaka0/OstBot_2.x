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
            foreach (BotPlayer player in OstBot.playerList.Values)
            {
                if (IsWithinRange(player))//ayer is within detection radius
                {
                    if (targetBotPlayer != null && !IsWithinRange(targetBotPlayer) ||targetBotPlayer == null)
                    {
                        targetBotPlayer = player;
                    }
                    break;

                }
            }

            if (targetBotPlayer != null && IsWithinRange(targetBotPlayer))
            {
                Console.WriteLine("Current position: X" + xBlock + " Y" + yBlock);

                pathFinding = new PathFinding();
                Queue<Point> pathToGo = pathFinding.Begin(xBlock, yBlock, targetBotPlayer.blockX, targetBotPlayer.blockY);

                Console.WriteLine(targetBotPlayer.blockX + " target " + targetBotPlayer.blockY);
                this.pathToGo = pathToGo;
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

        public bool IsWithinRange(BotPlayer player)
        {
            if (player.blockX < xBlock + zombieDetectRadius && player.blockX > xBlock - zombieDetectRadius)
            {
                if (player.blockY < yBlock + zombieDetectRadius && player.blockY > yBlock - zombieDetectRadius)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
