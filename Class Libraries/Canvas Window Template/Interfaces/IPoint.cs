using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Interfaces
{
    public interface IPoint
    {
        double X
        {
            get;
            set;
        }
        double Y
        {
            get;
            set;
        }
        double Z
        {
            get;
            set;
        }
        IPoint copy();
        bool equals(IPoint p);
        bool equals(double[] p);
        double[] toArray();
    }
}
