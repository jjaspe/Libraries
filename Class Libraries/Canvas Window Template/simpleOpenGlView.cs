
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;
using System.Windows.Forms;

namespace Canvas_Window_Template
{
    public class simpleOpenGlView : Tao.Platform.Windows.SimpleOpenGlControl
    {
        #region SET UP VARIABLES
        static bool is_depth = true;
        private static bool[] keys = new bool[256];                             // Array Used For The Keyboard Routine
        int Errors = 0;
        private double[] perspectiveEye;
        public double[] eyeFront;
        public double[] eyeCorner;
        public double[] eyeTop;
        public double[] eyeIso;
        public double[] eyeSide;
        public double[] eyeDefault = new double[] { 220.0d, 420.0d, 420.0d };
        double zoomFactor;

        public double ZoomFactor
        {
            get { return zoomFactor; }
            set { zoomFactor = value; }
        }
        double viewDistance = 600.0d;
        double eyeY, eyeX;
        double incrementL = 10, incrementX = 0, incrementY = 0, incrementZY = 0, incrementZX = 0;

        public double IncrementZX
        {
            get { return incrementZX; }
            set { incrementZX = value; }
        }

        public double IncrementZY
        {
            get { return incrementZY; }
            set { incrementZY = value; }
        }

        public double IncrementY
        {
            get { return incrementY; }
            set { incrementY = value; }
        }

        public double IncrementX
        {
            get { return incrementX; }
            set { incrementX = value; }
        }

        public double Increment
        {
            get { return incrementL; }
            set { incrementL = value; }
        }

        public double EyeX
        {
            get { return eyeX; }
            set { eyeX = value; }
        }

        public double EyeY
        {
            get { return eyeY; }
            set { eyeY = value; }
        }

        public double ViewDistance
        {
            get { return viewDistance; }
            set { viewDistance = value; }
        }

        public double[] PerspectiveEye
        {
            get { return perspectiveEye; }
            set { perspectiveEye = value; }
        }


        //COLORS
        static float[] colorBlack = { 0.0f, 0.0f, 0.0f }, colorWhite = { 1.0f, 1.0f, 1.0f }, colorBlue = { 0.0f, 0.0f, 1.0f };
        static float[] colorRed = { 1.0f, 0.0f, 0.0f }, colorOrange = { 1.0f, 0.5f, 0.25f }, colorYellow = { 1.0f, 1.0f, 0.0f };
        static float[] colorGreen = { 0.0f, 1.0f, 0.2f }, colorBrown = { 0.4f, 0.5f, 0.3f };

        #endregion

        #region START UP VARIABLES
        static int clickX, clickY;             //Coordinates of mouse pointer at last click
        #endregion

        public simpleOpenGlView()
        {
            perspectiveEye = eyeDefault;
            zoomFactor = 1.05;
            calculateCameraViews();
        }

        public bool isDisposed()
        {
            return base.IsDisposed;
        }
        // This function sets up the OpenGl context as normal, but note the Gl prefix's!
        new public void InitializeContexts()
        {
            base.InitializeContexts();
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            Gl.glClearDepth(2.0f);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);

            calculateCameraViews();
            perspectiveEye = perspectiveEye == null ? eyeDefault : perspectiveEye;


            this.KeyDown += new KeyEventHandler(keyboard);
            this.MouseDown += new MouseEventHandler(mDown);
            this.MouseClick += new MouseEventHandler(mClick);
            //this.MouseMove += new MouseEventHandler(mMove);
        }

        // Overidden method to handel the Event of the window's size being altered
        protected override void OnSizeChanged(EventArgs e)
        {
            // The base keyword is used to access members of the 
            // base class from within a derived class. So here it is accessing the 
            // OnSizeChanged() function from CsGl.OpenGl!
            base.OnSizeChanged(e);

            // the following setup functions are as normal for an OpenGl window.
            int height, width = this.Width;
            if (this.Height == 0)
                height = 1;
            else
                height = this.Height;

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            //Reset viewport and perspective
            setPerspective(width, height);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            calculateCameraViews();
        }

        public void setupScene()
        {
            // Below are the general OpenGl drawing setup functions note the Gl prefix
            if (is_depth)
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);	// Clear Screen And Depth Buffer
            else
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);                             // Clear Screen

            Gl.glEnable(Gl.GL_BLEND);

            //Set viewport to entire screen
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, this.Width, this.Height);
            setPerspective(this.Width, this.Height);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Glu.gluLookAt(perspectiveEye[0], perspectiveEye[1], perspectiveEye[2], 0, 0, 0, 0, 1, 0);

        }
        public void flushScene()
        {
            Gl.glFlush();
        }

        #region DRAW FUNCTIONS
        void setViewport(int myWidth, int myHeight)
        {
            Gl.glViewport(0, 0, myWidth, myHeight);
        }
        void setPerspective(float myWidth, float myHeight)
        {
            if (myHeight == 0)
                myHeight = 1;
            Glu.gluPerspective(90, (float)myWidth / myHeight, 0.1, 2000);          // Calculate The Aspect Ratio Of The Window
        }
        private void calculateCameraViews()
        {
            eyeFront = new double[] { 0.0d, 0.0d, 
                    viewDistance};
            eyeCorner = new double[]{ viewDistance/2, 
                                            viewDistance/2, 
                                            viewDistance/2};
            eyeTop = new double[] { 0.0d, 
                                        viewDistance,
                                        1.0d};
            eyeIso = new double[]{ viewDistance/2/Math.Tan(45 * Math.PI / 180), 
                                        viewDistance /2/ Math.Tan(45 * Math.PI / 180), 
                                        viewDistance / 2 / Math.Tan(45 * Math.PI / 180) };
            eyeSide = new double[] { viewDistance, 0.0d, 0.0d };
        }
        public void calculateIncrements()
        {
            //Calculate X rotation increment
            //Hypoteni
            double hypotenuseXY = getHypotenuse(perspectiveEye[0], perspectiveEye[2]);
            incrementX = incrementL*Math.Cos(Math.Atan(perspectiveEye[0] / perspectiveEye[2]));
            incrementZX = incrementL * Math.Sin(Math.Atan(perspectiveEye[0] / perspectiveEye[2]));

            incrementY = incrementL * Math.Cos(Math.Atan(perspectiveEye[1] / perspectiveEye[2]));
            incrementZY = incrementL * Math.Sin(Math.Atan(perspectiveEye[1] / perspectiveEye[2]));

        }
        private double getHypotenuse(double side1, double side2)
        {
            return Math.Sqrt(side1 * side1 + side2 * side2);
        }
        #endregion

        #region INPUT AND SELECTION
        void keyboard(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyData;
            Keys mod = e.Modifiers;

            /* This time the controls are:    	
              "a": move left
              "d": move right
              "w": move forward
              "s": move back
              "t": toggle depth-testing
            */
            /*
            switch (key)
            {
                case Keys.A:
                    xTranslate = tileSize;
                    break;
                case Keys.D:
                    xTranslate = -tileSize;
                    break;
                case Keys.W:
                    zTranslate = tileSize;
                    break;
                case Keys.S:
                    zTranslate = -tileSize;
                    break;
                case Keys.T:
                    if (is_depth)
                    {
                        is_depth = false;
                        Gl.glDisable(Gl.GL_DEPTH_TEST);
                    }
                    else
                    {
                        is_depth = true;
                        Gl.glEnable(Gl.GL_DEPTH_TEST);
                    }
                    break;
                case Keys.R:
                    yRotate = -totYRot;
                    break;
                case Keys.Escape:
                    finished = true;
                    break;
            }
            glDraw();*/
        }
        void mDown(object sender, MouseEventArgs e)
        {
            clickX = e.X;
            clickY = e.Y;
        }
        void mClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
            }
        }
        void mMove(object sender, MouseEventArgs e)
        {
            clickX = e.X;
            clickY = e.Y;
        }
        void processHits(int hits, int[] buffer)
        {
            int i, j;
            int names;
            int index = 0;

            Console.Write("hits = " + hits + "\n");
            for (i = 0; i < hits; i++)
            { /*  for each hit  */
                names = buffer[index]; index++;
                Console.Write("number of names for hit = {0} \n", names);
                Console.Write("  z1 is {0};", (float)buffer[index] / 0x7fffffff); index++;
                Console.Write(" z2 is {0}\n", (float)buffer[index] / 0x7fffffff); index++;
                Console.Write("   the name is ");
                for (j = 0; j < names; j++)
                {     /*  for each name */
                    Console.Write("{0} ", buffer[index]);
                    index++;
                }
                Console.Write("\n");
            }
        }
        void beginSelection(MouseEventArgs e)
        {
            int[] viewport = new int[4];
            float aspectRatio = (float)Width / Height;
            float curLY = 0.2f, curLX = curLY * aspectRatio;


            Gl.glRenderMode(Gl.GL_SELECT);

            Gl.glInitNames();
            Gl.glPushName(0);

            //Save current state matrix
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();

            //Set selection view to entire screen
            Gl.glViewport(0, 0, Width, Height);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            //Create picking matrix
            Glu.gluPickMatrix(e.X, Height - e.Y,
                curLX, curLY, viewport);
            setPerspective(curLX, curLY);

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
        void drawSelectionInset(float vX, float vY)
        {
            float aspectRatio = (float)this.Width / this.Height;
            float insetHeight = 70, insetWidth = insetHeight * aspectRatio;
            float curLY = 0.2f, curLX = curLY * aspectRatio;
            int[] viewport = new int[4];

            Gl.glRenderMode(Gl.GL_RENDER);

            //Save current state matrices
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();

            //Set inset and get new viewport
            Gl.glViewport(Width - (int)insetWidth, Height - (int)insetHeight, (int)insetWidth, (int)insetHeight);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            //Translate cursor position
            float curX = vX, curY = vY;
            //This translates cursor from the large viewport screen coordinates to the small one's
            float tCurX = insetWidth / (this.Width) * curX + this.Width - insetWidth;
            float tCurY = insetHeight / (this.Height) * curY;

            //This translates screen coords to small viewport coordinates
            float svCurX = tCurX - this.Width + insetWidth / 2;
            float svCurY = tCurY - insetHeight / 2;

            //Create picking matrix
            Glu.gluPickMatrix(tCurX, this.Height - tCurY,
                curLX, curLY, viewport);

            setPerspective(curLX, curLY);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();



            //Restore matrices
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopMatrix();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPopMatrix();
            Gl.glFlush();
        }
        #endregion INPUTANDSELECTION
    }
}
