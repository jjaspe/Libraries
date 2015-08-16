using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Interfaces
{
    public interface IWorld
    {        
        List<IDrawable> getEntities();
        IDrawable getNext();
        void add(IDrawable d);
        IDrawable remove(IDrawable d);
    }
    
}
