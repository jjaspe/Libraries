using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Basic_Drawing_Functions;using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Drawables.Shapes;


namespace Canvas_Window_Template.Drawables
{
    public class LowBlock:OpenGLCube
    {
        static int lowBlockIds = 1;

        public LowBlock()
        {
            assignId();
        }
        public LowBlock(IPoint or, int size, float[] color=null, float[] outlineColor=null)
        {
            //Default checking
            color=color==null?OpenGLDrawer.colorRed:color;
            outlineColor = outlineColor == null ? OpenGLDrawer.colorBlack : outlineColor;
            this.MyOrigin = or;
            this.OutlineColor = outlineColor;
            this.CubeSize = size;
            this.Color = color;
            assignId();

            this.createCubeTiles();
        }
        public LowBlock(int[] or, int size, float[] color = null, float[] outlineColor = null)
        {
            //Default checking
            color = color == null ? OpenGLDrawer.colorRed : color;
            outlineColor = outlineColor == null ? OpenGLDrawer.colorBlack : outlineColor;
            this.MyOrigin = new PointObj(or[0],or[1],or[2]);
            this.OutlineColor = outlineColor;
            this.CubeSize = size;
            this.Color = color;
            assignId();

            this.createCubeTiles();
        }

        private void assignId()
        {
            myId = lowBlockIds;
            lowBlockIds += GameObjects.objectTypes;
        }

        public new void draw()
        {
            OpenGLDrawer.drawCubeAndOutline(this);
        }

        public new double[] getPosition()
        { return new double[] { MyOrigin.X, MyOrigin.Y, MyOrigin.Z }; }
        public new void setPosition(IPoint newPosition)
        {
            MyOrigin = newPosition;
            this.createCubeTiles();
        }

        public void turn45(IPoint IPoint)
        {
            this.turn45();
        }
    }
}
