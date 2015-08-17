using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Drawables.Shapes;
using System.Drawing;

namespace Canvas_Window_Template.Drawables
{
    public class TileMap:IDrawable
    {
        public List<OpenGLTile> MyTiles { get;set; }
        Color color=Color.Green;
        public IPoint MyOrigin { get; set; }

        public TileMap(IPoint origin,List<IPoint> tileOriginList,int tileSize,Color color=default(Color))
        {
            this.color = color;
            MyOrigin = origin;
            CreateTilesFromOrigins(tileOriginList,tileSize);
        }

        void CreateTilesFromOrigins(IList<IPoint> tileOriginsList,int tileSize)
        {
            MyTiles=new List<OpenGLTile>();
            foreach (IPoint point in tileOriginsList)
            {
                MyTiles.Add(createTileFromOrigin(point,tileSize));
            }
        }

        OpenGLTile createTileFromOrigin(IPoint point,int tileSize)
        {
            IPoint end=new OpenGLPoint(point.X+tileSize,point.Y+tileSize,0);
            OpenGLTile tile=new OpenGLTile(point,end,color,Color.Black);
            return tile;
        }

        public void RecolorMap(Color newColor)
        {
            color = newColor;
            foreach (OpenGLTile tile in MyTiles)
            {
                tile.MyColor = new float[]{newColor.R,newColor.G,newColor.B};
            }
        }

        public virtual void draw()
        {
            foreach (OpenGLTile tile in MyTiles)
            {
                tile.draw();
            }
        }

        public int getId()
        {
            return 0;
        }

        public double[] getPosition()
        {
            return new double[]{MyOrigin.X,MyOrigin.Y,MyOrigin.Z};
        }

        public void setPosition(IPoint newPosition)
        {
            MyOrigin = newPosition;
        }

        public bool Visible { get; set; }
    }
}
