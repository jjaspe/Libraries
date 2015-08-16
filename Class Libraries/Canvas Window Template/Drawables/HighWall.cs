using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Drawables.Shapes;

namespace Canvas_Window_Template.Drawables
{
    public class HighWall:OpenGLWall,IDrawable,IOcluding
    {
        static int hWallIds = 4;
        public const int idType = 4;
        public int myId;

        public HighWall()
        {
            assignId();
        }
        public HighWall(double altitude, double startX, double endX, int tileSize)
        {
            assignId();
            IPoint or = new PointObj(startX, altitude, 0);
            IPoint topOr = new PointObj(or.X, or.Y, or.Z + tileSize);
            //Create tiles
            myTiles = new OpenGLTile[2,(int)(endX - startX)/tileSize];
            for (int i = 0; i < (endX-startX)/tileSize; i++)
            {
                myTiles[0,i] = new OpenGLTile(new PointObj(or.X+ i * tileSize, or.Y, or.Z),
                    new PointObj(or.X + (i + 1) * tileSize, or.Y, or.Z+tileSize), Common.colorBrown, Common.colorBlack);
                myTiles[1, i] = new OpenGLTile(new PointObj(topOr.X + i * tileSize, topOr.Y, topOr.Z),
                   new PointObj(topOr.X + (i + 1) * tileSize, topOr.Y, topOr.Z + tileSize), Common.colorBrown, Common.colorBlack);

            }
            MyOrigin = myTiles[0, 0].MyOrigin;
            TileSize = tileSize;
            Orientation = Common.planeOrientation.Y;
        }

        private void assignId()
        {
            myId = hWallIds;
            hWallIds += GameObjects.objectTypes;
        }


        public new void draw()
        {
            foreach (OpenGLTile tile in myTiles)
                Common.drawTileAndOutline(tile);
        }

        public new double[] getPosition()
        {
            if (myTiles != null)
                return new double[] { myTiles[0,0].MyOrigin.X, myTiles[0,0].MyOrigin.Y, myTiles[0,0].MyOrigin.Z };
            else
                return null;
        }
        public new void setPosition(IPoint newPosition)
        {
            return;
        }

        public new bool Intercepts(IPoint src, IPoint dest)
        {
            foreach (OpenGLTile tile in MyTiles)
            {
                if (tile.Intercepts(src, dest))
                    return true;
            }
            return false;
        }
        public void turn()
        {
            if (MyOrigin == null || MyTiles == null || MyTiles.Length == 0)
                return;
            double startY = MyOrigin.Y, endY = MyOrigin.Y + MyTiles.Length/2 * TileSize;

            IPoint or = new PointObj(MyOrigin.X,startY, 0);
            IPoint topOr = new PointObj(or.X, or.Y, or.Z + TileSize);
            for (int i = 0; i < (endY - startY) / TileSize; i++)
            {
                myTiles[0, i] = new OpenGLTile(new PointObj(or.X, or.Y + i * TileSize, or.Z),
                    new PointObj(or.X , or.Y + (i + 1) * TileSize, or.Z + TileSize), Common.colorBrown, Common.colorBlack);
                myTiles[1, i] = new OpenGLTile(new PointObj(topOr.X , topOr.Y + i * TileSize , topOr.Z),
                   new PointObj(topOr.X, topOr.Y + (i + 1) * TileSize, topOr.Z + TileSize), Common.colorBrown, Common.colorBlack);

            }
            Orientation = Common.planeOrientation.X;
        }

        public IPoint getLocation()
        {
            return MyOrigin;
        }
    }
}
