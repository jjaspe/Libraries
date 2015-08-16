using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ISE;
using Tao.FreeGlut;
using Tao.Platform.Windows;
using Tao.OpenGl;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template.Interfaces;

namespace Open_GL_Template
{
    public class BasicOpenGlTemplate : Form
    {

        /*public simpleOpenGlView MyView
        {
            get { return myView; }
        }*/

        public BasicOpenGlTemplate()
        {
            Text = "A Drawing Template";	// Application Window Text       
            this.MinimizeBox = false;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BasicOpenGlTemplate
            // 
            this.ClientSize = new System.Drawing.Size(1274, 735);
            this.MinimizeBox = false;
            this.Name = "BasicOpenGlTemplate";
            this.ResumeLayout(false);

        }

        /*USE THIS FORMAT FROM CLIENT TO USE THIS TEMPLATE */
        public static void tempMain()
        {
            /*BasicOpenGlTemplate templateForm = new BasicOpenGlTemplate();
            templateForm.WindowState = FormWindowState.Maximized;
            templateForm.MinimizeBox = false;
            templateForm.MaximizeBox = false;            
            
            while (!templateForm.myView.IsDisposed)
            {
                templateForm.myView.setupScene();
                //DRAW SCENE HERE
                //END DRAW SCENE HERE
                templateForm.myView.flushScene();
                templateForm.Refresh();
                Application.DoEvents();
            }
             * 

            templateForm.Dispose();
             * */
        }
    }
}
