using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class OpenGLParallepiped : IDrawable
    {
        IPoint origin, rotationAxis;
        int angle = 0;
        double xWidth, yWidth, zWidth;
        float[] color, outlineColor;
        Rectangle tileFront, tileBack, tileLeft, tileRight, tileTop, tileBottom;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        #region Properties
        public IPoint RotationAxis
        {
            get { return rotationAxis; }
            set { rotationAxis = value; createTiles(); }
        }
        public Rectangle TileBottom
        {
            get { return tileBottom; }
            set { tileBottom = value; }
        }
        public Rectangle TileTop
        {
            get { return tileTop; }
            set { tileTop = value; }
        }
        public Rectangle TileRight
        {
            get { return tileRight; }
            set { tileRight = value; }
        }
        public Rectangle TileLeft
        {
            get { return tileLeft; }
            set { tileLeft = value; }
        }
        public Rectangle TileBack
        {
            get { return tileBack; }
            set { tileBack = value; }
        }
        public Rectangle TileFront
        {
            get { return tileFront; }
            set { tileFront = value; }
        }
        public IPoint Origin
        {
            get { return origin; }
        }
        /// <summary>
        /// As multiple of 45 degrees.
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        public float[] OutlineColor
        {
            get { return outlineColor; }
            set { outlineColor = value; }
        }

        public float[] Color
        {
            get { return color; }
            set { color = value; }
        }

        public double ZWidth
        {
            get { return zWidth; }
            set { zWidth = value; createTiles(); }
        }

        public double YWidth
        {
            get { return yWidth; }
            set { yWidth = value; createTiles(); }
        }

        public double XWidth
        {
            get { return xWidth; }
            set { xWidth = value; createTiles(); }
        }
        #endregion

        public OpenGLParallepiped(IPoint origin, double xWidth, double yWidth, double zWidth, float[] color,
            float[] outlineColor = null)
        {
            this.origin = origin;
            this.xWidth = xWidth;
            this.yWidth = yWidth;
            this.zWidth = zWidth;
            rotationAxis = new pointObj(origin.X + xWidth / 2, origin.Y + yWidth / 2, 0);
            this.color = color;
            this.outlineColor = outlineColor ?? Common.colorBlack;
            createTiles();
        }

        private void createTiles()
        {
            IPoint b1 = origin,
                b2 = new pointObj(b1.X + xWidth, b1.Y, b1.Z),
                b3 = new pointObj(b1.X + xWidth, b1.Y + yWidth, b1.Z),
                b4 = new pointObj(b1.X, b1.Y + yWidth, b1.Z),
                t1 = new pointObj(b1.X, b1.Y, b1.Z + zWidth),
                t2 = new pointObj(b1.X + xWidth, b1.Y, b1.Z + zWidth),
                t3 = new pointObj(b1.X + xWidth, b1.Y + yWidth, b1.Z + zWidth),
                t4 = new pointObj(b1.X, b1.Y + yWidth, b1.Z + zWidth);

            tileBottom = new Rectangle(b1, b4, b2, b3,
                Color, OutlineColor);
            tileRight = new Rectangle(b2, t2, b3, t3,
                Color, OutlineColor);
            tileLeft = new Rectangle(b1, t1, b4, t4,
                Color, OutlineColor);
            tileTop = new Rectangle(t1, t4, t2, t3,
                Color, OutlineColor);
            tileFront = new Rectangle(b1, t1, b2, t2,
                Color, OutlineColor);
            tileBack = new Rectangle(b3, t3, b4, t4,
                Color, OutlineColor);
        }


        public void draw()
        {
            if (Visible)
                Common.drawParallepiped(this);
        }

        public int getId()
        {
            return 0;
        }

        public double[] getPosition()
        {
            return Origin.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            origin = newPosition;
            createTiles();
        }
    }
}
