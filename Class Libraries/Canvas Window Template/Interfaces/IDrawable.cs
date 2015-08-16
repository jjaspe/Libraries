using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Interfaces
{
    public interface IDrawable
    {
        void draw();
        int getId();
        double[] getPosition();
        void setPosition(IPoint newPosition);
        bool Visible
        {
            set;
        }
    }
}
