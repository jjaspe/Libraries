using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;
using System.Windows.Forms;
using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template
{
    public class simpleOpenGlView : Tao.Platform.Windows.SimpleOpenGlControl, ICanvas
    {
        #region GEOMETRY AND VIEWING VARIABLES
        static bool is_depth = true;
        private static bool[] keys = new bool[256];                             // Array Used For The Keyboard Routine
        // int Errors = 0;

        //COLORS
        static double[] colorBlack = { 0.0f, 0.0f, 0.0f }, colorWhite = { 1.0f, 1.0f, 1.0f }, colorBlue = { 0.0f, 0.0f, 1.0f };
        static double[] colorRed = { 1.0f, 0.0f, 0.0f }, colorOrange = { 1.0f, 0.5f, 0.25f }, colorYellow = { 1.0f, 1.0f, 0.0f };
        static double[] colorGreen = { 0.0f, 1.0f, 0.2f }, colorBrown = { 0.4f, 0.5f, 0.3f };

        #endregion

        #region START UP VARIABLES
        static int clickX, clickY;             //Coordinates of mouse pointer at last click
        #endregion

        #region VIEWS
        double viewDistance = 300.0d;
        public double ViewDistance
        {
            get { return viewDistance; }
            set { viewDistance = value; setView(); }
        }

        double viewTheta = 0;
        public double ViewTheta
        {
            get { return viewTheta; }
            set { viewTheta = value; setView(); }
        }

        double viewPhi = 0;
        public double ViewPhi
        {
            get { return viewPhi; }
            set { viewPhi = value; setView(); }
        }

        double[] translation = new double[3];

        void setParameters()
        {
            double[] v = PerspectiveEye;
            viewDistance = Math.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
            viewTheta = Math.Atan2(v[1], v[0]) * 180 / Math.PI;
            viewPhi = Math.Atan2(v[2], Math.Sqrt(v[0] * v[0] + v[1] * v[1])) * 180 / Math.PI;
            setView();
        }
        void setView()
        {
            double z = ViewDistance * Math.Sin(viewPhi * Math.PI / 180),
                   x = ViewDistance * Math.Cos(viewPhi * Math.PI / 180) * Math.Cos(viewTheta * Math.PI / 180),
                   y = ViewDistance * Math.Cos(viewPhi * Math.PI / 180) * Math.Sin(viewTheta * Math.PI / 180);
            z = z == 0 ? 0.0001d : z;
            perspectiveEye = new double[] { x, y, z };
        }

        private double[] perspectiveEye;
        public double[] eyeFront;
        public double[] eyeCorner;
        public double[] eyeTop;
        public double[] eyeIso;
        public double[] eyeFrontIso;
        public double[] eye2D;
        private double[] eyeCustom;

        public double[] EyeCustom
        {
            get { return eyeCustom; }
            set { eyeCustom = value; }
        }
        public double[] PerspectiveEye
        {
            get { return perspectiveEye; }
            set
            {
                perspectiveEye = value;
                setParameters();
            }
        }
        public enum VIEWS {
            TwoD=0,
            Front=1,
            Iso=2,
            Corner=3,
            Top=4,
            FrontUp=5,
            Left,
        Back,
        Right,
        Bottom
        };
        public double[][] angles = new double[][]
        { 
            new double[]{-90,90},//TwoD
            new double[]{-90,0},//Front
            new double[]{225,75},//Iso
            new double[]{225,45},//Corner
            new double[]{-90,90},//Top
            new double[]{-90,30},//FrontUp
            new double[]{0,0},//Right
            new double[]{90,0},//back
            new double[]{180,0},//left
            new double[]{-90,-90}//Bottom
        };

        #endregion

        public simpleOpenGlView()
        {
            setView();
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
            this.Dock = DockStyle.Left;
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            Gl.glClearDepth(1.0f);
            Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);

            calculateCameraViews();

            //this.KeyDown += new KeyEventHandler(keyboard);
            //this.MouseDown += new MouseEventHandler(mDown);
            //this.MouseClick += new MouseEventHandler(mClick);
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

            Glu.gluLookAt(PerspectiveEye[0], PerspectiveEye[1], PerspectiveEye[2], 0, 0, 0, 0, 0, 1);
            beginTranslateView();

        }
        public void flushScene()
        {
            Gl.glFlush();
            endTranslateView();
        }
        public void beginTranslateView()
        {
            Gl.glPushMatrix();
            Gl.glTranslated(translation[0], translation[1], translation[2]);
        }
        public void endTranslateView()
        {
            Gl.glPopMatrix();
        }
        private void calculateCameraViews()
        {
            eyeFront = new double[] { 0.0d, 0.0d, 
                    ViewDistance};
            eyeCorner = new double[]{ -Math.Sqrt(3)*ViewDistance, 
                                            -Math.Sqrt(3)*ViewDistance, 
                                            Math.Sqrt(3)*ViewDistance};
            eyeTop = new double[] { 0.0d,//this.Width / Math.Tan(30 * Math.PI / 180), 
                                        ViewDistance ,//Height / Math.Tan(45 * Math.PI / 180),
                                        0.0001d};//this.Width / Math.Tan(30 * Math.PI / 180) };
            eyeIso = new double[]{ -ViewDistance*2/3, 
                                        -ViewDistance *2/3, 
                                        ViewDistance / 3 };
            eyeFrontIso = new double[] { -ViewDistance/2 / Math.Tan(30* Math.PI / 180), 
                                        -ViewDistance/4 /Math.Tan(30 * Math.PI / 180), 
                                        ViewDistance/2 /Math.Tan(30 * Math.PI / 180) };
            eye2D = new double[] { 0.0d, 
                                        0.0d, 
                                        ViewDistance };

            eyeCustom  = new double[]{ -ViewDistance/ Math.Sqrt(2)*Math.Cos((45+ViewTheta) * Math.PI / 180), 
                                        -ViewDistance /Math.Sqrt(2)*Math.Sin((45+ViewTheta)  * Math.PI / 180), 
                                        ViewDistance / Math.Tan(30 * Math.PI / 180) };

            eyeCustom = new double[]{ -ViewDistance*Math.Cos(ViewTheta* Math.PI / 180)*Math.Cos(viewPhi*Math.PI/180), 
                                        -ViewDistance*Math.Sin(ViewTheta* Math.PI / 180)*Math.Cos(viewPhi*Math.PI/180), 
                                        -ViewDistance*Math.Sin(viewPhi*Math.PI/180) };


        }
        public void resetCamera()
        {
            //PerspectiveEye = eyeCustom;
        }
        public void setCameraView(double[] view)
        {
            PerspectiveEye = view;           
        }
        public void setCameraView(VIEWS view)
        {
            viewTheta = angles[(int)view][0];
            viewPhi = angles[(int)view][1];
            setView();
        }

        public void rotateCW()
        {
            ViewTheta = (ViewTheta + 15) % 360;
            calculateCameraViews();
            resetCamera();
        }
        public void rotateCCW()
        {
            ViewTheta = (ViewTheta - 15) % 360;
            calculateCameraViews();
            resetCamera();
        }
        public void zoomIn()
        {
            ViewDistance = ViewDistance / 1.4;
            //calculateCameraViews();
            //resetCamera();
        }
        public void zoomOut()
        {
            ViewDistance = ViewDistance * 1.4;
            //calculateCameraViews();
            //resetCamera();
        }
        public void moveXUp()
        {
            eyeCustom = PerspectiveEye;
            //eyeCustom[0] += ViewDistance / 50;
            PerspectiveEye = eyeCustom;
            translation[0] += ViewDistance / 50;
        }
        public void moveXDown()
        {
            eyeCustom = PerspectiveEye;
            //eyeCustom[0] -= ViewDistance / 50;
            PerspectiveEye = eyeCustom;
            translation[0] -= ViewDistance / 50;
        }
        public void moveYUp()
        {
            eyeCustom = PerspectiveEye;
            //eyeCustom[1] += ViewDistance / 50;
            PerspectiveEye = eyeCustom;
            translation[1] += ViewDistance / 50;
        }
        public void moveYDown()
        {
            eyeCustom = PerspectiveEye;
            //eyeCustom[1] -= ViewDistance / 50;
            PerspectiveEye = eyeCustom;
            translation[1] -= ViewDistance / 50;
        }
        public void moveZUp()
        {
            eyeCustom = PerspectiveEye;
            //eyeCustom[2] += ViewDistance / 50;
            PerspectiveEye = eyeCustom;
            translation[2] += ViewDistance / 50;
        }
        public void moveZDown()
        {
            eyeCustom = PerspectiveEye;
            //eyeCustom[2] -= ViewDistance / 50;
            PerspectiveEye = eyeCustom;
            translation[2] -= ViewDistance / 50;
        }

        #region ICanvasFunctions FUNCTIONS
        void setViewport(int myWidth, int myHeight)
        {
            Gl.glViewport(0, 0, myWidth, myHeight);
        }
        public void setPerspective(double myWidth, double myHeight)
        {
            if (myHeight == 0)
                myHeight = 1;
            Glu.gluPerspective(90, (double)myWidth / myHeight, 20, 600);          // Calculate The Aspect Ratio Of The Window
        }
        public int getWidth() { return Width; }
        public int getHeight() { return Height; }
        #endregion

        
        #region INPUT AND SELECTION
        /*
        void keyboard(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyData;
            Keys mod = e.Modifiers;

             This time the controls are:    	
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
        glDraw();
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
        { // for each hit  
            names = buffer[index]; index++;
            Console.Write("number of names for hit = {0} \n", names);
            Console.Write("  z1 is {0};", (double)buffer[index] / 0x7fffffff); index++;
            Console.Write(" z2 is {0}\n", (double)buffer[index] / 0x7fffffff); index++;
            Console.Write("   the name is ");
            for (j = 0; j < names; j++)
            {     //  for each name 
                Console.Write("{0} ", buffer[index]);
                index++;
            }
            Console.Write("\n");
        }
    }
    void beginSelection(MouseEventArgs e)
    {
        int[] viewport = new int[4];
        double aspectRatio = (double)Width / Height;
        double curLY = 0.2f, curLX = curLY * aspectRatio;


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
    void drawSelectionInset(double vX, double vY)
    {
        double aspectRatio = (double)this.Width / this.Height;
        double insetHeight = 70, insetWidth = insetHeight * aspectRatio;
        double curLY = 0.2f, curLX = curLY * aspectRatio;
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
        double curX = vX, curY = vY;
        //This translates cursor from the large viewport screen coordinates to the small one's
        double tCurX = insetWidth / (this.Width) * curX + this.Width - insetWidth;
        double tCurY = insetHeight / (this.Height) * curY;

        //This translates screen coords to small viewport coordinates
        double svCurX = tCurX - this.Width + insetWidth / 2;
        double svCurY = tCurY - insetHeight / 2;

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
         * */
        #endregion INPUTANDSELECTION
    }
}
