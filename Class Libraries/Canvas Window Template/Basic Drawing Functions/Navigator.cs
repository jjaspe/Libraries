using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Canvas_Window_Template.Basic_Drawing_Functions;
using Canvas_Window_Template;
using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template
{
    public partial class Navigator : UserControl
    {
        simpleOpenGlView myView;
        ICanvasWindow myWindowOwner;
        Common.planeOrientation orientation;

        public Common.planeOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public ICanvasWindow MyWindowOwner
        {
            get { return myWindowOwner; }
            set { myWindowOwner = value; }
        }

        public simpleOpenGlView MyView
        {
            get { return myView; }
            set { myView = value; }
        }
        public Navigator()
        {
            InitializeComponent();
        }
        public Navigator(simpleOpenGlView view)
        {
            myView = view;
            InitializeComponent();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            moveRight();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            moveLeft();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            moveUp();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            moveDown();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            MyView.zoomIn();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            MyView.zoomOut();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        void moveUp()
        {
            switch (orientation)
            {
                case Common.planeOrientation.X://Y is left right,Z is up down
                    myView.moveZUp();
                    break;
                case Common.planeOrientation.Z://Y is up down, X is left right
                    myView.moveYUp();
                    break;
                case Common.planeOrientation.Y://Z is up down, X is left right
                    myView.moveZUp();
                    break;
                default://Z is up down, X is left right
                    myView.moveZUp();
                    break;
            }
        }
        void moveDown()
        {
            switch (orientation)
            {
                case Common.planeOrientation.X://Y is left right,Z is up down
                    myView.moveZDown();
                    break;
                case Common.planeOrientation.Z://Y is up down, X is left right
                    myView.moveYDown();
                    break;
                case Common.planeOrientation.Y://Z is up down, X is left right
                    myView.moveZDown();
                    break;
                default://Z is up down, X is left right
                    myView.moveZDown();
                    break;
            }
        }
        void moveLeft()
        {
            switch (orientation)
            {
                case Common.planeOrientation.X://Y is left right,Z is up down
                    myView.moveYDown();
                    break;
                case Common.planeOrientation.Z://Y is up down, X is left right
                    myView.moveXDown();
                    break;
                case Common.planeOrientation.Y://Z is up down, X is left right
                    myView.moveXDown();
                    break;
                default://Z is up down, X is left right
                    myView.moveXDown();
                    break;
            }
        }
        void moveRight()
        {
            switch (orientation)
            {
                case Common.planeOrientation.X://Y is left right,Z is up down
                    myView.moveYUp();
                    break;
                case Common.planeOrientation.Z://Y is up down, X is left right
                    myView.moveXUp();
                    break;
                case Common.planeOrientation.Y://Z is up down, X is left right
                    myView.moveXUp();
                    break;
                default://Z is up down, X is left right
                    myView.moveXUp();
                    break;
            }
        }

        private void btnRotateDown_Click(object sender, EventArgs e)
        {
            myView.rotateCCW();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }

        private void btnRotateUp_Click(object sender, EventArgs e)
        {
            myView.rotateCW();
            if (myWindowOwner != null)
                myWindowOwner.refresh();
        }
    }
}
