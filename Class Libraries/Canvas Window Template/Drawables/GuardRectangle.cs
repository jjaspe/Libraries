using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;using Canvas_Window_Template.Interfaces;


namespace Canvas_Window_Template.Drawables
{
    public class GuardRectangle:IDrawable
    {
        rectangleObj myRectangle;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public rectangleObj MyRectangle
        {
            get { return myRectangle; }
            set { myRectangle = value; }
        }

        public void draw()
        {
            if(Visible)
            Common.drawRectangleAndOutline(myRectangle);
        }

        public int getId()
        {
            return 0;
        }

        public double[] getPosition()
        {
            return myRectangle.BottomLeft.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            MyRectangle.BottomLeft = newPosition;
        }
    }
}
