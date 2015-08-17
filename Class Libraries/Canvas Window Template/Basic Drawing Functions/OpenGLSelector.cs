using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Interfaces;
using Tao.OpenGl;

namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    public class OpenGLSelector
    {
        //int selectedObjectId=-1;

        ICanvas canvas;

        public ICanvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public OpenGLSelector(ICanvas _canvas)
        {
            Canvas = _canvas;
        }


        #region  SELECTION
        /// <summary>
        /// Selects entity from world at location given by selectionLocation,
        /// and returns the id of the entity
        /// </summary>
        /// <param name="selectionLocation"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public int getSelectedObjectId(int[] selectionLocation, IWorld world)
        {
            int[] buffer = new int[512];
            int hits = 0;
            double[] position;

            Gl.glSelectBuffer(512, buffer);
            beginSelection(selectionLocation);

            foreach (IDrawable _obj in world.getEntities())
            {
                Gl.glLoadName(_obj.getId());
                Gl.glPushMatrix();
                position = _obj.getPosition();
                _obj.draw();
                Gl.glPopMatrix();
            }

            endSelection();

            hits = Gl.glRenderMode(Gl.GL_RENDER);

            //For now, return number with highest z value
            if (hits > 0)
            {
                int highest = buffer[1], highestId = buffer[3];
                for (int i = 0; i < hits; i++)
                {
                    if (buffer[4 * i + 1] > highest)
                    {
                        highest = buffer[4 * i + 1];
                        highestId = buffer[4 * i + 3];
                    }
                }
                return highestId;
            }
            else
                return -1;
        }
        void beginSelection(int[] location)
        {
            int[] viewport = new int[4];
            double aspectRatio = (double)canvas.getWidth() / canvas.getHeight();
            double curLY = 0.2f, curLX = curLY * aspectRatio;

            Gl.glRenderMode(Gl.GL_SELECT);

            Gl.glInitNames();
            Gl.glPushName(0);

            //Save current state matrix
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();

            //Set selection view to entire screen
            Gl.glViewport(0, 0, canvas.getWidth(), canvas.getHeight());
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            //Create picking matrix
            Glu.gluPickMatrix(location[0], canvas.getHeight() - location[1],
                curLX, curLY, viewport);
            canvas.setPerspective(curLX, curLY);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
        }
        void endSelection()
        {
            //Restore matrices
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPopMatrix();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glFlush();
        }
        public void processHits(int hits, int[] buffer)
        {
            int i, j;
            int names;
            int index = 0;

            Console.Write("hits = " + hits + "\n");
            for (i = 0; i < hits; i++)
            { /*  for each hit  */
                names = buffer[index]; index++;
                Console.Write("number of names for hit = {0} \n", names);
                Console.Write("  z1 is {0};", (double)buffer[index] / 0x7fffffff); index++;
                Console.Write(" z2 is {0}\n", (double)buffer[index] / 0x7fffffff); index++;
                Console.Write("   the name is ");
                for (j = 0; j < names; j++)
                {     /*  for each name */
                    Console.Write("{0} ", buffer[index]);
                    index++;
                }
                Console.Write("\n");
            }
        }
        #endregion

    }
}
