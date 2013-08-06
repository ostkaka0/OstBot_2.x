using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    class Square
    {
        public int x;
        public int y;
        public int thisG = 0;
        public int G;
        public int H;
        public Square parent;
        public int F { get { return G + H; } }
        public Square(int x, int y, int G, int H, Square parent)
        {
            this.x = x;
            this.y = y;
            this.G = H;
            this.H = H;
            this.parent = parent;
        }

        public override bool Equals(object obj)
        {
            Square square = obj as Square;
            return square.x == x && square.y == y;
        }

        public bool Equals(Square square)
        {
            return square.x == x && square.y == y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 15 + x.GetHashCode() + y.GetHashCode();
        }

        public static bool operator !=(Square a, Square b)
        {
            if ((object)b == null || (object)a == null)
            {
                return (object)b != (object)a;
            }
            return a.x != b.x || a.y != b.y;
        }

        public static bool operator ==(Square a, Square b)
        {
            if ((object)b == null || (object)a == null)
            {
                return (object)b == (object)a;
            }
            return a.x == b.x && a.y == b.y;
        }
    }

    class PathFinding
    {
        public Stack<Square> closedSquares = new Stack<Square>();
        public List<Square> openSquares = new List<Square>();
        //public Dictionary<Point, Square> squareData = new Dictionary<Point, Square>();

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

        public Stack<Square> Begin(int startX, int startY, int targetX, int targetY)
        {
            this.startX = startX;
            this.startY = startY;
            this.targetX = targetX;
            this.targetY = targetY;

            Square startingSquare = new Square(startX, startY, 0, CalculateH(startX, startY, targetX, targetY), null);
            openSquares.Add(startingSquare); //Add the starting square (or node) to the open list.
            GetCloseSquareData();
            if (openSquares.Count == 0)
            {
                int lowestH = -1;
                Square parent = null;
                foreach (Square s in closedSquares)
                {
                    if (s.H < lowestH || lowestH == -1)
                    {
                        lowestH = s.H;
                        parent = s;
                    }
                }
                if (parent != null)
                {
                    Console.WriteLine("Lowest H pos X:" + parent.x + " Y:" + parent.y);
                    Stack<Square> temp = new PathFinding().Begin(startX, startY, parent.x, parent.y);
                    return temp;
                }

            }
            Stack<Square> finalPath = new Stack<Square>();
            Square currentSquare = closedSquares.Pop();
            while (currentSquare != null && currentSquare.parent != null)
            {
                Square parentOfCurrent = currentSquare.parent;
                finalPath.Push(parentOfCurrent);
                Console.WriteLine("Added square to path X:" + parentOfCurrent.x + " Y:" + parentOfCurrent.y);
                currentSquare = parentOfCurrent;
            }
            if (finalPath.Count != 0)
                Console.WriteLine("Returned " + finalPath.Count + " items!");
            return finalPath;
        }



        public void GetCloseSquareData()
        {
            int lowestF = 0;            //Look for the lowest F cost square on the open list. We refer to this as the current square.
            Square parent = null;
            foreach (Square s in openSquares)
            {
                if (s.F < lowestF || lowestF == 0)
                {
                    lowestF = s.F;
                    parent = s;
                }
            }
            if (parent == null)
                return;
            int parentX = parent.x;
            int parentY = parent.y;
            closedSquares.Push(parent); //Switch it to the closed list.
            if (parentX == targetX && parentY == targetY || openSquares.Count == 0)  //Stop when you: Add the target square to the closed list, in which case the path has been found (see note below), or fail to find the target square, and the open list is empty. In this case, there is no path.   
            {
                return;
            }
            openSquares.Remove(parent);
            foreach (Point adjacentSquare_ in adjacentSquares) //For each of the 8 squares adjacent to this current square …
            {
                int additionalCost = 0;
                if (additionalCostSquares.Contains(adjacentSquare_))
                    additionalCost = 4;
                Square adjacentSquare = new Square(parentX + adjacentSquare_.X, parentY + adjacentSquare_.Y, 0, 0, parent);
                if (!closedSquares.Contains(adjacentSquare) && OstBot.room.getMapBlock(0, adjacentSquare.x, adjacentSquare.y, 0).blockId == 4) //If it is not walkable or if it is on the closed list, ignore it.
                {
                    if (!openSquares.Contains(adjacentSquare))//If it isn’t on the open list, add it to the open list. //Make the current square the parent of this square. Record the F, G, and H costs of the square. 
                    {
                        adjacentSquare.G = parent.G + 10 + additionalCost;
                        adjacentSquare.H = CalculateH(adjacentSquare.x, adjacentSquare.x, targetX, targetY);
                        adjacentSquare.thisG = 10 + additionalCost;
                        openSquares.Add(adjacentSquare);//Make the current square the parent of this square. Record the F, G, and H costs of the square. 
                    }
                    else //If it is on the open list already,
                    {
                        int oldG = adjacentSquare.G;
                        int newG = parent.G + 10 + additionalCost;
                        if (newG < oldG)  //check to see if the G score for that square is lower if we use the parent square to get there.
                        {
                            adjacentSquare.parent = parent;
                            adjacentSquare.G = newG;
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
