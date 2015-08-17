using Canvas_Window_Template.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canvas_Window_Template.Drawables.Shapes
{
    public abstract class OpenGLShape : IDrawable
    {
        protected int id;

        public int Tag { get; set; }

        protected IPoint origin;

        public abstract void draw();

        public virtual int getId()
        {
            return id;
        }

        public virtual void setId(int id)
        {
            this.id = id;
        }

        public virtual double[] getPosition()
        {
            return new double[] { origin.X, origin.Y, origin.Z };
        }

        public virtual void setPosition(IPoint newPosition)
        {
            origin = newPosition;
        }

        public bool Visible { get; set; }
    }
}
