using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{

    public class OpenGLLine : IDrawable
    {
        static int lineIds = 0;
        int myId;

        float[] myColor;
        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        IPoint p1, p2;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint P2
        {
            get { return p2; }
            set { p2 = value; }
        }
        public IPoint P1
        {
            get { return p1; }
            set { p1 = value; }
        }

        /// <summary>
        /// If no color is entered,defaults to red
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="color"></param>
        public OpenGLLine(IPoint p1, IPoint p2, float[] color = null)//defaulted to red
        {
            MyColor = color ?? Common.colorRed;
            P1 = p1;
            P2 = p2;

            myId = lineIds++;
        }

        public OpenGLLine()
        {
            myId = lineIds++;
        }

        public void draw()
        {
            if (Visible)
                Common.drawLine(P1, P2, MyColor);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return P2.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            P2 = newPosition;
        }
    }
}
