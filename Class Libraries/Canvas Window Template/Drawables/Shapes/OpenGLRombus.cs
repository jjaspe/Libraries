using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class OpenGLRombus : IDrawable
    {
        static int rhombusObjIds = 1;
        public const int idType = 1;
        public int myId;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint origin, end;

        public IPoint MyEnd
        {
            get { return end; }
            set { end = value.copy(); }
        }

        public IPoint MyOrigin
        {
            get { return origin; }
            set { origin = value.copy(); }
        }
        private int rombusSize;

        public int RombusSize
        {
            get { return rombusSize; }
            set { rombusSize = value; }
        }
        private float[] myColor;

        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        private float[] myOutlineColor;

        public float[] MyOutlineColor
        {

            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }

        public OpenGLRombus(IPoint origin, IPoint end, float[] color, float[] outlineColor)
        {
            myId = rhombusObjIds++;
            MyOrigin = origin;
            MyEnd = end;
            myColor = color;
            myOutlineColor = outlineColor;
        }
        public OpenGLRombus()
        {
            myId = rhombusObjIds++;
            myColor = new float[] { 0.0f, 0.0f, 0.0f };
            myOutlineColor = new float[] { 0.0f, 0.0f, 0.0f };
        }
        public OpenGLRombus(OpenGLTile tile)
        {
            myId = rhombusObjIds++;
            MyOrigin = tile.MyOrigin;
            MyEnd = tile.MyEnd;
            myColor = tile.MyColor;
            myOutlineColor = tile.MyOutlineColor;
        }
        public void setColor(float[] color)
        {
            myColor = color;
        }
        public float[] getColor()
        {
            return myColor;
        }
        public void setOutlineColor(float[] color)
        {
            myOutlineColor = color;
        }
        public float[] getOutlineColor()
        {
            return myOutlineColor;
        }

        public OpenGLRombus copy()
        {
            return new OpenGLRombus(origin.copy(), end.copy(), MyColor, MyOutlineColor);
        }

        public void draw()
        {
            if (Visible)
                Common.drawRombusAndOutline(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[] { origin.X, origin.Y, origin.Z };
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }
    }
}
