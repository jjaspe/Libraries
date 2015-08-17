using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Open_GL_Template;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Basic_Setup;

namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    public partial  class ReadyOpenGlTemplate:BasicOpenGlTemplate,ICanvasWindow
    {
        OpenGLSelector mySelector;
        public simpleOpenGlView MyView
        {
            get { return simpleOpenGlView1; }
        }
        public Navigator MyNavigator
        {
            get { return this.navigator1; }
        }
        public IWorld MyWorld { get; set; }

        public int clickX { get; set; }
        public int clickY { get; set; }

        public ReadyOpenGlTemplate()
            :base()
        {
            InitializeComponent();
            mySelector = new OpenGLSelector(this.MyView);
            MyWorld = new World();
            MyView.InitializeContexts();
            this.MyView.MouseClick += new MouseEventHandler(viewClick);
            this.MyView.Dock = DockStyle.None;

            MyNavigator.Orientation = OpenGLDrawer.planeOrientation.Z;
            MyNavigator.MyWindowOwner = this;
            MyNavigator.MyView = this.MyView;
            MyNavigator.Parent = MyView;
            MyNavigator.Dock = DockStyle.Right;

            this.WindowState = FormWindowState.Maximized;
        }

        public void drawingLoop()
        {
            OpenGLDrawer myDrawer = new OpenGLDrawer();
            MyView.setCameraView(simpleOpenGlView.VIEWS.Iso);
            
            while (!MyView.isDisposed())
            {
                MyView.setupScene();
                //DRAW SCENE HERE
                myDrawer.drawWorld(MyWorld);
                //END DRAW SCENE HERE
                MyView.flushScene();
                this.Refresh();
                Application.DoEvents();
            }
        }

        #region DRAWING FUNCTIONS
        
        #endregion

        #region MISC
        public void start()
        {
            InitializeComponent();
            MyView.InitializeContexts();
            this.MyView.MouseClick += new MouseEventHandler(mClick); 
            this.MyView.Dock = DockStyle.None;
            this.Show();
        }
        public void end()
        {            
            if(MyView!=null)
                MyView.Dispose();
        }
        public string getName()
        {
            return Name;
        }
        public void enabled(bool value)
        {
            this.Enabled = value;
        }
        #endregion

        
        #region HANDLERS
       
        void mDown(object sender, MouseEventArgs e)
        {
            clickX = e.X;
            clickY = e.Y;
        }
        void mClick(object sender, MouseEventArgs e)
        {



        }
        void mMove(object sender, MouseEventArgs e)
        {
            clickX = e.X;
            clickY = e.Y;
        }

        public virtual void viewClick(object sender, MouseEventArgs e)
        {
            //Check all objects, see if any was selected
            int id = mySelector.getSelectedObjectId(new int[] { e.X, e.Y }, MyWorld);
        }
        public virtual void zoomInButton_Click(object sender, EventArgs e)
        {
            this.MyView.zoomIn();
        }

        public virtual void zoomOutButton_Click(object sender, EventArgs e)
        {
            MyView.zoomOut();
            
        }

        public virtual void myView_Load(object sender, EventArgs e)
        {

        }

        public virtual void eyeFrontMenuItem_Click(object sender, EventArgs e)
        {
            MyView.setCameraView(MyView.eyeFront);
        }

        public virtual void eyeTopMenuItem_Click(object sender, EventArgs e)
        {
            MyView.setCameraView(MyView.eyeTop);
        }

        public virtual void cornerMenuItem_Click(object sender, EventArgs e)
        {
            MyView.setCameraView(MyView.eyeCorner);
        }

        public virtual void isometricMenuItem_Click_1(object sender, EventArgs e)
        {
            MyView.setCameraView(MyView.eyeIso);
        }

        public virtual void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyView.setCameraView(MyView.eye2D);
        }
        
        public virtual void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            


        }

        public virtual void MapCreation_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        public virtual void rotateCW_Click(object sender, EventArgs e)
        {
            MyView.rotateCW();
        }
        public virtual void rotateCCW_Click(object sender, EventArgs e)
        {
            MyView.rotateCCW();
        }
        #endregion

        public simpleOpenGlView getView()
        {
            return MyView;
        }

        public IWorld getMap()
        {
            return MyWorld;
        }

        public void refresh()
        {
            base.Refresh();
        }

        
    }
}
