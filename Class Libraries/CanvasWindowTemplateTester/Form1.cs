using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canvas_Window_Template.Basic_Drawing_Functions;

namespace CanvasWindowTemplateTester
{
    public partial class Form1 : ReadyOpenGlTemplate
    {
        public Form1()
            :base()
        {
            InitializeComponent();
            //this.MyWorld.add(new cubeObj(new pointObj(0, 0, 0), 30, null, null));
            this.Show();
        }
    }
}
