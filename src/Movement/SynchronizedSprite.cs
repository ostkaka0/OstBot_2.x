using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OstBot_2_
{
    //import SynchronizedSprite.*;
    //import flash.display.*;
    //import flash.geom.*;

    public class SynchronizedSprite : SynchronizedObject
    {
        protected Rectangle rect;
        protected int size;
        
        //protected var bmd:BitmapData;

        public SynchronizedSprite(int param2 = 0)
        {
            //this.bmd = param1;
            this.size = param2;
            width = param2;
            height = this.size;
            return;
        }// end function

        public double frame
        {
            get
            {
                return this.rect.X / this.size;
            }
            set
            {
                this.rect.X = (int)(value * this.size);
            }
        }// end function

        public bool hitTest(int param1, int param2)
        {
            return (param1 >= x && param2 >= y && param1 <= x + 16 && param2 <= y + 16);
        }// end function

        /*override public void draw(param1:BitmapData, param2:int, param3:int) : void
        {
            param1.copyPixels(this.bmd, this.rect, new Point(x + param2, y + param3));
            return;
        }// end function*/

    }
}
