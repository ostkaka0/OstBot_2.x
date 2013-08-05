using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstBot_2_
{
    public class Monster
    {
        private int xPos_ = 0;
        private int yPos_ = 0;
        private int xOldPos_ = 0;
        private int yOldPos_ = 0;
        private int xBlock_ = 0;
        private int yBlock_ = 0;
        public Monster(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        public int xPos { get { return xPos_; } set { xOldPos_ = xPos_; xPos_ = value; } }
        public int yPos { get { return yPos_; } set { yOldPos_ = yPos_; yPos_ = value; } }
        public int xBlock { get { return xPos_ / 16; } set { xPos_ = value * 16; xBlock_ = value; } }
        public int yBlock { get { return yPos_ / 16; } set { yPos_ = value * 16; yBlock_ = value; } }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }

}
