using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Interfaces
{
    public interface ICanvas
    {
        void setPerspective(double length, double width);
        int getWidth();
        int getHeight();
    }
}
