using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public class OpenGLWall : IDrawable
    {
        protected OpenGLTile[,] myTiles;
        int myHeight, myWidth, tileSize;
        Common.planeOrientation orientation;
        public float[] defaultColor, defaultOutlineColor;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        public int TileSize
        {
            get { return tileSize; }
            set { tileSize = value; }
        }
        public OpenGLTile[,] MyTiles
        {
            get { return myTiles; }
            set { myTiles = value; }
        }
        public Common.planeOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        public int MyHeight
        {
            get { return myHeight; }
            set { myHeight = value; }
        }
        public int MyWidth
        {
            get { return myWidth; }
            set { myWidth = value; }
        }
        protected IPoint origin;

        public IPoint MyOrigin
        {
            get { return origin; }
            set { origin = value.copy(); }
        }

        public OpenGLWall()
        {
            myTiles = new OpenGLTile[0, 0];
        }
        /* Wall starts at "origin", wallHeight and wallWidth is how many tiles it has in each dimension,
         * tileSize their size, orientation is 1 for wall perpendicular to X axis, 2 for Y axis, 3 for Z axis,
         * and color and outlineColor the tiles' colors*/
        public OpenGLWall(IPoint origin, int wallHeight, int wallWidth,
            int _tileSize, int _orientation, float[] color, float[] outlineColor)
        {
            myHeight = wallHeight;
            myWidth = wallWidth;
            MyOrigin = origin;
            tileSize = _tileSize;
            orientation = (Common.planeOrientation)_orientation;
            myTiles = new OpenGLTile[wallHeight, wallWidth];
            defaultColor = color;
            defaultOutlineColor = outlineColor;

            fillWall();
        }
        /* Wall starts at "origin", wallHeight and wallWidth is how many tiles it has in each dimension,
        * tileSize their size, orientation orientation of perpendicular line,
        * and color and outlineColor the tiles' colors*/
        public OpenGLWall(IPoint origin, int wallHeight, int wallWidth,
            int _tileSize, Common.planeOrientation _orientation, float[] color, float[] outlineColor)
        {
            myHeight = wallHeight;
            myWidth = wallWidth;
            MyOrigin = origin;
            tileSize = _tileSize;
            this.orientation = _orientation;
            myTiles = new OpenGLTile[wallHeight, wallWidth];
            defaultColor = color;
            defaultOutlineColor = outlineColor;

            fillWall();
        }

        public virtual void fillWall()
        {
            myTiles = createTiles();
        }
        public bool Intercepts(IPoint src, IPoint dest)
        {
            foreach (OpenGLTile tile in myTiles)
            {
                if (tile.Intercepts(src, dest))
                    return true;
            }
            return false;
        }
        public OpenGLTile[,] createTiles()
        {
            OpenGLTile[,] newTiles = new OpenGLTile[myWidth, myHeight];
            IPoint currentTileOrigin = origin.copy(), currentTileEnd;
            #region CREATE_TILES_FOR_WALL
            switch (orientation)
            {
                case Common.planeOrientation.X: //Perpendicular to X axis
                    {
                        currentTileEnd = new pointObj(currentTileOrigin.X,
                            currentTileOrigin.Y + tileSize, currentTileOrigin.Z + tileSize);
                        for (int i = 0; i < myHeight; i++)
                        {

                            for (int j = 0; j < myWidth; j++)
                            {
                                currentTileOrigin.Z = currentTileOrigin.Z + tileSize;
                                currentTileEnd.Z = currentTileEnd.Z + tileSize;
                                newTiles[i, j] = new OpenGLTile(currentTileOrigin, currentTileEnd, defaultColor, defaultOutlineColor);
                            }
                            currentTileOrigin.Y = currentTileOrigin.Y + tileSize;
                            currentTileEnd.Y = currentTileEnd.Y + tileSize;

                            currentTileOrigin.Z = origin.Z;
                            currentTileEnd.Z = currentTileOrigin.Z + tileSize;
                        }
                        break;
                    }
                case Common.planeOrientation.Y: //Perpendicular to Y axis
                    {
                        currentTileEnd = new pointObj(currentTileOrigin.X + tileSize,
                            currentTileOrigin.Y, currentTileOrigin.Z + tileSize);
                        for (int i = 0; i < myHeight; i++)
                        {
                            for (int j = 0; j < myWidth; j++)
                            {
                                currentTileOrigin.X = currentTileOrigin.X + tileSize;
                                currentTileEnd.X = currentTileEnd.X + tileSize;
                                newTiles[i, j] = new OpenGLTile(currentTileOrigin, currentTileEnd, defaultColor, defaultOutlineColor);
                            }
                            currentTileOrigin.Z = currentTileOrigin.Z + tileSize;
                            currentTileEnd.Z = currentTileEnd.Z + tileSize;

                            currentTileOrigin.X = origin.X;
                            currentTileEnd.X = currentTileOrigin.X + tileSize;
                        }
                        break;
                    }
                case Common.planeOrientation.Z: //Perpendicular to Z axis
                    {
                        currentTileEnd = new pointObj(currentTileOrigin.X + tileSize,
                            currentTileOrigin.Y + tileSize, currentTileOrigin.Z);
                        for (int i = 0; i < myWidth; i++)
                        {
                            for (int j = 0; j < myHeight; j++)
                            {
                                newTiles[i, j] = new OpenGLTile(currentTileOrigin, currentTileEnd, defaultColor, defaultOutlineColor);
                                currentTileOrigin.X = currentTileOrigin.X + tileSize;
                                currentTileEnd.X = currentTileEnd.X + tileSize;
                            }
                            currentTileOrigin.Y = currentTileOrigin.Y + tileSize;
                            currentTileEnd.Y = currentTileEnd.Y + tileSize;

                            currentTileOrigin.X = origin.X;
                            currentTileEnd.X = currentTileOrigin.X + tileSize;
                        }
                        break;
                    }
            }
            #endregion
            return newTiles;
        }

        public void setTileColor(int indexI, int indexJ, float[] color, float[] outlineColor)
        {
            if (myWidth > indexI && myHeight > indexJ)
            {
                myTiles[indexI, indexJ].MyColor = color;
                myTiles[indexI, indexJ].MyOutlineColor = outlineColor;
            }
        }
        void recolorWall(float[] color)
        {
            for (int i = 0; i < MyHeight; i++)
            {
                for (int j = 0; j < this.MyWidth; j++)
                {
                    MyTiles[i, j].SetColor(color);
                }
            }
        }

        public void draw()
        {
            if (Visible)
                Common.staticDrawWall(this);
        }

        public int getId()
        {
            throw new NotImplementedException();
        }

        public double[] getPosition()
        {
            return MyOrigin.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }
    }
}
