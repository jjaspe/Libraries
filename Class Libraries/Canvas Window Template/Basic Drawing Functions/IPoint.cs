using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    public interface IPoint
    {
        int X
        {
            get;
            set;
        }
        int Y
        {
            get;
            set;
        }
        int Z
        {
            get;
            set;
        }
        IPoint copy();
    }
}
