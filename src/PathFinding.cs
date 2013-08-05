using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class PathFindingSquare
    {
        public int inhertedGValue = 0;
        public int HValue = 0;
        public int totalValue = 0;
        public PathFindingSquare()
        {

        }
    }

    class PathFinding
    {
        List<Point> blocksToCheck = new List<Point>();
        List<Point> blocksChecked = new List<Point>();
        Dictionary<Point, PathFindingSquare> blockData = new Dictionary<Point, PathFindingSquare>();

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
            blocksToCheck = new List<Point>();
            blockData = new Dictionary<Point, PathFindingSquare>();
            finalPath = new Queue<Point>();
            blocksChecked.Add(startingPoint);
            blockData.Add(startingPoint, new PathFindingSquare());
            blockData[startingPoint].inhertedGValue = 0;
            blockData[startingPoint].totalValue = 0;
            GetQuickPath(startingPoint, targetPoint);
            Point closestSquarePos;


            while (true)
            {

                int closestSquareCost = 0;
                closestSquarePos = new Point(0, 0);
                foreach (Point current in blocksToCheck)
                {
                    if (current == targetPoint)
                    {
                        finalPath.Enqueue(current);
                        return finalPath;
                    }
                    if (blockData[current].inhertedGValue < closestSquareCost || closestSquareCost == 0)
                    {
                        closestSquareCost = blockData[current].totalValue;
                        closestSquarePos = current;

                        Console.WriteLine("Closest square cost: " + closestSquareCost);
                    }
                }
                if (closestSquarePos.X == 0 && closestSquarePos.Y == 0)
                    return null;
                finalPath.Enqueue(closestSquarePos);
                blocksChecked.Add(closestSquarePos);
                blocksToCheck.Clear();
                GetQuickPath(closestSquarePos, targetPoint);
            }
        }

        public void GetQuickPath(Point currentSquare, Point targetPoint)
        {
            if (currentSquare.X == 0 && currentSquare.Y == 0)
                return;
            // Console.WriteLine("Parsngisn " + currentSquare + " and tagert " + targetPoint);
            foreach (Point p in blockMoves)
            {
                Point squareNextToCurrentSquare = new Point(currentSquare.X + p.X, currentSquare.Y + p.Y);
                Block currentBlock = OstBot.room.getMapBlock(0, squareNextToCurrentSquare.X, squareNextToCurrentSquare.Y, 0);
                if (currentBlock.blockId == 4)
                {
                    if (!blockData.ContainsKey(squareNextToCurrentSquare))
                    {
                        if (!blocksToCheck.Contains(squareNextToCurrentSquare))
                            blocksToCheck.Add(squareNextToCurrentSquare);

                        blockData.Add(squareNextToCurrentSquare, new PathFindingSquare());
                        blockData[squareNextToCurrentSquare].inhertedGValue = blockData[currentSquare].inhertedGValue + 10;
                        blockData[squareNextToCurrentSquare].HValue = CalculateSecondCost(squareNextToCurrentSquare, targetPoint);
                        blockData[squareNextToCurrentSquare].totalValue = blockData[squareNextToCurrentSquare].inhertedGValue + blockData[squareNextToCurrentSquare].HValue;
                        //GetQuickPath(nextPoint, targetPoint, blocksToCheck[nextPoint]);
                    }
                }
                //else
                //Console.WriteLine("Omg, a wall! " + currentBlock.blockId);
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
            if (currentPoint.X >= targetPoint.X)
                x = currentPoint.X - targetPoint.X;
            else
                x = targetPoint.X - currentPoint.X;

            if (currentPoint.Y >= targetPoint.Y)
                y = currentPoint.Y - targetPoint.Y;
            else
                y = targetPoint.Y - currentPoint.Y;
            Console.WriteLine("H is " + x + " " + y + " total+");
            return x + y;
        }
    }

}
