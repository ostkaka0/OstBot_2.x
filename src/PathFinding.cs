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
        public int G;
        public int H;
        public Square parent;
        public int F 
        { 
            get 
            { 
                return G + H; 
            } 
        }
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

        static Dictionary<Point, int> adjacentSquares = new Dictionary<Point, int>() { 
            {new Point(1, 1), 14}, 
            {new Point(-1, 1), 14}, 
            {new Point(-1, -1), 14}, 
            {new Point(1, -1), 14}, 
            {new Point(1, 0), 10}, 
            {new Point(-1, 0), 10}, 
            {new Point(0, 1), 10}, 
            {new Point(0, -1), 10}};

        public Stack<Square> Begin(int startX, int startY, int targetX, int targetY)
        {
            Square startingSquare = new Square(startX, startY, 0, CalculateH(startX, startY, targetX, targetY), null);
            openSquares.Add(startingSquare);
            GetCloseSquareData(targetX, targetY);
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
            if (closedSquares.Count > 0)
            {
                Square currentSquare = closedSquares.Pop();
                while (currentSquare != null && currentSquare.parent != null)
                {
                    Square parentOfCurrent = currentSquare.parent;
                    finalPath.Push(parentOfCurrent);
                    //Console.WriteLine("Added square to path X:" + parentOfCurrent.x + " Y:" + parentOfCurrent.y);
                    currentSquare = parentOfCurrent;
                }
            }
            return finalPath;
        }



        public void GetCloseSquareData(int targetX, int targetY)
        {
            Square parent = null;
            foreach (Square s in openSquares)
            {
                if (parent == null || s.F < parent.F)
                {
                    parent = s;
                }
            }

            if (parent == null || parent.x == targetX && parent.y == targetY || openSquares.Count == 0)
                return;

            closedSquares.Push(parent);
            openSquares.Remove(parent);

            foreach (KeyValuePair<Point, int> adjacentSquareVar in adjacentSquares)
            {
                int additionalCost = adjacentSquareVar.Value;
                int adjacentSquareX = parent.x + adjacentSquareVar.Key.X;
                int adjacentSquareY = parent.y + adjacentSquareVar.Key.Y;
                Square adjacentSquare = new Square(adjacentSquareX, adjacentSquareY, parent.G + 10 + additionalCost, CalculateH(adjacentSquareX, adjacentSquareY, targetX, targetY), parent);

                if (!closedSquares.Contains(adjacentSquare) && OstBot.room.getMapBlock(0, adjacentSquare.x, adjacentSquare.y, 0).blockId == 4)

                    if (!openSquares.Contains(adjacentSquare))
                    {
                        openSquares.Add(adjacentSquare);
                    }
                    else
                    {
                        int oldG = adjacentSquare.G;
                        int newG = parent.G + 10 + additionalCost;
                        if (newG < oldG)
                        {
                            adjacentSquare.parent = parent;
                            adjacentSquare.G = newG;
                        }
                    }
            }
            GetCloseSquareData(targetX, targetY);
        }

        public int CalculateH(int x1, int y1, int x2, int y2)
        {
            return 10 * (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }
    }

}
