using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class MazeGenerator : SubBot
    {
        BlockPos[] moves = new BlockPos[] { new BlockPos(-1, 0, 0), new BlockPos(1, 0, 0), new BlockPos(0, -1, 0), new BlockPos(0, 1, 0) };

        Random random = new Random();

        public MazeGenerator()
            : base()
        {
        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        //void onCommand(object sender, string text, string[] args, Player player, bool isBotMod, Bot bot)
        {
            if (isBotMod)
            {
                switch (args[0])
                {
                    case "genmaze":
                    case "genmazedig":
                        {
                            List<BlockPos> points = new List<BlockPos>();

                            points.Add(moves[random.Next(moves.Length)]);
                            points.Add(new BlockPos(player.blockX, player.blockY, 0));

                            int pathMaxLength;
                            int randomFactor;
                            int pathBlock;

                            if (args[0] == "genmaze")
                            {
                                pathBlock = 4;
                            }
                            else
                            {
                                pathBlock = 196;
                            }

                            if (args.Length > 1)
                            {
                                if (!Int32.TryParse(args[1], out pathMaxLength))
                                    pathMaxLength = 12;
                            }
                            else
                            {
                                pathMaxLength = 12;
                            }

                            if (args.Length > 2)
                            {
                                if (!Int32.TryParse(args[2], out randomFactor))
                                    randomFactor = 8;
                            }
                            else
                            {
                                randomFactor = 32;
                            }


                            while (points.Count > 0)
                            {
                                BlockPos point;
                                BlockPos move;


                                if (random.Next(randomFactor) == 1)
                                {// like prim's algorithm
                                    int i = random.Next(points.Count)>>1<<1;

                                    point = points[i+1];
                                    move = points[i];

                                    points.RemoveAt(i+1);
                                    points.RemoveAt(i);
                                }
                                else
                                {// like depth search
                                    point = points.Last();
                                    points.RemoveAt(points.Count - 1);

                                    move = points.Last();
                                    points.RemoveAt(points.Count - 1);
                                }

                                BlockPos wall = new BlockPos(point.x, point.y, 0);

                                point.x += 2 * move.x;
                                point.y += 2 * move.y;

                                wall.x += move.x;
                                wall.y += move.y;

                                for (int i = 0; i * i < random.Next(1, pathMaxLength * pathMaxLength); i++)
                                {
                                    if (point.x >= 1 && point.y >= 1 && point.x < OstBot.room.width - 1 && point.y < OstBot.room.height - 1)
                                    {
                                        Block b = OstBot.room.getBotMapBlock(0, point.x, point.y);
                                        if (b != null)
                                        {
                                            if (b.blockId > 8 && b.blockId < 218 && b.blockId != pathBlock)
                                            {
                                                OstBot.room.DrawBlock(Block.CreateBlock(0, wall.x, wall.y, pathBlock, -1));
                                                OstBot.room.DrawBlock(Block.CreateBlock(0, point.x, point.y, pathBlock, -1));

                                                List<BlockPos> newpoints = new List<BlockPos>();
                                                newpoints.AddRange(moves);

                                                while (newpoints.Count > 0)
                                                {
                                                    int index = random.Next(newpoints.Count);

                                                    points.Add(newpoints[index]);
                                                    points.Add(new BlockPos(point.x, point.y, 0));

                                                    newpoints.RemoveAt(index);
                                                }
                                            }
                                            else break;
                                        }
                                        else break;
                                    }
                                    else break;

                                    point.x += 2 * move.x;
                                    point.y += 2 * move.y;

                                    wall.x += 2 * move.x;
                                    wall.y += 2 * move.y;
                                }


                                /*if (random.Next(8) == 0)
                                {
                                    int index = random.Next(points.Count) >> 1 << 1;

                                    point = points[index + 1];
                                    point_B = points[index];
                                    points.RemoveAt(index);
                                    points.RemoveAt(index);
                                }
                                else
                                {
                                    point = points.Last();
                                    points.RemoveAt(points.Count - 1);
                                    point_B = points.Last(); // between the old and this dot
                                    points.RemoveAt(points.Count - 1);
                                }*/

                                /*if (random.Next(4) == 0)
                                {
                                    points.
                                }*/

                                /*if (point.x >= 1 && point.y >= 1 && point.x < OstBot.room.width - 1 && point.y < OstBot.room.height - 1)
                                {
                                    Block b = OstBot.room.getBotMapBlock(0, point.x, point.y);
                                    if (b != null)
                                    {
                                        if (b.blockId > 8 && b.blockId < 218)
                                        {
                                            OstBot.room.DrawBlock(Block.CreateBlock(0, point_B.x, point_B.y, 4, -1));
                                            OstBot.room.DrawBlock(Block.CreateBlock(0, point.x, point.y, 4, -1));

                                            List<BlockPos> newpoints = new List<BlockPos>();
                                            newpoints.AddRange(moves);

                                            for (int i = 0; newpoints.Count > 0; i++)
                                            {
                                                int index = random.Next(newpoints.Count);

                                                //BlockPos pointA = new BlockPos(point.x + newpoints[index].x, point.y + newpoints[index].y, 0);
                                                //BlockPos pointB = new BlockPos(point.x + 2 * newpoints[index].x, point.y + 2 * newpoints[index].y, 0);

                                                

                                                //if (pointB.x >= 1 && pointB.y >= 1 && pointB.x < OstBot.room.width - 1 && pointB.y < OstBot.room.height - 1)
                                                {
                                                    //if (random.Next(4) == 0)
                                                    // {
                                                    points.Add(newpoints[index]);
                                                    points.Add(point);
                                                    /*}
                                                    else
                                                    {
                                                        int index2 = random.Next(points.Count);
                                                        points.Insert(index2, 
                                                    }* /

                                                    /*Block b = OstBot.room.getBotMapBlock(0, pointB.x, pointB.y);
                                                    if (b != null)
                                                    {
                                                        if (b.blockId > 8 && b.blockId < 218)
                                                        {
                                                            //OstBot.room.DrawBlock(Block.CreateBlock(0, pointA.x, pointA.y, 4, -1));
                                                            //OstBot.room.DrawBlock(Block.CreateBlock(0, pointB.x, pointB.y, 4, -1));

                                                            points.Add(pointA);
                                                            points.Add(pointB);
                                                        }
                                                    }* /
                                                }

                                                newpoints.RemoveAt(index);
                                            }

                                        }
                                    }
                                }*/


                            }
                        }
                        break;

                    case "genmaze2":
                        {
                            List<BlockPos> points = new List<BlockPos>();

                            points.Add(new BlockPos(player.blockX, player.blockY, 0));
                            points.Add(new BlockPos(player.blockX, player.blockY, 0));

                            while (points.Count > 0)
                            {
                                BlockPos point;
                                BlockPos point_B;

                                if (random.Next(8) == 0)
                                {
                                    int index = random.Next(points.Count) >> 1 << 1;

                                    point = points[index + 1];
                                    point_B = points[index];
                                    points.RemoveAt(index);
                                    points.RemoveAt(index);
                                }
                                else
                                {
                                    point = points.Last();
                                    points.RemoveAt(points.Count - 1);
                                    point_B = points.Last(); // between the old and this dot
                                    points.RemoveAt(points.Count - 1);
                                }

                                /*if (random.Next(4) == 0)
                                {
                                    points.
                                }*/

                                if (point.x >= 1 && point.y >= 1 && point.x < OstBot.room.width - 1 && point.y < OstBot.room.height - 1)
                                {
                                    Block b = OstBot.room.getBotMapBlock(0, point.x, point.y);
                                    if (b != null)
                                    {
                                        if (b.blockId > 8 && b.blockId < 218)
                                        {
                                            OstBot.room.DrawBlock(Block.CreateBlock(0, point_B.x, point_B.y, 4, -1));
                                            OstBot.room.DrawBlock(Block.CreateBlock(0, point.x, point.y, 4, -1));

                                            List<BlockPos> newpoints = new List<BlockPos>();
                                            newpoints.AddRange(moves);

                                            for (int i = 0; newpoints.Count > 0; i++)
                                            {
                                                int index = random.Next(newpoints.Count);

                                                BlockPos pointA = new BlockPos(point.x + newpoints[index].x, point.y + newpoints[index].y, 0);
                                                BlockPos pointB = new BlockPos(point.x + 2 * newpoints[index].x, point.y + 2 * newpoints[index].y, 0);

                                                newpoints.RemoveAt(index);

                                                if (pointB.x >= 1 && pointB.y >= 1 && pointB.x < OstBot.room.width - 1 && pointB.y < OstBot.room.height - 1)
                                                {
                                                    //if (random.Next(4) == 0)
                                                    // {
                                                    points.Add(pointA);
                                                    points.Add(pointB);
                                                    /*}
                                                    else
                                                    {
                                                        int index2 = random.Next(points.Count);
                                                        points.Insert(index2, 
                                                    }*/

                                                    /*Block b = OstBot.room.getBotMapBlock(0, pointB.x, pointB.y);
                                                    if (b != null)
                                                    {
                                                        if (b.blockId > 8 && b.blockId < 218)
                                                        {
                                                            //OstBot.room.DrawBlock(Block.CreateBlock(0, pointA.x, pointA.y, 4, -1));
                                                            //OstBot.room.DrawBlock(Block.CreateBlock(0, pointB.x, pointB.y, 4, -1));

                                                            points.Add(pointA);
                                                            points.Add(pointB);
                                                        }
                                                    }*/
                                                }
                                            }

                                        }
                                    }
                                }


                            }
                        }
                        break;


                    case "genmaze1":
                        {
                            List<BlockPos> points = new List<BlockPos>();

                            points.Add(new BlockPos(player.blockX & 0xFFFE, player.blockY & 0xFFFE, 0));

                            OstBot.room.DrawBlock(Block.CreateBlock(0, points[0].x, points[0].y, 4, -1));

                            while (points.Count > 0)
                            {
                                int i = random.Next(points.Count);

                                foreach (BlockPos p in moves)
                                {
                                    BlockPos newPoint = new BlockPos(points[i].x + p.x * 2, points[i].y + p.y * 2, 0);

                                    if (newPoint.x >= 1 && newPoint.y >= 1 && newPoint.x < OstBot.room.width - 1 && newPoint.y < OstBot.room.height - 1)
                                    {
                                        Block b = OstBot.room.getBotMapBlock(0, newPoint.x, newPoint.y);
                                        if (b != null)
                                        {
                                            if (b.blockId > 8 && b.blockId < 218)
                                            {
                                                OstBot.room.DrawBlock(Block.CreateBlock(0, points[i].x + p.x, points[i].y + p.y, 4, -1));
                                                OstBot.room.DrawBlock(Block.CreateBlock(0, newPoint.x, newPoint.y, 4, -1));
                                                points.Add(newPoint);
                                            }
                                        }
                                    }

                                }

                                points.RemoveAt(i);
                            }
                        }
                        break;
                }
            }
        }


        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
        }

        public override void onDisconnect(object sender, string reason)
        {
        }

        public override void Update()
        {
        }

    }
}
