using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class Rectangle : IDrawable
    {
        static int rectableObjIds = 1;
        public const int idType = 1;
        public int myId;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint bottomLeft, topLeft, bottomRight, topRight;
        Common.planeOrientation Orientation;

        public Common.planeOrientation Orientation1
        {
            get { return Orientation; }
        }
        private float[] myColor;
        private float[] myOutlineColor;

        public IPoint TopRight
        {
            get { return topRight; }
            set { topRight = value; }
        }

        public IPoint BottomRight
        {
            get { return bottomRight; }
            set { bottomRight = value; }
        }

        public IPoint TopLeft
        {
            get { return topLeft; }
            set { topLeft = value; }
        }

        public IPoint BottomLeft
        {
            get { return bottomLeft; }
            set { bottomLeft = value; }
        }

        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }

        public float[] MyOutlineColor
        {
            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }

        public void setOrientation()
        {
            if (BottomLeft.X == TopRight.X)
                Orientation = Common.planeOrientation.X;
            else if (BottomLeft.Y == TopRight.Y)
                Orientation = Common.planeOrientation.Y;
            else if (BottomLeft.Z == TopRight.Z)
                Orientation = Common.planeOrientation.Z;
            else
                Orientation = Common.planeOrientation.None;
        }

        public Rectangle(IPoint _bLeft, IPoint _tLeft, IPoint _bRight, IPoint _tRight
            , float[] color, float[] outlineColor)
        {
            BottomLeft = _bLeft;
            bottomRight = _bRight;
            TopLeft = _tLeft;
            TopRight = _tRight;
            myColor = color;
            myOutlineColor = outlineColor;
            setOrientation();
        }
        public Rectangle()
        {
            myColor = new float[] { 0.0f, 0.0f, 0.0f };
            myOutlineColor = new float[] { 0.0f, 0.0f, 0.0f };
            myId = rectableObjIds;
            rectableObjIds++;
        }

        public Rectangle copy()
        {
            return new Rectangle(BottomLeft.copy(), TopLeft.copy(), bottomRight.copy(), topRight.copy(),
                MyColor, MyOutlineColor);
        }

        public void draw()
        {
            if (Visible)
                Common.drawRectangleAndOutline(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[] { bottomLeft.X, bottomLeft.Y, bottomLeft.Z };
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }
    }
}
