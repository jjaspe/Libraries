using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class OpenGLCube : IDrawable
    {
        static int cubeObjIds = 1;
        public const int idType = 1;
        public int myId;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        int cubeSize;
        int angle = 0;

        /// <summary>
        /// As Multiples of 45 Degrees. So 3 means 135 Degrees
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        IPoint origin;
        IPoint rotationAxis;
        internal Stack<Rotation> Rotations;

        public IPoint RotationAxis
        {
            get { return rotationAxis; }
            set { rotationAxis = value; }
        }
        float[] myColor;
        float[] myOutlineColor;

        public float[] OutlineColor
        {
            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }
        OpenGLTile tileFront, tileBack, tileLeft, tileRight, tileTop, tileBottom;

        public IPoint MyOrigin
        {
            get { return origin.copy(); }
            set { origin = value.copy(); }
        }

        public OpenGLTile TileBottom
        {
            get { return tileBottom; }
            set { tileBottom = value; }
        }
        public OpenGLTile TileTop
        {
            get { return tileTop; }
            set { tileTop = value; }
        }
        public OpenGLTile TileRight
        {
            get { return tileRight; }
            set { tileRight = value; }
        }
        public OpenGLTile TileLeft
        {
            get { return tileLeft; }
            set { tileLeft = value; }
        }
        public OpenGLTile TileBack
        {
            get { return tileBack; }
            set { tileBack = value; }
        }
        public OpenGLTile TileFront
        {
            get { return tileFront; }
            set { tileFront = value; }
        }

        public float[] Color
        {
            get { return myColor; }
            set { myColor = value; }
        }
        public int CubeSize
        {
            get { return cubeSize; }
            set { cubeSize = value; }
        }
        public void turn45()
        {
            //TileFront.turn45();
            //TileBack.turn45();
            //TileLeft.turn45();
            //TileRight.turn45();
            //TileTop.turn45();
            //TileBottom.turn45();
            angle++;
        }

        public OpenGLCube()
        {
            myId = cubeObjIds++;
            Rotations = new Stack<Rotation>();
        }
        public OpenGLCube(IPoint origin, int cubeSize, float[] color, float[] outlineColor)
        {
            myId = cubeObjIds++;
            this.origin = origin;
            this.cubeSize = cubeSize;
            this.myColor = color;
            this.myOutlineColor = outlineColor;
            createCubeTiles();
            rotationAxis = new OpenGLPoint(origin.X + cubeSize / 2, origin.Y + cubeSize / 2, 0);
            Rotations = new Stack<Rotation>();
        }
        public void createCubeTiles()
        {
            rotationAxis = new OpenGLPoint(origin.X + cubeSize / 2, origin.Y + cubeSize / 2, 0);
            tileFront = new OpenGLTile(origin,
                new OpenGLPoint(origin.X + cubeSize, origin.Y, origin.Z + cubeSize),
                Color, OutlineColor);
            tileRight = new OpenGLTile(new OpenGLPoint(origin.X + cubeSize, origin.Y, origin.Z),
                new OpenGLPoint(origin.X + cubeSize, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
            tileLeft = new OpenGLTile(origin,
                new OpenGLPoint(origin.X, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
            tileBack = new OpenGLTile(new OpenGLPoint(origin.X, origin.Y + cubeSize, origin.Z),
                new OpenGLPoint(origin.X + cubeSize, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
            tileBottom = new OpenGLTile(origin,
                new OpenGLPoint(origin.X + cubeSize, origin.Y + cubeSize, origin.Z),
                Color, OutlineColor);
            tileTop = new OpenGLTile(new OpenGLPoint(origin.X, origin.Y, origin.Z + cubeSize),
                new OpenGLPoint(origin.X + cubeSize, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
        }

        public bool Intercepts(IPoint src, IPoint dest)
        {
            return (tileFront.Intercepts(src, dest) || tileBack.Intercepts(src, dest) ||
                tileLeft.Intercepts(src, dest) || tileRight.Intercepts(src, dest)
                || tileBottom.Intercepts(src, dest) || tileTop.Intercepts(src, dest));
        }

        public void draw()
        {
            if (Visible)
                OpenGLDrawer.drawCubeAndOutline(this);
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

        public void Rotate(OpenGLLine line, double degrees)
        {
            Rotations.Push(new Rotation() { Axis = line, Degrees = degrees });
        }
    }
}
