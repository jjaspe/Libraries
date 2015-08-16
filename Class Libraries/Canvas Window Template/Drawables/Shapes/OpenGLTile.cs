using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class OpenGLTile : OpenGLShape
    {
        public IPoint end;
        Common.planeOrientation Orientation;
        private float[] myColor;
        private float[] myOutlineColor;
        public int angle = 0;


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
        public double TileSize { get; set; }

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

        public OpenGLTile(IPoint origin, IPoint end, float[] color, float[] outlineColor)
        {
            MyOrigin = origin;
            MyEnd = end;
            myColor = color;
            myOutlineColor = outlineColor;
            setOrientation();
            setTileSize();
        }
        public OpenGLTile()
        {
            myColor = new float[] { 0.0f, 0.0f, 0.0f };
            myOutlineColor = new float[] { 0.0f, 0.0f, 0.0f };
        }
        public void SetColor(float[] color)
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
        public void setOrientation()
        {
            if (MyOrigin.X == MyEnd.X)
                Orientation = Common.planeOrientation.X;
            else if (MyOrigin.Y == MyEnd.Y)
                Orientation = Common.planeOrientation.Y;
            else if (MyOrigin.Z == MyEnd.Z)
                Orientation = Common.planeOrientation.Z;
            else
                Orientation = Common.planeOrientation.None;
        }
        public void setTileSize()
        {
            switch (Orientation)
            {
                case Common.planeOrientation.X:
                    TileSize = MyEnd.Z - MyOrigin.Z;
                    break;
                case Common.planeOrientation.Y:
                    TileSize = MyEnd.Z - MyOrigin.Z;
                    break;
                case Common.planeOrientation.Z:
                    TileSize = MyEnd.X - MyOrigin.X;
                    break;
                default:
                    break;
            }
        }

        public void turn45()
        {
            angle++;
        }

        public bool Intercepts(IPoint src, IPoint dest)
        {
            //Assume we dont want tiles perp to Z
            double zStart = Math.Min(MyOrigin.Z, MyEnd.Z), zEnd = Math.Max(MyOrigin.Z, MyEnd.Z);
            double lineMinY = Math.Min(src.Y, dest.Y), lineMaxY = Math.Max(src.Y, dest.Y),
                lineMinX = Math.Min(src.X, dest.X), lineMaxX = Math.Max(src.X, dest.X);
            if (src.Z < zStart || src.Z > zEnd)
                return false;
            //Slope of line 
            double slope = 0;
            bool noSlope = false;
            if (src.X != dest.X)
                slope = (src.Y - dest.Y) / (src.X - dest.X);
            else
                noSlope = true;
            double spot, start, end;
            double equationResult;
            //Get checking point
            setOrientation();
            switch (Orientation)
            {
                case Common.planeOrientation.X://Vertical, so get Y positions
                    if (!noSlope)//Vertical lines wont intercept vertical tiles
                    {
                        spot = origin.X;
                        if (lineMaxX < spot || lineMinX > spot)//endpoints too high or too low
                            return false;
                        start = Math.Min(MyOrigin.Y, MyEnd.Y);
                        end = Math.Max(MyOrigin.Y, MyEnd.Y);
                        equationResult = slope * (spot - src.X) + src.Y;
                        return (start <= equationResult && equationResult <= end);
                    }
                    else
                        return false;
                case Common.planeOrientation.Y://Horizontal, so get X positions
                    spot = origin.Y;
                    if (lineMaxY < spot || lineMinY > spot)//endpoints too high or too low
                        return false;
                    start = Math.Min(MyOrigin.X, MyEnd.X);
                    end = Math.Max(MyOrigin.X, MyEnd.X);
                    if (noSlope)
                        equationResult = src.X;//Same X for all points
                    else
                        equationResult = (spot - src.Y) / slope + src.X;
                    return (start <= equationResult && equationResult <= end);
                default:
                    return false;
            }
        }

        public OpenGLTile copy()
        {
            return new OpenGLTile(origin.copy(), end.copy(), MyColor, MyOutlineColor);
        }

        public override void draw()
        {
            Common.drawTileAndOutline(this);
        }

    }
}
