using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class OpenGLCircle : IDrawable
    {
        static int circleObjId = 1;
        public int myId;
        public double[,] vertices = new double[360, 3];
        IPoint center;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint Center
        {
            get { return center; }
            set { center = value; }
        }
        double radius;
        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        float[] myColor;
        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        float[] outlineColor;

        public float[] OutlineColor
        {
            get { return outlineColor; }
            set { outlineColor = value; }
        }


        public OpenGLCircle(IPoint center, double radius, float[] color, float[] outlineColor)
        {
            Center = center;
            Radius = radius;
            MyColor = color;
            OutlineColor = outlineColor;
            myId = circleObjId++;
            for (int i = 0; i < vertices.Length / 3; i++)
            {
                vertices[i, 0] = Radius * Math.Cos(i * Math.PI / 180);
                vertices[i, 1] = Radius * Math.Sin(i * Math.PI / 180);
                vertices[i, 2] = 0;
            }
        }


        public void draw()
        {
            if (Visible)
                Common.drawCircle(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[] { center.X, center.Y, center.Z };
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }


    }
}
