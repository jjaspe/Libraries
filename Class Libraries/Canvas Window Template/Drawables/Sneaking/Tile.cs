using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Drawables.Shapes;

namespace Canvas_Window_Template.Drawables
{
    public class Tile : OpenGLTile
    {        
        static int tileIds = 0;
        public const int idType = 0;
        public int myId;

        /// <summary>
        /// 1 is regular color, >1 makes it darker
        /// </summary>
        protected int shade = 1;

        float[] originalColor;
        public float[] OriginalColor
        {
            get { return originalColor; }
            set { originalColor = value; }
        }

        public int Shaded
        {
            get { return shade; }
            set { shade = Math.Max(1, value); setShade(); }
        }

        public Tile(IPoint tileStart, IPoint tileEnd)
        {
            MyOrigin = tileStart;
            MyEnd = tileEnd;
            assignId();
        }
        public Tile(double[] start, int[] end)
        {
            MyOrigin = new PointObj(start[0], start[1], start[2]);
            MyEnd = new PointObj(end[0], end[1], end[2]);
            assignId();

        }
        public Tile(double[] start, int _tileSize, int orientation = 3)
        {
            MyOrigin = new PointObj(start[0], start[1], start[2]);
            MyEnd = new PointObj(start[0] + _tileSize, start[1] + _tileSize, start[2]);
            this.TileSize = _tileSize;
            assignId();
        }

        protected void assignId()
        {
            myId = tileIds;
            tileIds += GameObjects.objectTypes;
        }
        /* 0 for nothing, 1 for low block, 2 for high block, 3 for guard, 4 for pc*/
        public int myState;

        /* 0 for none, 1 for low wall, 2 for high wall */
        public int upWall, downWall, rightWall, leftWall;

        protected void setShade()
        {
            MyColor = new float[] { originalColor[0] / shade, originalColor[1] / shade, originalColor[2] / shade };
        }
        public void lighten(int level)
        {
            Shaded = Math.Max(1, shade - level);
        }

        public new void draw()
        {
            if(Visible)
            OpenGLDrawer.drawTileAndOutline(this);
            //Common.drawTile(this);
        }

        public new double[] getPosition()
        {
            return new double[] { MyOrigin.X, MyOrigin.Y, MyOrigin.Z };
        }
        public new void setPosition(IPoint newPosition)
        {
            MyOrigin.X = newPosition.X;
            MyOrigin.Y = newPosition.Y;
            MyOrigin.Z = newPosition.Z;
        }

        /*
        public double[] getCenter(OpenGlMap.tileSide side)
        {
            switch (side)
            {
                case OpenGlMap.tileSide.Top:
                    return new double[] { MyOrigin.X + TileSize / 2, MyOrigin.Y + TileSize };
                case OpenGlMap.tileSide.Right:
                    return new double[] { MyOrigin.X + TileSize, MyOrigin.Y + TileSize / 2 };
                case OpenGlMap.tileSide.Bottom:
                    return new double[] { MyOrigin.X + TileSize / 2, MyOrigin.Y };
                case OpenGlMap.tileSide.Left:
                    return new double[] { MyOrigin.X, MyOrigin.Y + TileSize / 2 };
                default:
                    return getCenter();
            }
        }*/
        public double[] getCenter(OpenGLDrawer.tileSide side)
        {
            switch (side)
            {
                case OpenGLDrawer.tileSide.Top:
                    return new double[] { MyOrigin.X + TileSize / 2, MyOrigin.Y + TileSize };
                case OpenGLDrawer.tileSide.Right:
                    return new double[] { MyOrigin.X + TileSize, MyOrigin.Y + TileSize / 2 };
                case OpenGLDrawer.tileSide.Bottom:
                    return new double[] { MyOrigin.X + TileSize / 2, MyOrigin.Y };
                case OpenGLDrawer.tileSide.Left:
                    return new double[] { MyOrigin.X, MyOrigin.Y + TileSize / 2 };
                default:
                    return getCenter();
            }
        }
        public double[] getCenter()
        {
            return new double[]{MyOrigin.X+TileSize/2,MyOrigin.Y+TileSize/2};
        }
        public double[] topLeft()
        {
            return new double[] { this.MyOrigin.X, this.MyOrigin.Y + TileSize };
        }
        public double[] topRight()
        {
            return new double[] { MyOrigin.X + TileSize, MyOrigin.Y + TileSize };
        }
        public double[] bottomLeft()
        {
            return new double[] { MyOrigin.X, MyOrigin.Y };
        }
        public double[] bottomRight()
        {
            return new double[] { MyOrigin.X + TileSize, MyOrigin.Y };
        }


        public static Tile[,] createTileArray(IPoint leftBottomCorner, int width, int height, OpenGLDrawer.planeOrientation orientation,
            float[] color, double tileSize)
        {
            Tile[,] newTiles = new Tile[width, height];
            IPoint currentTileOrigin = leftBottomCorner.copy(), currentTileEnd;
            #region CREATE_TILES_FOR_WALL
            switch (orientation)
            {
                case OpenGLDrawer.planeOrientation.X: //Perpendicular to X axis
                    {
                        currentTileEnd = new OpenGLPoint(currentTileOrigin.X,
                            currentTileOrigin.Y + tileSize, currentTileOrigin.Z + tileSize);
                        for (int i = 0; i < height; i++)
                        {

                            for (int j = 0; j < width; j++)
                            {
                                currentTileOrigin.Z = currentTileOrigin.Z + tileSize;
                                currentTileEnd.Z = currentTileEnd.Z + tileSize;
                                newTiles[i, j] = new Tile(currentTileOrigin, currentTileEnd) { MyColor = color, MyOutlineColor = OpenGLDrawer.colorBlack, TileSize = tileSize };
                            }
                            currentTileOrigin.Y = currentTileOrigin.Y + tileSize;
                            currentTileEnd.Y = currentTileEnd.Y + tileSize;

                            currentTileOrigin.Z = leftBottomCorner.Z;
                            currentTileEnd.Z = currentTileOrigin.Z + tileSize;
                        }
                        break;
                    }
                case OpenGLDrawer.planeOrientation.Y: //Perpendicular to Y axis
                    {
                        currentTileEnd = new OpenGLPoint(currentTileOrigin.X + tileSize,
                            currentTileOrigin.Y, currentTileOrigin.Z + tileSize);
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                currentTileOrigin.X = currentTileOrigin.X + tileSize;
                                currentTileEnd.X = currentTileEnd.X + tileSize;
                                newTiles[i, j] = new Tile(currentTileOrigin, currentTileEnd) { MyColor = color, MyOutlineColor = OpenGLDrawer.colorBlack, TileSize = tileSize };
                            }
                            currentTileOrigin.Z = currentTileOrigin.Z + tileSize;
                            currentTileEnd.Z = currentTileEnd.Z + tileSize;

                            currentTileOrigin.X = leftBottomCorner.X;
                            currentTileEnd.X = currentTileOrigin.X + tileSize;
                        }
                        break;
                    }
                case OpenGLDrawer.planeOrientation.Z: //Perpendicular to Z axis
                    {
                        currentTileEnd = new OpenGLPoint(currentTileOrigin.X + tileSize,
                            currentTileOrigin.Y + tileSize, currentTileOrigin.Z);
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < height; j++)
                            {
                                newTiles[i, j] = new Tile(currentTileOrigin, currentTileEnd) { MyColor = color, MyOutlineColor = OpenGLDrawer.colorBlack, TileSize = tileSize };
                                currentTileOrigin.X = currentTileOrigin.X + tileSize;
                                currentTileEnd.X = currentTileEnd.X + tileSize;
                            }
                            currentTileOrigin.Y = currentTileOrigin.Y + tileSize;
                            currentTileEnd.Y = currentTileEnd.Y + tileSize;

                            currentTileOrigin.X = leftBottomCorner.X;
                            currentTileEnd.X = currentTileOrigin.X + tileSize;
                        }
                        break;
                    }
            }
            #endregion
            return newTiles;
        }
    }
}
