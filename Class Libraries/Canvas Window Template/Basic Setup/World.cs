using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template.Basic_Setup
{
    class World:IWorld
    {
        List<IDrawable> myDrawables = new List<IDrawable>();
        int index = 0;
        public List<IDrawable> getEntities()
        {
            return myDrawables;
        }

        public IDrawable getNext()
        {
            return myDrawables[index++ % myDrawables.Count];
        }

        public void add(IDrawable d)
        {
            myDrawables.Add(d);
        }

        public IDrawable remove(IDrawable d)
        {
            bool removed=myDrawables.Remove(d);
            if (index >= myDrawables.Count)
                index = 0;
            return removed?d:null;
        }
    }
}
