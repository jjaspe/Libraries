using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Drawables.Shapes;

namespace Canvas_Window_Template.Drawables
{
    public class LowWall:OpenGLWall,IDrawable
    {
        static int lWallIds = 3;
        public const int idType = 3;
        public int myId;

        public LowWall()
        {
            assignId();
        }
        public LowWall(double altitude, double startX, double endX, int tileSize)
        {
            assignId();
            //Create tiles
            myTiles = new OpenGLTile[1,(int)(endX - startX)/tileSize];
            for (int i = 0; i < (endX-startX)/tileSize; i++)
            {
                myTiles[0,i] = new OpenGLTile(new PointObj(startX + i * tileSize, altitude, 0),
                    new PointObj(startX + (i + 1) * tileSize, altitude, tileSize), Common.colorBrown, Common.colorBlack);
            }
            MyOrigin = myTiles[0, 0].MyOrigin;
            TileSize = tileSize;
            Orientation = Common.planeOrientation.Y;
        }

        private void assignId()
        {
            myId = lWallIds;
            lWallIds += GameObjects.objectTypes;
        }

        public void turn()
        {
            if (MyOrigin == null || MyTiles == null || MyTiles.Length == 0)
                return;
            double startY = MyOrigin.Y, endY = MyOrigin.Y + MyTiles.Length * TileSize,latitude=MyOrigin.X;
            for (int i = 0; i < (endY - startY) / TileSize; i++)
            {
                myTiles[0, i] = new OpenGLTile(new PointObj(latitude, startY + i * TileSize, 0),
                    new PointObj(latitude, startY + (i + 1) * TileSize, TileSize), Common.colorBrown, Common.colorBlack);
            }
            Orientation = Common.planeOrientation.X;
        }

        public new void draw()
        {
            foreach (OpenGLTile tile in myTiles)
                Common.drawTileAndOutline(tile);
        }

        public new double[] getPosition()
        {
            if (myTiles != null)
                return myTiles[0,0].MyOrigin.toArray();
            else
                return null;
        }

        public new void setPosition(IPoint newPosition)
        {
            return;
        }
    }
}
