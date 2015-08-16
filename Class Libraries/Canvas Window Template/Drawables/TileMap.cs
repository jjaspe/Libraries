using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Basic_Drawing_Functions;

namespace Canvas_Window_Template.Drawables
{
    public class TileMap:wallObj
    {
        public TileMap(IPoint origin,int length,int width,Common.planeOrientation orientation
            , int tileSize,float[] color,float[] outlineColor)
            :base(origin,length,width,tileSize,orientation,color,outlineColor)
        {

        }
    }
}
