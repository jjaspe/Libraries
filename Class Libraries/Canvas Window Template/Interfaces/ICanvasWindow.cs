using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template.Interfaces
{
    public interface ICanvasWindow
    {
        simpleOpenGlView getView();
        //void setMap(IWorld newMap,int width, int height,int tileSize,IPoint origin,Common.planeOrientation orientation);
        IWorld getMap();
        void refresh();
    }
}
