using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class square
    {
        int x;
        int y;
        public int thisG = 0;
        public int G;
        int H;
        public int parentX = 0;
        public int parentY = 0;
        public int F { get { return G + H; } }
        public square(int G, int H, int parentX, int parentY)
        {
            this.G = H;
            this.H = H;
            this.parentX = parentX;
            this.parentY = parentY;
        }
    }

    class PathFinding
    {
        public List<Point> closedSquares = new List<Point>();
        public List<Point> openListThatHasData = new List<Point>();
        public Dictionary<Point, square> squareData = new Dictionary<Point, square>();

        //public SortedList<int, Point> fScoreList = new SortedList<int, Point>();

        static List<Point> additionalCostSquares = new List<Point> { 
            new Point(1, 1), 
            new Point(-1, 1), 
            new Point(1, 1), 
            new Point(1, -1)};

        static List<Point> adjacentSquares = new List<Point> { 
            new Point(1, 1), 
            new Point(-1, 1), 
            new Point(1, 1), 
            new Point(1, -1),
            new Point(1, 0), 
            new Point(-1, 0), 
            new Point(0, 1), 
            new Point(0, -1)};

        public int startX = 0;
        public int startY = 0;
        public int targetX = 0;
        public int targetY = 0;

        Point lowestFSquare = new Point(0, 0);

        public Stack<Point> Begin(int startX, int startY, int targetX, int targetY)
        {
            this.startX = startX;
            this.startY = startY;
            this.targetX = targetX;
            this.targetY = targetY;

            squareData.Add(new Point(startX, startY), new square(0, 0, 0, 0));
            openListThatHasData.Add(new Point(startX, startY)); //Add the starting square (or node) to the open list.
            GetCloseSquareData();
            if(openListThatHasData.Count == 0)
            {
                Console.WriteLine("Could not find path!");
                return null;
            }
            Stack<Point> finalPath = new Stack<Point>();
            square currentSquare = squareData[new Point(targetX, targetY)];
            while (currentSquare.parentX != 0 && currentSquare.parentY != 0)
            {
                Point parentOfCurrent = new Point(currentSquare.parentX,  currentSquare.parentY);
                finalPath.Push(parentOfCurrent);
                //Console.WriteLine("Added square to path " + parentOfCurrent);
                if (!squareData.ContainsKey(parentOfCurrent))
                    break;
                currentSquare = squareData[parentOfCurrent];
            }
            return finalPath;
        }

        public Point GetLowestFCostInOpenList()
        {
            int lowestF = 0;
            Point lowestFSquare = this.lowestFSquare;
            foreach (Point p in openListThatHasData)
            {
                if (squareData[p].F < lowestF || lowestF == 0)
                {
                    lowestF = squareData[p].F;
                    lowestFSquare = p;
                }
            }
            return lowestFSquare;
        }

        public void GetCloseSquareData()
        {
            Point parent = GetLowestFCostInOpenList(); //Look for the lowest F cost square on the open list. We refer to this as the current square.
            int parentX = parent.X;
            int parentY = parent.Y;
            closedSquares.Add(parent); //Switch it to the closed list.
            if (parentX == targetX && parentY == targetY)  //Stop when you: Add the target square to the closed list, in which case the path has been found (see note below),
                return;
            if (openListThatHasData.Count == 0) //or fail to find the target square, and the open list is empty. In this case, there is no path.   
                return;
            openListThatHasData.Remove(parent);
            foreach (Point adjacentSquare_ in adjacentSquares) //For each of the 8 squares adjacent to this current square …
            {   
                int additionalCost = 0;
                if (additionalCostSquares.Contains(adjacentSquare_))
                    additionalCost = 4;
                Point adjacentSquare = new Point(parentX + adjacentSquare_.X, parentY + adjacentSquare_.Y);
                if (!closedSquares.Contains(adjacentSquare) && OstBot.room.getMapBlock(0, adjacentSquare.X, adjacentSquare.Y, 0).blockId == 4) //If it is not walkable or if it is on the closed list, ignore it.
                {
                    if (!openListThatHasData.Contains(adjacentSquare))//If it isn’t on the open list, 
                    {
                        openListThatHasData.Add(adjacentSquare);     //add it to the open list. 
                        square square = new square(squareData[parent].G + 10 + additionalCost, CalculateH(adjacentSquare.X, adjacentSquare.Y, targetX, targetY), parentX, parentY);
                        square.thisG = 10 + additionalCost;
                        squareData.Add(adjacentSquare, square);        //Make the current square the parent of this square. Record the F, G, and H costs of the square. 
                    }
                    else //If it is on the open list already,
                    {
                        //check to see if the G score for that square is lower if we use the parent square to get there.
                        int oldG = squareData[adjacentSquare].G;
                        int newG = squareData[parent].G + 10 + additionalCost;
                        if (newG < oldG)
                        {
                            squareData[adjacentSquare].parentX = parentX;
                            squareData[adjacentSquare].parentY = parentY;

                            squareData[adjacentSquare].G = newG;
                        }
                    }
                }
            }
            GetCloseSquareData();
        }

        public int CalculateH(int x1, int y1, int x2, int y2)
        {
            int x = 0;
            int y = 0;
            if (x1 >= x2)
                x = x1 - x2;
            else
                x = x2 - x1;

            if (y1 >= y2)
                y = y1 - y2;
            else
                y = y2 - y1;

            return x + y;
        }
    }

}
