using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
//using CsGl.OpenGl;

//Enumerated started from middle far point, going clockwise.
enum Direction { Far, FarRight, Right, NearRight, Near, NearLeft, Left, FarLeft }
/*************************************************************************
 * Class QuadricDemo
 * 
 * This is essentially the Main class.  The application is run from here
 * This class adds the OpenGl drawings etc... to the newly created Window.
 * **********************************************************************/
public class QuadricDemo : Form
{

    #region SET UP VARIABLES
    static bool is_depth = true;			        //depth testing flag
    static bool finished = false;
    static bool fullscreen = false;
    static Form form;
    private static bool[] keys = new bool[256];                             // Array Used For The Keyboard Routine
    private static IntPtr hDC;                                              // Private GDI Device Context
    private static IntPtr hRC;                                              // Permanent Rendering Context

    //COLORS
    static float[] colorBlack ={ 0.0f, 0.0f, 0.0f }, colorWhite ={ 1.0f, 1.0f, 1.0f }, colorBlue ={ 0.0f, 0.0f, 1.0f };
    static float[] colorRed ={ 1.0f, 0.0f, 0.0f }, colorOrange = { 1.0f, 0.5f, 0.25f }, colorYellow ={ 1.0f, 1.0f, 0.0f };
    static float[] colorGreen ={ 0.0f, 1.0f, 0.2f }, colorBrown ={ 0.4f, 0.5f, 0.3f };

    private static int fontbase;
    #endregion

    #region START UP VARIABLES
    static int clickX, clickY;             //Coordinates of mouse pointer at last click
    const int tileSize = 50;
    const int backTileSize = 8;
    const int outcroppingSize = 8;
    const int blockSize = 28;
    const int number_of_tiles = 1;
    const int wallSize =  number_of_tiles* tileSize;
    const int height = 840, width = 1440;

    static bool selectMode = false;
    static int selectedTile = number_of_tiles*number_of_tiles+1;
    static int selectedButton = 5;
    static bool stop = false;
    /************************************************/
    #endregion

    #region OBJECTS
    static characterObj PC = new characterObj();
    static tileObj[,] myTiles = new tileObj[wallSize / tileSize, wallSize / tileSize];
    static tileObj[] backgroundButtons = new tileObj[5];

    #endregion


    // Class Constructor
    public QuadricDemo()
    {
        this.CreateParams.ClassStyle = this.CreateParams.ClassStyle |       // Redraw On Size, And Own DC For Window.
                User.CS_HREDRAW | User.CS_VREDRAW | User.CS_OWNDC;
        //this.MinimizeBox = false;
        //this.MaximizeBox = false;
        this.WindowState = FormWindowState.Maximized;
        

        this.KeyDown += new KeyEventHandler(keyboard);
        this.MouseDown += new MouseEventHandler(mDown);
        this.MouseClick += new MouseEventHandler(mClick);
        this.MouseMove += new MouseEventHandler(mMove);
        this.Closing += new CancelEventHandler(this.Form_Closing);          // On Closing Event Call Form_Closing
    }

    public static void Main()
    {
        if (!CreateGlWindow("Wall Maker", 12*6, 7*6, 16, false))
            finished = true;

        initObjects();        

        while (!finished)
        {
            Application.DoEvents();
            if(!stop)
                drawGlScene(); 
            Gdi.SwapBuffers(hDC);
            form.Width = 1440;
            form.Height = 840;
        }

        // Shutdown
        KillGlWindow();                                                     // Kill The Window
        return;
    }
  
    // This function sets up the OpenGl context as normal, but note the Gl prefix's!
    protected static void InitGl()
    {
        Gl.glShadeModel(Gl.GL_SMOOTH);
        Gl.glEnable(Gl.GL_DEPTH_TEST);
        Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
        Gl.glClearDepth(1.0f);
        Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
        Gl.glDepthFunc(Gl.GL_LEQUAL);
        BuildFont();
    }

    // Overidden method to handel the Event of the window's size being altered
    protected override void OnSizeChanged(EventArgs e)
    {
        // The base keyword is used to access members of the 
        // base class from within a derived class. So here it is accessing the 
        // OnSizeChanged() function from CsGl.OpenGl!
        base.OnSizeChanged(e);

        // the following setup functions are as normal for an OpenGl window.
        int height, width = form.Width;
        if (this.Height == 0)
            height = 1;
        else
            height = form.Height;

        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glLoadIdentity();

        //Reset viewport and perspective
        setPerspective(width, height);

        Gl.glMatrixMode(Gl.GL_MODELVIEW);
    }

    #region bool CreateGlWindow(string title, int width, int height, int bits, bool fullscreenflag)
    /// <summary>
    ///     Creates our OpenGl Window.
    /// </summary>
    /// <param name="title">
    ///     The title to appear at the top of the window.
    /// </param>
    /// <param name="width">
    ///     The width of the Gl window or fullscreen mode.
    /// </param>
    /// <param name="height">
    ///     The height of the Gl window or fullscreen mode.
    /// </param>
    /// <param name="bits">
    ///     The number of bits to use for color (8/16/24/32).
    /// </param>
    /// <param name="fullscreenflag">
    ///     Use fullscreen mode (<c>true</c>) or windowed mode (<c>false</c>).
    /// </param>
    /// <returns>
    ///     <c>true</c> on successful window creation, otherwise <c>false</c>.
    /// </returns>
    private static bool CreateGlWindow(string title, int width, int height, int bits, bool fullscreenflag)
    {
        int pixelFormat;                                                    // Holds The Results After Searching For A Match
        fullscreen = fullscreenflag;                                        // Set The Global Fullscreen Flag
        form = null;                                                        // Null The Form

        GC.Collect();                                                       // Request A Collection
        // This Forces A Swap
        Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

        if (fullscreen)
        {                                                    // Attempt Fullscreen Mode?
            Gdi.DEVMODE dmScreenSettings = new Gdi.DEVMODE();               // Device Mode
            // Size Of The Devmode Structure
            dmScreenSettings.dmSize = (short)Marshal.SizeOf(dmScreenSettings);
            dmScreenSettings.dmPelsWidth = width;                           // Selected Screen Width
            dmScreenSettings.dmPelsHeight = height;                         // Selected Screen Height
            dmScreenSettings.dmBitsPerPel = bits;                           // Selected Bits Per Pixel
            dmScreenSettings.dmFields = Gdi.DM_BITSPERPEL | Gdi.DM_PELSWIDTH | Gdi.DM_PELSHEIGHT;

            // Try To Set Selected Mode And Get Results.  NOTE: CDS_FULLSCREEN Gets Rid Of Start Bar.
            fullscreen = true;
        }

        form = new QuadricDemo();                                              // Create The Window

        form.Width = width;                                                 // Set Window Width
        form.Height = height;                                               // Set Window Height
        form.Text = title;

        #region WINDOW CREATION STUFF
        Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();    // pfd Tells Windows How We Want Things To Be
        pfd.nSize = (short)Marshal.SizeOf(pfd);                            // Size Of This Pixel Format Descriptor
        pfd.nVersion = 1;                                                   // Version Number
        pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW |                              // Format Must Support Window
            Gdi.PFD_SUPPORT_OPENGL |                                        // Format Must Support OpenGl
            Gdi.PFD_DOUBLEBUFFER;                                           // Format Must Support Double Buffering
        pfd.iPixelType = (byte)Gdi.PFD_TYPE_RGBA;                          // Request An RGBA Format
        pfd.cColorBits = (byte)bits;                                       // Select Our Color Depth
        pfd.cRedBits = 0;                                                   // Color Bits Ignored
        pfd.cRedShift = 0;
        pfd.cGreenBits = 0;
        pfd.cGreenShift = 0;
        pfd.cBlueBits = 0;
        pfd.cBlueShift = 0;
        pfd.cAlphaBits = 0;                                                 // No Alpha Buffer
        pfd.cAlphaShift = 0;                                                // Shift Bit Ignored
        pfd.cAccumBits = 0;                                                 // No Accumulation Buffer
        pfd.cAccumRedBits = 0;                                              // Accumulation Bits Ignored
        pfd.cAccumGreenBits = 0;
        pfd.cAccumBlueBits = 0;
        pfd.cAccumAlphaBits = 0;
        pfd.cDepthBits = 16;                                                // 16Bit Z-Buffer (Depth Buffer)
        pfd.cStencilBits = 0;                                               // No Stencil Buffer
        pfd.cAuxBuffers = 0;                                                // No Auxiliary Buffer
        pfd.iLayerType = (byte)Gdi.PFD_MAIN_PLANE;                         // Main Drawing Layer
        pfd.bReserved = 0;                                                  // Reserved
        pfd.dwLayerMask = 0;                                                // Layer Masks Ignored
        pfd.dwVisibleMask = 0;
        pfd.dwDamageMask = 0;

        hDC = User.GetDC(form.Handle);                                      // Attempt To Get A Device Context
        if (hDC == IntPtr.Zero)
        {                                            // Did We Get A Device Context?
            KillGlWindow();                                                 // Reset The Display
            MessageBox.Show("Can't Create A Gl Device Context.", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);                  // Attempt To Find An Appropriate Pixel Format
        if (pixelFormat == 0)
        {                                              // Did Windows Find A Matching Pixel Format?
            KillGlWindow();                                                 // Reset The Display
            MessageBox.Show("Can't Find A Suitable PixelFormat.", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd))
        {                // Are We Able To Set The Pixel Format?
            KillGlWindow();                                                 // Reset The Display
            MessageBox.Show("Can't Set The PixelFormat.", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        hRC = Wgl.wglCreateContext(hDC);                                    // Attempt To Get The Rendering Context
        if (hRC == IntPtr.Zero)
        {                                            // Are We Able To Get A Rendering Context?
            KillGlWindow();                                                 // Reset The Display
            MessageBox.Show("Can't Create A Gl Rendering Context.", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (!Wgl.wglMakeCurrent(hDC, hRC))
        {                                 // Try To Activate The Rendering Context
            KillGlWindow();                                                 // Reset The Display
            MessageBox.Show("Can't Activate The Gl Rendering Context.", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion


        form.Show();                                                        // Show The Window
        form.Focus();                                                       // Focus The Window       

        InitGl();                                                            //Initiate OpenGl

        setViewport(width, height);                                          // Set viewport
        setPerspective(width, height);                                       // Set Up Our Perspective Gl Screen

        return true;                                                         // Success
        
    }
    #endregion bool CreateGlWindow(string title, int width, int height, int bits, bool fullscreenflag)

    #region KillGlWindow()
    /// <summary>
    ///     Properly kill the window.
    /// </summary>
    private static void KillGlWindow()
    {
        if (fullscreen)
        {                                                    // Are We In Fullscreen Mode?
            User.ChangeDisplaySettings(IntPtr.Zero, 0);                     // If So, Switch Back To The Desktop
            Cursor.Show();                                                  // Show Mouse Pointer
        }

        if (hRC != IntPtr.Zero)
        {                                            // Do We Have A Rendering Context?
            if (!Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero))
            {             // Are We Able To Release The DC and RC Contexts?
                MessageBox.Show("Release Of DC And RC Failed.", "SHUTDOWN ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!Wgl.wglDeleteContext(hRC))
            {                                // Are We Able To Delete The RC?
                MessageBox.Show("Release Rendering Context Failed.", "SHUTDOWN ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            hRC = IntPtr.Zero;                                              // Set RC To Null
        }

        if (hDC != IntPtr.Zero)
        {                                            // Do We Have A Device Context?
            if (form != null && !form.IsDisposed)
            {                          // Do We Have A Window?
                if (form.Handle != IntPtr.Zero)
                {                            // Do We Have A Window Handle?
                    if (!User.ReleaseDC(form.Handle, hDC))
                    {                 // Are We Able To Release The DC?
                        MessageBox.Show("Release Device Context Failed.", "SHUTDOWN ERROR",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            hDC = IntPtr.Zero;                                              // Set DC To Null
        }

        if (form != null)
        {                                                  // Do We Have A Windows Form?
            form.Hide();                                                    // Hide The Window
            form.Close();                                                   // Close The Form
            form.Dispose();
            form = null;                                                    // Set form To Null
        }
        
    }
    #endregion KillGlWindow()

    private static void initObjects()
    {
        PC.setColor(colorRed);

        //Create tiles
        for (int i = 0; i < number_of_tiles; i++)
        {
            for (int j = 0; j < number_of_tiles; j++)
            {
                myTiles[i, j] = new tileObj(i*tileSize-wallSize/2, j*tileSize-wallSize/2, 0, colorRed,colorWhite);
            }
        }

        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            backgroundButtons[i] = new tileObj();
            backgroundButtons[i].myX = -form.Width / 2 + 100;
            backgroundButtons[i].myY = form.Height / 2 - 100 - 50 * i;
            switch (i)
            {
                case 0:
                    backgroundButtons[i].setColor(colorRed);
                    break;
                case 1:
                    backgroundButtons[i].setColor(colorOrange);
                    break;
                case 2:
                    backgroundButtons[i].setColor(colorYellow);
                    break;
                case 3:
                    backgroundButtons[i].setColor(colorGreen);
                    break;
                case 4:
                    backgroundButtons[i].setColor(colorBrown);
                    break;
                default:
                    backgroundButtons[i].setColor(colorWhite);
                    break;
            }
        }
    }

    private static void drawGlScene()
    {        
        // Below are the general OpenGl drawing setup functions note the Gl prefix
        if (is_depth)
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);	// Clear Screen And Depth Buffer
        else
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);                             // Clear Screen

        Gl.glEnable(Gl.GL_BLEND);

        //SET UP PROJECTION BEFORE DRAWING
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glLoadIdentity();
        
        setPerspective(form.Width, form.Height);

        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glLoadIdentity();

        #region DRAW SCENE
        Glu.gluLookAt(0, 0, form.Height / 2 / Math.Tan(45 * Math.PI / 180), 0, 0, 0, 0, 1, 0);
        drawWall();
        drawText();
        drawBackgroundButtons();
        #endregion

        Gl.glFlush();
        drawSelectionInset(clickX, clickY);     
    }      

    #region DRAW FUNCTIONS
    static void drawTile()
    {
        drawTile(new float[] { 0.0f, 0.0f, 0.0f });
    }
    static void drawTile(float[] color)
    {
        Gl.glBegin(Gl.GL_QUADS);
        Gl.glColor3fv(color);
        Gl.glVertex3f(0.0f, 0.0f, 0.0f);
        Gl.glVertex3f(tileSize, 0.0f, 0.0f);
        Gl.glVertex3f(tileSize, tileSize, 0.0f);
        Gl.glVertex3f(0.0f, tileSize, 0.0f);
        Gl.glEnd();
    }
    static void drawCube(float[] color)
    {
        float x_big = blockSize;
        float y_big = blockSize;
        float x_small = 0;
        float y_small = 0;

        Gl.glColor3fv(color);
        Gl.glBegin(Gl.GL_QUADS);
        Gl.glVertex3f(x_small, y_big, blockSize); /* front */
        Gl.glVertex3f(x_small, y_small, blockSize);
        Gl.glVertex3f(x_big, y_small, blockSize);
        Gl.glVertex3f(x_big, y_big, blockSize);

        Gl.glVertex3f(x_big, y_big, 0.0f); /* back */
        Gl.glVertex3f(x_big, y_small, 0.0f);
        Gl.glVertex3f(x_small, y_small, 0.0f);
        Gl.glVertex3f(x_small, y_big, 0.0f);

        Gl.glVertex3f(x_big, y_big, blockSize); /* right */
        Gl.glVertex3f(x_big, y_small, blockSize);
        Gl.glVertex3f(x_big, y_small, 0.0f);
        Gl.glVertex3f(x_big, y_big, 0.0f);

        Gl.glVertex3f(x_small, y_big, 0.0f); /* left */
        Gl.glVertex3f(x_small, y_small, 0.0f);
        Gl.glVertex3f(x_small, y_small, blockSize);
        Gl.glVertex3f(x_small, y_big, blockSize);

        Gl.glVertex3f(x_small, y_big, blockSize); /* top */
        Gl.glVertex3f(x_big, y_big, blockSize);
        Gl.glVertex3f(x_big, y_big, 0.0f);
        Gl.glVertex3f(x_small, y_big, 0.0f);

        Gl.glVertex3f(x_small, y_small, 0.0f); /* bottom */
        Gl.glVertex3f(x_big, y_small, 0.0f);
        Gl.glVertex3f(x_big, y_small, blockSize);
        Gl.glVertex3f(x_small, y_small, blockSize);
        Gl.glEnd();
    }
    static void drawCube(int x_offset, int z_offset, float[] color)
    {
        /* this function draws a cube centerd at (x_offset, z_offset)
	    x and z _big are the back and rightmost points, x and z _small are
	    the front and leftmost points */
        float x_big = (float)x_offset * tileSize + blockSize;
        float z_big = (float)z_offset * tileSize + blockSize;
        float x_small = (float)x_offset * tileSize;
        float z_small = (float)z_offset * tileSize;

        Gl.glColor3fv(color);
        Gl.glBegin(Gl.GL_QUADS);
        Gl.glVertex3f(x_small, blockSize, z_big); /* front */
        Gl.glVertex3f(x_small, 0.0f, z_big);
        Gl.glVertex3f(x_big, 0.0f, z_big);
        Gl.glVertex3f(x_big, blockSize, z_big);

        Gl.glVertex3f(x_big, blockSize, z_small); /* back */
        Gl.glVertex3f(x_big, 0.0f, z_small);
        Gl.glVertex3f(x_small, 0.0f, z_small);
        Gl.glVertex3f(x_small, blockSize, z_small);

        Gl.glVertex3f(x_big, blockSize, z_big); /* right */
        Gl.glVertex3f(x_big, 0.0f, z_big);
        Gl.glVertex3f(x_big, 0.0f, z_small);
        Gl.glVertex3f(x_big, blockSize, z_small);

        Gl.glVertex3f(x_small, blockSize, z_small); /* left */
        Gl.glVertex3f(x_small, 0.0f, z_small);
        Gl.glVertex3f(x_small, 0.0f, z_big);
        Gl.glVertex3f(x_small, blockSize, z_big);

        Gl.glVertex3f(x_small, blockSize, z_big); /* top */
        Gl.glVertex3f(x_big, blockSize, z_big);
        Gl.glVertex3f(x_big, blockSize, z_small);
        Gl.glVertex3f(x_small, blockSize, z_small);

        Gl.glVertex3f(x_small, 0.0f, z_small); /* bottom */
        Gl.glVertex3f(x_big, 0.0f, z_small);
        Gl.glVertex3f(x_big, 0.0f, z_big);
        Gl.glVertex3f(x_small, 0.0f, z_big);
        Gl.glEnd();
    }
    static void drawSelectedCube(float[] cubeColor, float[] outlineColor)
    {
        //Draw cube
        drawCube(0, 0, cubeColor);
        //Draw outline
        drawOutline(outlineColor);
    }
    static void drawOutline(float[] color)
    {

        //Set selection lines color
        Gl.glColor3fv(color);

        //Draw selection lines
        float x_big = blockSize;
        float z_big = blockSize;
        float x_small = 0;
        float z_small = 0;

        Gl.glBegin(Gl.GL_LINE_LOOP);
        Gl.glVertex3f(x_small, blockSize, z_big); /* front */
        Gl.glVertex3f(x_small, 0.0f, z_big);
        Gl.glVertex3f(x_big, 0.0f, z_big);
        Gl.glVertex3f(x_big, blockSize, z_big);
        Gl.glEnd();

        Gl.glBegin(Gl.GL_LINE_LOOP);
        Gl.glVertex3f(x_big, blockSize, z_small); /* back */
        Gl.glVertex3f(x_big, 0.0f, z_small);
        Gl.glVertex3f(x_small, 0.0f, z_small);
        Gl.glVertex3f(x_small, blockSize, z_small);
        Gl.glEnd();

        Gl.glBegin(Gl.GL_LINE_LOOP);
        Gl.glVertex3f(x_big, blockSize, z_big); /* right */
        Gl.glVertex3f(x_big, 0.0f, z_big);
        Gl.glVertex3f(x_big, 0.0f, z_small);
        Gl.glVertex3f(x_big, blockSize, z_small);
        Gl.glEnd();

        Gl.glBegin(Gl.GL_LINE_LOOP);
        Gl.glVertex3f(x_small, blockSize, z_small); /* left */
        Gl.glVertex3f(x_small, 0.0f, z_small);
        Gl.glVertex3f(x_small, 0.0f, z_big);
        Gl.glVertex3f(x_small, blockSize, z_big);
        Gl.glEnd();
    }
    static void drawTileOutline(float[] color)
    {
        Gl.glBegin(Gl.GL_LINE_LOOP);
        Gl.glColor3fv(color);
        Gl.glVertex2f(0, 0); 
        Gl.glVertex2f(tileSize, 0);
        Gl.glVertex2f(tileSize, tileSize);
        Gl.glVertex2f(0, tileSize);
        Gl.glEnd();

    }
    //OFFSETS WILL BE TREATED AS REAL COORDINATES, NOT MULTIPLES OF TILESIZES OR ANY OTHER CONSTANT!!
    static void drawSquare(int x_offset, int y_offset, float[] color)
    {
        Gl.glBegin(Gl.GL_QUADS);
        Gl.glColor3fv(color);
        Gl.glVertex2d(x_offset, y_offset);
        Gl.glVertex2d(x_offset + blockSize, y_offset);
        Gl.glVertex2d(x_offset + blockSize, y_offset + blockSize);
        Gl.glVertex2d(x_offset, y_offset + blockSize);
        Gl.glEnd();
    }
    static void drawOutcropping(int x_offset, int y_offset, float[] color)
    {
        Gl.glBegin(Gl.GL_QUADS);
        Gl.glColor3fv(color);
        Gl.glVertex2d(x_offset * backTileSize, y_offset * backTileSize);
        Gl.glVertex2d(x_offset * backTileSize + outcroppingSize, y_offset * backTileSize);
        Gl.glVertex2d(x_offset * backTileSize + outcroppingSize, y_offset * backTileSize + outcroppingSize);
        Gl.glVertex2d(x_offset * backTileSize, y_offset * backTileSize + outcroppingSize);
        Gl.glEnd();
    }
    static void drawCharacter(characterObj PC)
    {
        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glPushMatrix();

        Gl.glTranslatef(PC.getX(), PC.getY(), PC.getZ());
        if (selectMode == true)
        {
            //Draw a cube with outline
            drawCube(PC.getColor());
            drawOutline(colorWhite);
        }
        else
            drawCube(PC.getColor());

        Gl.glPopMatrix();
    }
    static void resetTiles()
    {
        for (int i = 0; i < wallSize / tileSize; i++)
        {
            for (int j = 0; j < wallSize / tileSize; j++)
            {
                myTiles[i, j].setColor(colorBlack);
            }
        }
    }
    static void drawBackgroundButtons()
    {
        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(backgroundButtons[i].myX, backgroundButtons[i].myY, 0);
            drawTile(backgroundButtons[i].getColor());
            Gl.glPopMatrix();
        }
    }
    static void drawAxis3D()
    {
        Gl.glBegin(Gl.GL_LINES);
        //X AXIS
        Gl.glColor3d(1.0, 0, 0); //RED
        Gl.glVertex3i(form.Width, 0, 0);
        Gl.glVertex3i(form.Width, 0, 0);

        //Y AXIS
        Gl.glColor3d(0, 1.0, 0); //GREEN
        Gl.glVertex3i(0, 0, 0);
        Gl.glVertex3i(0, form.Height, 0);

        //Z AXIS
        Gl.glColor3d(0, 0, 1.0); //BLUE
        Gl.glVertex3i(0, 0, form.Width);
        Gl.glVertex3i(0, 0, form.Width);

        Gl.glEnd();
    }
    static void drawAxis2D()
    {
        Gl.glBegin(Gl.GL_LINES);
        //X AXIS
        Gl.glColor3d(1.0, 0, 0); //RED
        Gl.glVertex3i(-form.Width / 2, 0, 0);
        Gl.glVertex3i(form.Width / 2, 0, 0);

        //Y AXIS
        Gl.glColor3d(0, 1.0, 0); //GREEN
        Gl.glVertex3i(0, -form.Height, 0);
        Gl.glVertex3i(0, form.Height, 0);
        Gl.glEnd();
    }
    static void drawWall()
    {

        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        for (int i = 0; i < wallSize / tileSize; i++)
        {
            for (int j = 0; j < wallSize / tileSize; j++)
            {
                Gl.glPushMatrix();
                Gl.glTranslatef(myTiles[i, j].myX, myTiles[i, j].myY, 0);
                drawTile(myTiles[i, j].getColor());
                drawTileOutline(myTiles[i,j].getOutlineColor());
                Gl.glPopMatrix();
            }
        }

    }
    static void drawText()
    {
        Gl.glColor3fv(colorBlue);
        Gl.glRasterPos2f(-form.Width / 2 + 100, form.Height / 2 - 50);
        glPrint(string.Format("Background"));
    }
    static void recolorToSelection()
    {
        recolorWall(backgroundButtons[selectedButton].getColor());
    }
    static void recolorWall(float[] color)
    {
        for (int i = 0; i < number_of_tiles; i++)
        {
            for (int j = 0; j < number_of_tiles; j++)
                myTiles[i, j].setColor(color);
        }
    }

    static void setViewport(int myWidth, int myHeight)
    {       
        Gl.glViewport(0, 0, myWidth, myHeight);
    }
    static void setPerspective(float myWidth, float myHeight)
    {
        if (myHeight == 0)
            myHeight = 1;
        Glu.gluPerspective(90, (float)form.Width / form.Height, 0.1, 800);          // Calculate The Aspect Ratio Of The Window
    }
    #endregion

    #region FONT_STUFF
    private static void BuildFont()
    {
        IntPtr font;                                                        // Windows Font ID
        IntPtr oldfont;                                                     // Used For Good House Keeping
        fontbase = Gl.glGenLists(96);                                       // Storage For 96 Characters

        font = Gdi.CreateFont(                                              // Create The Font
            -24,                                                            // Height Of Font
            0,                                                              // Width Of Font
            0,                                                              // Angle Of Escapement
            0,                                                              // Orientation Angle
            Gdi.FW_BOLD,                                                    // Font Weight
            false,                                                          // Italic
            false,                                                          // Underline
            false,                                                          // Strikeout
            Gdi.ANSI_CHARSET,                                               // Character Set Identifier
            Gdi.OUT_TT_PRECIS,                                              // Output Precision
            Gdi.CLIP_DEFAULT_PRECIS,                                        // Clipping Precision
            Gdi.ANTIALIASED_QUALITY,                                        // Output Quality
            Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,                            // Family And Pitch
            "Courier New");                                                 // Font Name

        oldfont = Gdi.SelectObject(hDC, font);                              // Selects The Font We Want
        Wgl.wglUseFontBitmaps(hDC, 32, 96, fontbase);                       // Builds 96 Characters Starting At Character 32
        Gdi.SelectObject(hDC, oldfont);                                     // Selects The Font We Want
        Gdi.DeleteObject(font);                                             // Delete The Font
    }
    private static void KillFont()
    {
        Gl.glDeleteLists(fontbase, 96);                                     // Delete All 96 Characters
    }
    private static void glPrint(string text)
    {
        if (text == null || text.Length == 0)
        {                              // If There's No Text
            return;                                                         // Do Nothing
        }
        Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
            Gl.glListBase(fontbase - 32);                                   // Sets The Base Character to 32
            // .NET -- we can't just pass text, we need to convert
            byte[] textbytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++) 
                textbytes[i] = (byte)text[i];
            Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);        // Draws The Display List Text
        Gl.glPopAttrib();                                                   // Pops The Display List Bits
    }
    #endregion

    #region CHARACTER SELECTION AND MOVEMENT
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
            //drawSelectionInset(12*3,7*3);
            //stop = !stop;
            if (!selectTile(e))  //Tile was selected?
            {
                if (selectButton(e))    //Button was selected
                    recolorToSelection();
                else
                    recolorWall(colorRed);
            }
            else
                return;
        }
        //MessageBox.Show("X:" + e.X + "Y:" + e.Y);
        //MessageBox.Show(form.Width + "," + form.Height);
    }
    void mMove(object sender, MouseEventArgs e)
    {
        clickX = e.X;
        clickY = e.Y;
    }
    static void processHits(int hits, int[] buffer)
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
    static void selectCube(MouseEventArgs e)
    {
        int[] buffer = new int[512];
        int hits = 0;
        int[] viewport = new int[4];
        float aspectRatio = (float)form.Width / form.Height;
        float curLY = 0.5f, curLX = (float)curLY * aspectRatio;

        //CLEAR SCREEN
        Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

        //DRAW MODEL
        drawGlScene();

        #region CUBE SELECTION
        Gl.glSelectBuffer(512, buffer);
        Gl.glRenderMode(Gl.GL_SELECT);

        Gl.glInitNames();
        Gl.glPushName(0);

        //Save current state matrix
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPushMatrix();
        Gl.glLoadIdentity();

        //Set selection view to entire screen
        Gl.glViewport(0, 0, form.Width, form.Height);
        Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

        //Create picking matrix
        Glu.gluPickMatrix(e.X, form.Height - e.Y,
            curLX, curLY, viewport);
        setPerspective(curLX, curLY);

        Gl.glMatrixMode(Gl.GL_MODELVIEW);

        //Test PC for picking
        Gl.glLoadName(0);
        Gl.glPushMatrix();
        Gl.glTranslatef(PC.getX(), PC.getY(), 0.0f);
        drawCharacter(PC);
        Gl.glPopMatrix();

        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPopMatrix();
        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glFlush();

        hits = Gl.glRenderMode(Gl.GL_RENDER);
        if (hits > 0)
        {
            selectMode = true;
        }
        else
        {
            if (selectMode)
            {
                selectMode = false;
            }
        }
        processHits(hits, buffer);
        #endregion
    }
    //*********************************************************//
    //This function selects the tile at cursor location e.X,e.Y//
    //and sets selectedTile to the selected tile's index       // 
    //*********************************************************//
    static bool selectTile(MouseEventArgs e)
    {
        int[] buffer = new int[512];
        int hits = 0;
        int[] viewport = new int[4];
        float aspectRatio = (float)form.Width / form.Height;
        float curLY = (float)1, curLX = (float)curLY * aspectRatio;

        
        #region TILE SELECTION
        Gl.glSelectBuffer(512, buffer);
        Gl.glRenderMode(Gl.GL_SELECT);

        Gl.glInitNames();
        Gl.glPushName(0);

        //Save current state matrix
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPushMatrix();
        Gl.glLoadIdentity();

        //Set selection view to entire screen
        setPerspective(curLX, curLY);
        setViewport(width, height);
        Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

        //Create picking matrix
        Glu.gluPickMatrix(e.X, viewport[3] - e.Y,
            curLX, curLY, viewport);       

        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glPushMatrix();
        Gl.glLoadIdentity();
        Glu.gluLookAt(0, 0, form.Height / 2 / Math.Tan(45 * Math.PI / 180), 0, 0, 0, 0, 1, 0);

        //Test all the tiles for picking
        for (int i = 0; i < number_of_tiles; i++)
        {
            for (int j = 0; j < number_of_tiles; j++)
            {
                Gl.glLoadName(i * number_of_tiles + j);
                Gl.glPushMatrix();
                Gl.glTranslatef(myTiles[i, j].myX, myTiles[i, j].myY,
                    0);
                drawTile();
                Gl.glPopMatrix();
            }            
        }

        //Restore matrices
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPopMatrix();
        Gl.glMatrixMode(Gl.GL_MODELVIEW);        
        Gl.glFlush();
        Gl.glPopMatrix();

        hits = Gl.glRenderMode(Gl.GL_RENDER);
        //processHits(hits, buffer);
        if (hits > 0)
        {
            int[] index;
            if(selectedTile<number_of_tiles*number_of_tiles)    //Was there a tile selected before?
            {            //Reset the previous selected tile's outline to white
                index = tileIndeces(selectedTile);
                myTiles[index[0],index[1]].setOutlineColor(colorWhite);
            }

            //set outline of newly selected tile to black.            
            selectedTile = (int)buffer[3];
            index = tileIndeces(selectedTile);
            myTiles[index[0], index[1]].setOutlineColor(colorBlack);
            return true;
        }
        else
            return false;
        #endregion
    }
    static void drawSelectionInset(float vX, float vY)
    {
        float aspectRatio = (float)form.Width / form.Height;
        float insetHeight = 200, insetWidth = insetHeight * aspectRatio;
        float curLY = 0.5f, curLX = curLY * aspectRatio;
        int[] viewport = new int[4];     

        Gl.glRenderMode(Gl.GL_RENDER);        

        //Save current state matrices
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPushMatrix();
        Gl.glLoadIdentity();

        //Set inset and get new viewport
        Gl.glViewport(form.Width - (int)insetWidth, form.Height - (int)insetHeight, (int)insetWidth, (int)insetHeight);
        Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);        

        //Translate cursor position
        float curX = vX, curY = vY;
        //This translates cursor from the large viewport screen coordinates to the small one's
        float tCurX = insetWidth / form.Width * curX + form.Width - insetWidth;
        float tCurY = insetHeight / (form.Height) * curY;

        //This translates screen coords to small viewport coordinates
        float svCurX = tCurX - form.Width + insetWidth / 2;
        float svCurY = tCurY - insetHeight / 2;

        //Create picking matrix
        Glu.gluPickMatrix(tCurX, form.Height - tCurY,
            curLX, curLY, viewport);

        setPerspective(curLX, curLY);

        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glPushMatrix();
        Gl.glLoadIdentity();
        Glu.gluLookAt(0, 0, form.Height / 2 / Math.Tan(45 * Math.PI / 180), 0, 0, 0, 0, 1, 0);

        //Draw pick volume in inset window
        Gl.glBegin(Gl.GL_QUADS);
        Gl.glColor3fv(colorBlue);
        Gl.glVertex2d(-insetWidth / 2, -insetHeight / 2);
        Gl.glVertex2d(insetWidth / 2, -insetHeight / 2);
        Gl.glVertex2d(insetWidth / 2, insetHeight / 2);
        Gl.glVertex2d(-insetWidth / 2, insetHeight / 2);
        Gl.glEnd();
        drawWall();
        drawBackgroundButtons();
        


        //Restore matrices
        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glPopMatrix();
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPopMatrix();
        Gl.glFlush();
        /*
        Console.Write("Translated X:" + tCurX + ", Translated Y:" + (this.Height-tCurY) + "\n");
        Console.Write("Screen X:" + curX + ", Screen Y:" + curY + "\n");
        Console.Write("view X:" + svCurX +" view Y:" + svCurY + "\n");
         * */
    }
    static bool selectButton(MouseEventArgs e)
    {
        int[] buffer = new int[512];
        int hits = 0;
        int[] viewport = new int[4];
        float aspectRatio = (float)form.Width / form.Height;
        float curLY = 0.5f, curLX = (float)curLY * aspectRatio;

        //CLEAR SCREEN
        Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

        //DRAW MODEL
        drawGlScene();

        #region BUTTON SELECTION
        Gl.glSelectBuffer(512, buffer);
        Gl.glRenderMode(Gl.GL_SELECT);

        Gl.glInitNames();
        Gl.glPushName(0);

        //Save current state matrix
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPushMatrix();
        Gl.glLoadIdentity();

        //Set selection view to entire screen
        Gl.glViewport(0, 0, form.Width, form.Height);
        Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

        //Create picking matrix
        Glu.gluPickMatrix(e.X, form.Height - e.Y,
            curLX, curLY, viewport);
        setPerspective(curLX, curLY);

        Gl.glMatrixMode(Gl.GL_MODELVIEW);

        //Test all the buttons for picking
        for (int i = 0; i < 5; i++)
        {            
                Gl.glLoadName(i);
                Gl.glPushMatrix();
                Gl.glTranslatef(backgroundButtons[i].myX, backgroundButtons[i].myY, 0);
                drawTile();
                Gl.glPopMatrix();
        }

        //Restore matrices
        Gl.glMatrixMode(Gl.GL_PROJECTION);
        Gl.glPopMatrix();
        Gl.glMatrixMode(Gl.GL_MODELVIEW);
        Gl.glFlush();

        hits = Gl.glRenderMode(Gl.GL_RENDER);
        if (hits > 0)
        {
            selectedButton = (int)buffer[3];
            processHits(hits, buffer);
            return true;
        }
        else
            return false;
        #endregion BUTTON SELECTION
    }

    static void moveChar(int destX, int destY)
    {
        PC.setX(destX);
        PC.setY(destY);
        resetTiles();
    }
    #endregion

    #region ACCESSORS
    /* Takes tile coordinates (in map coordinates) and
     * returns tile coordinates in tile list */
    static int[] tileIndeces(float xPos, float yPos)
    {
        int[] indeces = new int[2];
        indeces[0] = (int)(xPos + wallSize / 2) / tileSize;
        indeces[1] = (int)(yPos + wallSize / 2) / tileSize;
        return indeces;
    }
    /* Takes number of tile in tile list and returns indices of 
    *  tile in tile array */
    static int[] tileIndeces(int tileNumber)
    {
        //The tile numbers are on a one-Dim., starting from far left and going forward
        //then right through the list, so we need to mod to get the y-coord and to divide to get the x-coord.
        int[] Location = new int[2];//X,Y
        Location[1] = tileNumber % (wallSize / tileSize);
        Location[0] = (tileNumber - Location[1]) / (wallSize / tileSize);
        return Location;
    }
    /*Return false if there is a cube on  
    * tile "myTiles[ii,ij]", or if it indices fall outside map, true otherwise */
    static bool emptyTile(int ii, int ij)
    {
        if (PC.getX() == myTiles[ii, ij].myX && PC.getY() == myTiles[ii, ij].myY && PC.getZ() == myTiles[ii, ij].myZ)
            return false;

        if (ii < 0 || ii < wallSize / (tileSize) || ij < 0 || ij < wallSize / (tileSize))
            return false;

        return true;
    }
    #endregion

    /*-------------------- FORM STUFF ---------------------------*/
    #region Form_Closing(object sender, CancelEventArgs e)
    /// <summary>
    ///     Handles the form's closing event.
    /// </summary>
    /// <param name="sender">
    ///     The event sender.
    /// </param>
    /// <param name="e">
    ///     The event arguments.
    /// </param>
    private void Form_Closing(object sender, CancelEventArgs e)
    {
        finished = true;                                                        // Send A Quit Message
    }
    #endregion Form_Closing(object sender, CancelEventArgs e)

    
}

public class characterObj
{
    private int myX;
    private int myY;
    private int myZ;
    private int[] myLocation;
    private float[] myColor = { 1.0f, 1.0f, 1.0f };


    public characterObj(int x, int y, int z, float[] color)
    {
        myX = x; myY = y; myZ = z; myLocation = new int[] { myX, myY, myZ }; myColor = color;
    }
    public characterObj()
    {
        myX = 0; myY = 0; myZ = 0; myLocation = new int[] { myX, myY, myZ }; myColor = new float[] { 0.0f, 0.0f, 0.0f };
    }

    public void setX(int XCoord)
    {
        myX = XCoord;
        myLocation[0] = myX;
    }
    public int getX()
    {
        return myX;
    }
    public void setY(int YCoord)
    {
        myY = YCoord;
        myLocation[1] = myY;
    }
    public int getY()
    {
        return myY;
    }
    public void setZ(int ZCoord)
    {
        myZ = ZCoord;
        myLocation[2] = myZ;
    }
    public int getZ()
    {
        return myZ;
    }
    public void setLocation(int[] Location)
    {
        myLocation = Location;
    }
    public int[] getLocation()
    {
        return myLocation;
    }
    public void setColor(float[] color)
    {
        myColor = color;
    }
    public float[] getColor()
    {
        return myColor;
    }



}

public class tileObj
{
    public int myX = 0;
    public int myY = 0;
    public int myZ = 0;
    private float[] myColor;
    private float[] myOutlineColor;

    public tileObj(int myX, int myY, int myZ, float[] color,float[] outlineColor)
    {
        this.myX = myX;
        this.myY = myY;
        this.myZ = myZ;
        myColor = color;
        myOutlineColor = outlineColor;

    }
    public tileObj()
    {
        myColor = new float[] { 0.0f, 0.0f, 0.0f };
    }
    public void setColor(float[] color)
    {
        myColor = color;
    }
    public float[] getColor()
    {
        return myColor;
    }
    public void setOutlineColor(float[] color)
    {
        myOutlineColor = color;
    }
    public float[] getOutlineColor()
    {
        return myOutlineColor;
    }
}
