using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Drawables.Shapes;


namespace Canvas_Window_Template.Drawables
{
    public class HighBlock:IDrawable,IOcluding
    {
        static int highBlockIds = 2;
        public const int idType = 2;
        public int myId;
        double height;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public double Height
        {
            get { return height; }
            private set { height = value; }
        }
        private OpenGLCube[] myCubes;
        IPoint myOrigin;

        public IPoint MyOrigin
        {
            get { return myCubes[0].MyOrigin; }
        }

        public HighBlock()
        {
            assignId(); 
        }
        public HighBlock(IPoint or, int size, float[] color, float[] outlineColor)
        {    
            assignId();
            createBlock(or, size, color, outlineColor);
        }
        public HighBlock(int[] or, int size, float[] color = null, float[] outlineColor = null)
        {
            //Default checking
            color = color == null ? Common.colorRed : color;
            outlineColor = outlineColor == null ? Common.colorBlack : outlineColor;
            createBlock(new PointObj(or[0],or[1],or[2]),size,color,outlineColor);
            
            assignId();
        }
        public void assignId()
        {
            myId = highBlockIds;
            highBlockIds += GameObjects.objectTypes;
        }
        private void createBlock(IPoint or, int size, float[] color, float[] outlineColor)
        {
            IPoint topOr = new PointObj(or.X, or.Y, or.Z + size);
            //Create two cubes, one on top of the other
            myCubes = new OpenGLCube[2];
            myCubes[0] = new OpenGLCube(or, size, color, outlineColor);
            myCubes[1] = new OpenGLCube(topOr, size, color, outlineColor);
            myCubes[0].createCubeTiles();
            myCubes[1].createCubeTiles();

            myOrigin = myCubes[0].MyOrigin;
            height = myCubes[0].CubeSize * 2;
        }

        #region IDRAWABLE
        public void draw()
        {
            if(Visible)
                foreach(OpenGLCube cube in myCubes)
                    Common.drawCubeAndOutline(cube);
        }
        public int getId()
        {
            return myId;
        }
        public double[] getPosition()
        {
            return myCubes[0].MyOrigin.toArray();
        }
        public void setPosition(IPoint newPosition)
        {
            createBlock(newPosition,myCubes[0].CubeSize,myCubes[0].Color,myCubes[0].OutlineColor);
        }
        #endregion

        public bool Intercepts(IPoint src, IPoint dest)
        {
            foreach (OpenGLCube cube in myCubes)
            {
                if (cube.Intercepts(src, dest))
                    return true;
            }
            return false;
        }
        public void turn45()
        {
            foreach (OpenGLCube cube in myCubes)
                cube.turn45();
        }
        public void turn45(IPoint axis)
        {
            foreach (OpenGLCube cube in myCubes)
            {
                cube.RotationAxis = axis;
                cube.turn45();
            }
        }

        public IPoint getLocation()
        {
            return myOrigin;
        }
    }
}
