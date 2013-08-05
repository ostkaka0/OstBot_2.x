using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class PathFinding
    {
        List<Point> blocksChecked = new List<Point>();
        Dictionary<Point, int> blocksToCheck = new Dictionary<Point, int>();
        Queue<Point> finalPath = new Queue<Point>();

        /*static List<Point> easyOneBlockMoves = new List<Point> { 
            new Point(1, 0), 
            new Point(-1, 0), 
            new Point(0, 1), 
            new Point(0, -1) };

        static List<Point> diagonalOneBlockMoves = new List<Point> { 
            new Point(1, 1), 
            new Point(-1, 1), 
            new Point(1, 1), 
            new Point(1, -1) };
        */

        static List<Point> blockMoves = new List<Point> { 
            new Point(1, 1), 
            new Point(-1, 1), 
            new Point(1, 1), 
            new Point(1, -1),
            new Point(1, 0), 
            new Point(-1, 0), 
            new Point(0, 1), 
            new Point(0, -1)};


        public Queue<Point> Begin(int x, int y, int xTarget, int yTarget)
        {
            Point startingPoint = new Point(x, y);
            Point targetPoint = new Point(xTarget, yTarget);
            blocksChecked = new List<Point>();
            blocksToCheck = new Dictionary<Point, int>();
            finalPath = new Queue<Point>();
            GetQuickPath(startingPoint, targetPoint, 0);

            while (true)
            {
                int lowest = 10000;
                Point lowestPos = new Point(0, 0);
                foreach (KeyValuePair<Point, int> current in blocksToCheck)
                {
                    if (current.Key == targetPoint)
                    {
                        finalPath.Enqueue(current.Key);
                        return finalPath;
                    }
                    if (current.Value < lowest)
                    {
                        lowest = current.Value;
                        lowestPos = current.Key;
                    }
                }
                //if (lowestPos.X == 0 && lowestPos.Y == 0)
                    //GetQuickPath(startingPoint, targetPoint, 0);
                finalPath.Enqueue(lowestPos);
                blocksToCheck.Clear();
                GetQuickPath(lowestPos, targetPoint, lowest);
                if (blocksToCheck.Count == 0)
                    return null;
            }
        }

        public void GetQuickPath(Point currentPoint, Point targetPoint, int cost)
        {
            foreach (Point p in blockMoves)
            {
                if (!blocksChecked.Contains(p))
                {
                    Point nextPoint = new Point(currentPoint.X + p.X, currentPoint.Y + p.Y);
                    Block currentBlock = OstBot.room.getMapBlock(0, nextPoint.X, nextPoint.Y, 0);
                    if (currentBlock.blockId == 4)
                    {
                        if (!blocksToCheck.ContainsKey(nextPoint))
                        {
                            blocksToCheck.Add(nextPoint, cost + 10 + CalculateSecondCost(nextPoint, targetPoint));
                            blocksChecked.Add(currentPoint);
                            //GetQuickPath(nextPoint, targetPoint, blocksToCheck[nextPoint]);
                        }
                    }
                }
            }
            /*foreach (Point p in diagonalOneBlockMoves)
            {
                if (!blocksChecked.Contains(p))
                {
                    Point nextPoint = new Point(currentPoint.X + p.X, currentPoint.Y + p.Y);
                    Block currentBlock = OstBot.room.getMapBlock(0, nextPoint.X, nextPoint.Y, 0);
                    if (currentBlock.blockId == 4)
                    {
                        if (!blocksToCheck.ContainsKey(nextPoint))
                        {
                            blocksToCheck.Add(nextPoint, cost + 10 + CalculateSecondCost(nextPoint, targetPoint));
                            blocksChecked.Add(currentPoint);
                        }
                        //GetQuickPath(nextPoint, targetPoint, blocksToCheck[nextPoint]);
                    }
                }
            }*/
        }

        public int CalculateSecondCost(Point currentPoint, Point targetPoint)
        {
            int x = 0;
            int y = 0;
            if (currentPoint.X > targetPoint.X)
                x = currentPoint.X - targetPoint.X;
            else
                x = targetPoint.X - currentPoint.X;

            if (currentPoint.Y > targetPoint.Y)
                x = currentPoint.Y - targetPoint.Y;
            else
                x = targetPoint.Y - currentPoint.Y;

            return x + y;
        }
    }

}
