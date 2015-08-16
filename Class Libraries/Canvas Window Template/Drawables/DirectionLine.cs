using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template.Drawables
{
    public class DirectionLine:IDrawable
    {
        IPoint begin, end;
        float[] color;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public DirectionLine(IPoint _begin, IPoint _end)
        {
            begin = _begin;
            end = _end;
            color = Common.colorWhite;
        }
        public DirectionLine(IPoint _begin, IPoint _end,float[] _color)
        {
            begin = _begin;
            end = _end;
            color = _color;
        }
        public void draw()
        {
            if(Visible)
            Common.drawLine(begin,end,color);
        }

        public int getId()
        {
            return 0;
        }

        public double[] getPosition()
        {
            return begin.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            begin = newPosition;
        }
    }
}
