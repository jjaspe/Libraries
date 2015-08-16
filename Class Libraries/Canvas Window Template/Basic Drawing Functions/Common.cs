using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISE;
using Tao.FreeGlut;
using Tao.Platform.Windows;
using Tao.OpenGl;
using System.Windows.Forms;
using Canvas_Window_Template.Interfaces;
using Canvas_Window_Template.Drawables;
using System.Drawing;

namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    /// <summary>
    /// Use this class to draw primitives (tiles,cubes,walls, etc).
    /// However, the intended use is to create an IWorld implementation, add drawables to it (tiles,cubes,walls) and 
    /// call draw on the IWorld;
    /// </summary>
    public class Common
    {
        public enum tileSide { None, Top, Right, Bottom, Left };
        public static FTFont myFont ;
        //COLORS
        public enum planeOrientation {None=0,X=1,Y=2,Z=3};
        public static float[] colorBlack = { 0.0f, 0.0f, 0.0f }, colorWhite = { 1.0f, 1.0f, 1.0f }, colorBlue = { 0.0f, 0.0f, 1.0f };
        public static float[] colorRed = { 1.0f, 0.0f, 0.0f }, colorOrange = { 1.0f, 0.5f, 0.25f }, colorYellow = { 1.0f, 1.0f, 0.0f };
        public static float[] colorGreen = { 0.0f, 1.0f, 0.2f }, colorBrown = { 0.4f, 0.5f, 0.3f };

        //const int tileSize = 24;
        //const int backTileSize = 10;
        //const int number_of_tiles = 10;
        //const int wallSize = number_of_tiles * tileSize;       

        /// <summary>
        /// fontResource holds location of font
        /// </summary>
        /// <param name="fontResource"></param>
        public static void initializeFont(string fontResource)
        {
            try
            {               
                int Errors = 0;
                // CREATE FONT
                Common.myFont = new FTFont(fontResource, out Errors);
                // INITIALISE FONT AS A PER_CHARACTER TEXTURE MAPPED FONT
                Common.myFont.ftRenderToTexture(24, 196);

                Common.myFont.FT_ALIGN = FTFontAlign.FT_ALIGN_CENTERED;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }


        #region DRAW FUNCTIONS
        public static float[] colorToArray(Color color)
        {
            return new float[] { ((float)color.R)/256, ((float)color.G)/256, ((float)color.B)/256 };
        }
        public static void translate(IPoint position)
        {
            Gl.glTranslated(position.X, position.Y, position.Z);
        }
        public static void rotate(double angle)
        {
            Gl.glRotated(angle, 0, 0, 1);
        }
        public static void rotate(double angle,IPoint endpoint)
        {
            Gl.glRotated(angle, endpoint.X, endpoint.Y, endpoint.Z);
        }

        public static void drawLine(IPoint p1, IPoint p2)
        {
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3fv(colorBlack);

            Gl.glVertex3d(p1.X, p1.Y, p1.Z);
            Gl.glVertex3d(p2.X, p2.Y, p2.Z);

            Gl.glEnd();
        }
        public static void drawLine(IPoint p1, IPoint p2,float[] color)
        {
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3fv(color);

            Gl.glVertex3d(p1.X, p1.Y, p1.Z);
            Gl.glVertex3d(p2.X, p2.Y, p2.Z);

            Gl.glEnd();
        }

        public static void drawTile(tileObj tile)
        {
            int tileOrientation=0;// 1 for perp to X axis, 2 for Y, 3 for Z
            if(tile.MyOrigin.X==tile.MyEnd.X)
                tileOrientation=1;
            else if(tile.MyOrigin.Y==tile.MyEnd.Y)
                tileOrientation=2;
            else if(tile.MyOrigin.Z==tile.MyEnd.Z)
                tileOrientation=3;

            Gl.glPushMatrix();           

           
            //Get Center point
            IPoint center;
            switch (tileOrientation)
            {
                case 1: //Perp to X
                    {
                        center = new pointObj(tile.MyOrigin.X, tile.MyOrigin.Y + tile.TileSize / 2,
                            tile.MyOrigin.Z + tile.TileSize / 2);
                        translate(center);
                        rotate(tile.angle * 45);
                        Gl.glBegin(Gl.GL_QUADS);
                        Gl.glColor3fv(tile.getColor());
                        Gl.glVertex3d(0, -tile.TileSize / 2, -tile.TileSize / 2);
                        Gl.glVertex3d(0, -tile.TileSize / 2, tile.TileSize / 2);
                        Gl.glVertex3d(0, tile.TileSize / 2, tile.TileSize / 2);
                        Gl.glVertex3d(0, tile.TileSize / 2, -tile.TileSize / 2);
                        
                    }
                    break;
                case 2://Perp to Y
                    {
                        center = new pointObj(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y,
                            tile.MyOrigin.Z + tile.TileSize / 2);
                        translate(center);
                        rotate(tile.angle * 45);
                        Gl.glBegin(Gl.GL_QUADS);
                        Gl.glColor3fv(tile.getColor());
                        Gl.glVertex3d(-tile.TileSize / 2, 0, -tile.TileSize / 2);
                        Gl.glVertex3d(tile.TileSize / 2, 0, -tile.TileSize / 2);
                        Gl.glVertex3d(tile.TileSize / 2, 0, tile.TileSize / 2);
                        Gl.glVertex3d(-tile.TileSize / 2, 0, tile.TileSize / 2);
                    }
                    break;
                case 3://Perp to Z
                    {
                        center = new pointObj(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y + tile.TileSize / 2,
                           tile.MyOrigin.Z);
                        translate(center);
                        rotate(tile.angle * 45);
                        Gl.glBegin(Gl.GL_QUADS);
                        Gl.glColor3fv(tile.getColor());
                        Gl.glVertex3d(-tile.TileSize / 2, -tile.TileSize / 2, 0);
                        Gl.glVertex3d(tile.TileSize / 2, -tile.TileSize / 2, 0);
                        Gl.glVertex3d(tile.TileSize / 2, tile.TileSize / 2, 0);
                        Gl.glVertex3d(-tile.TileSize / 2, tile.TileSize / 2, 0);
                    }
                    break;
            }
            #region ORIGINAL SWITCH
            /*
            switch(tileOrientation)
            {
                case 1: //Perp to X
                    {
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyEnd.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyEnd.Y, tile.MyOrigin.Z);
                    }
                    break;
                case 2://Perp to Y
                    {
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyEnd.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyEnd.Z);
                    }
                    break;
                case 3://Perp to Z
                     {
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyEnd.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyEnd.Y, tile.MyOrigin.Z);
                     }
                     break;
            }*/
            #endregion
            Gl.glEnd();
            Gl.glPopMatrix();

        }
        public static void drawTileOutline(tileObj tile)
        {
            int tileOrientation=0;// 1 for perp to X axis, 2 for Y, 3 for Z
            if(tile.MyOrigin.X==tile.MyEnd.X)
                tileOrientation=1;
            else if(tile.MyOrigin.Y==tile.MyEnd.Y)
                tileOrientation=2;
            else if(tile.MyOrigin.Z==tile.MyEnd.Z)
                tileOrientation=3;

            Gl.glPushMatrix();
            
            
            IPoint center;
            switch (tileOrientation)
            {
                case 1: //Perp to X
                    {
                        center = new pointObj(tile.MyOrigin.X, tile.MyOrigin.Y + tile.TileSize / 2,
                            tile.MyOrigin.Z + tile.TileSize / 2);
                        translate(center);
                        rotate(tile.angle * 45);
                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glColor3fv(tile.getOutlineColor());
                        Gl.glVertex3d(0, -tile.TileSize / 2, -tile.TileSize / 2);
                        Gl.glVertex3d(0, -tile.TileSize / 2, tile.TileSize / 2);
                        Gl.glVertex3d(0, tile.TileSize / 2, tile.TileSize / 2);
                        Gl.glVertex3d(0, tile.TileSize / 2, -tile.TileSize / 2);
                    }
                    break;
                case 2://Perp to Y
                    {
                        center = new pointObj(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y,
                            tile.MyOrigin.Z + tile.TileSize / 2);
                        translate(center);
                        rotate(tile.angle * 45);
                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glColor3fv(tile.getOutlineColor());
                        Gl.glVertex3d(-tile.TileSize / 2, 0, -tile.TileSize / 2);
                        Gl.glVertex3d(tile.TileSize / 2, 0, -tile.TileSize / 2);
                        Gl.glVertex3d(tile.TileSize / 2, 0, tile.TileSize / 2);
                        Gl.glVertex3d(-tile.TileSize / 2, 0, tile.TileSize / 2);
                    }
                    break;
                case 3://Perp to Z
                    {
                        center = new pointObj(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y + tile.TileSize / 2,
                           tile.MyOrigin.Z);                        
                        translate(center);
                        rotate(tile.angle * 45);
                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glColor3fv(tile.getOutlineColor());
                        Gl.glVertex3d(-tile.TileSize / 2, -tile.TileSize / 2, 0);
                        Gl.glVertex3d(tile.TileSize / 2, -tile.TileSize / 2, 0);
                        Gl.glVertex3d(tile.TileSize / 2, tile.TileSize / 2, 0);
                        Gl.glVertex3d(-tile.TileSize / 2, tile.TileSize / 2, 0);
                    }
                    break;
            }
            #region ORIGINAL SWITCH
            /*
            switch (tileOrientation)
            {
                case 1:
                    {
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyEnd.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyEnd.Y, tile.MyOrigin.Z);
                    }
                    break;
                case 2:
                    {
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyEnd.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyEnd.Z);
                    }
                    break;
                case 3:
                     {
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyOrigin.Y, tile.MyOrigin.Z);
                        Gl.glVertex3d(tile.MyEnd.X, tile.MyEnd.Y, tile.MyEnd.Z);
                        Gl.glVertex3d(tile.MyOrigin.X, tile.MyEnd.Y, tile.MyOrigin.Z);
                     }
                     break;
            }*/
            #endregion
            Gl.glEnd();
            Gl.glPopMatrix();
        }
        public static void drawTileAndOutline(tileObj tile)
        {
            drawTile(tile);
            drawTileOutline(tile);
        }
        

        public static void drawRectangle(rectangleObj rect)
        {
            if(rect.Orientation1!=Common.planeOrientation.None)
            {
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glColor3fv(rect.MyColor);

                Gl.glVertex3d(rect.BottomLeft.X, rect.BottomLeft.Y, rect.BottomLeft.Z);
                Gl.glVertex3d(rect.BottomRight.X, rect.BottomRight.Y, rect.BottomRight.Z);
                Gl.glVertex3d(rect.TopRight.X, rect.TopRight.Y, rect.TopRight.Z);
                Gl.glVertex3d(rect.TopLeft.X, rect.TopLeft.Y, rect.TopLeft.Z);

                Gl.glEnd();
            }
        }
        public static void drawRectangleOutline(rectangleObj rect)
        {
            if(rect.Orientation1!=Common.planeOrientation.None)
            {
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor3fv(rect.MyOutlineColor);

                Gl.glVertex3d(rect.BottomLeft.X, rect.BottomLeft.Y, rect.BottomLeft.Z);
                Gl.glVertex3d(rect.BottomRight.X, rect.BottomRight.Y, rect.BottomRight.Z);
                Gl.glVertex3d(rect.TopRight.X, rect.TopRight.Y, rect.TopRight.Z);
                Gl.glVertex3d(rect.TopLeft.X, rect.TopLeft.Y, rect.TopLeft.Z);

                Gl.glEnd();
            }
        }
        public static void drawRectangleAndOutline(rectangleObj rect)
        {
            drawRectangle(rect);
            drawRectangleOutline(rect);
        }

        public static void drawRombus(rombusObj rombus)
        {
            int rombusOrientation = 0;// 1 for perp to X axis, 2 for Y, 3 for Z
            if (rombus.MyOrigin.X == rombus.MyEnd.X)
                rombusOrientation = 1;
            else if (rombus.MyOrigin.Y == rombus.MyEnd.Y)
                rombusOrientation = 2;
            else if (rombus.MyOrigin.Z == rombus.MyEnd.Z)
                rombusOrientation = 3;

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3fv(rombus.getColor());

            switch (rombusOrientation)
            {
                case 1: //Perp to X
                    {
                        Gl.glRotated(45 * Math.PI / 180, 1, 0, 0);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyEnd.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyEnd.Y, rombus.MyOrigin.Z);
                        Gl.glRotated(-45 * Math.PI / 180, 1, 0, 0);
                    }
                    break;
                case 2://Perp to Y
                    {
                        Gl.glRotated(45 * Math.PI / 180, 0, 1, 0);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyEnd.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyEnd.Z);
                        Gl.glRotated(-45 * Math.PI / 180, 0, 1, 0);
                    }
                    break;
                case 3://Perp to Z
                    {
                        Gl.glRotated(45 * Math.PI / 180, 0, 0, 1);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyEnd.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyEnd.Y, rombus.MyOrigin.Z);
                        Gl.glRotated(-45 * Math.PI / 180, 0, 0, 1);
                    }
                    break;
            }
            Gl.glEnd();
        }
        public static void drawRombusOutline(rombusObj rombus)
        {
            int rombusOrientation = 0;// 1 for perp to X axis, 2 for Y, 3 for Z
            if (rombus.MyOrigin.X == rombus.MyEnd.X)
                rombusOrientation = 1;
            else if (rombus.MyOrigin.Y == rombus.MyEnd.Y)
                rombusOrientation = 2;
            else if (rombus.MyOrigin.Z == rombus.MyEnd.Z)
                rombusOrientation = 3;

            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glColor3fv(rombus.getOutlineColor());


            switch (rombusOrientation)
            {
                case 1:
                    {
                        Gl.glRotated(45 * Math.PI / 180, 1, 0, 0);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyEnd.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyEnd.Y, rombus.MyOrigin.Z);
                        Gl.glRotated(-45 * Math.PI / 180, 1, 0, 0);
                    }
                    break;
                case 2:
                    {
                        Gl.glRotated(45 * Math.PI / 180, 0, 1, 0);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyEnd.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyEnd.Z);
                        Gl.glRotated(-45 * Math.PI / 180, 0, 1, 0);
                    }
                    break;
                case 3:
                    {
                        Gl.glRotated(45 * Math.PI / 180, 0, 0, 1);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyOrigin.Y, rombus.MyOrigin.Z);
                        Gl.glVertex3d(rombus.MyEnd.X, rombus.MyEnd.Y, rombus.MyEnd.Z);
                        Gl.glVertex3d(rombus.MyOrigin.X, rombus.MyEnd.Y, rombus.MyOrigin.Z);
                        Gl.glRotated(-45 * Math.PI / 180, 0, 0, 1);
                    }
                    break;
            }
            Gl.glEnd();
        }
        public static void drawRombusAndOutline(rombusObj rombus)
        {
            drawRombus(rombus);
            drawRombusOutline(rombus);
        }

        public static void drawCubeOutline(cubeObj cube)
        {
            //Draw Faces
            Gl.glPushMatrix();
            translate(cube.RotationAxis);
            rotate(cube.Angle * 45);
            translate(new pointObj(-cube.RotationAxis.X, -cube.RotationAxis.Y, 0));
            drawTileOutline(cube.TileFront);
            drawTileOutline(cube.TileBack);
            drawTileOutline(cube.TileLeft);
            drawTileOutline(cube.TileRight);
            drawTileOutline(cube.TileBottom);
            drawTileOutline(cube.TileTop);  
            Gl.glPopMatrix();
        }
        public static void drawCube(cubeObj cube)
        {
            //Draw Faces
            Gl.glPushMatrix();
            translate(cube.RotationAxis);
            rotate(cube.Angle * 45);
            translate(new pointObj(-cube.RotationAxis.X, -cube.RotationAxis.Y, 0));
            drawTile(cube.TileFront);
            drawTile(cube.TileBack);
            drawTile(cube.TileLeft);
            drawTile(cube.TileRight);
            drawTile(cube.TileBottom);
            drawTile(cube.TileTop);
            Gl.glPopMatrix();
        }
        public static void drawCubeAndOutline(cubeObj cube)
        {
            //Draw Faces
            Gl.glPushMatrix();
            foreach(Rotation rotation in cube.Rotations)
            {
                double x = rotation.Axis.P2.X - rotation.Axis.P1.X,
                    y = rotation.Axis.P2.Y - rotation.Axis.P1.Y,
                    z = rotation.Axis.P2.Z - rotation.Axis.P1.Z;
                translate(rotation.Axis.P1);
                rotate(rotation.Degrees,new pointObj(x,y,z));
                translate(new pointObj(-rotation.Axis.P1.X, -rotation.Axis.P1.Y, -rotation.Axis.P1.Z));
            }
            //translate(cube.RotationAxis);
            //rotate(cube.Angle * 45);
            //translate(new pointObj(-cube.RotationAxis.X, -cube.RotationAxis.Y, 0));
            drawTileAndOutline(cube.TileFront);
            drawTileAndOutline(cube.TileBack);
            drawTileAndOutline(cube.TileLeft);
            drawTileAndOutline(cube.TileRight);
            drawTileAndOutline(cube.TileBottom);
            drawTileAndOutline(cube.TileTop);
            Gl.glPopMatrix();
        }

        public static void drawParallepiped(parallepiped p)
        {
            //Draw Faces
            Gl.glPushMatrix();
            translate(p.RotationAxis);
            rotate(p.Angle * 45);
            translate(new pointObj(-p.RotationAxis.X, -p.RotationAxis.Y, 0));
            drawRectangleAndOutline(p.TileBottom);
            drawRectangleAndOutline(p.TileBack);
            drawRectangleAndOutline(p.TileFront);            
            drawRectangleAndOutline(p.TileLeft);
            drawRectangleAndOutline(p.TileRight);            
            drawRectangleAndOutline(p.TileTop);
            
            Gl.glPopMatrix();
        }

        public static void drawCircleOutline(circleObj circle)
        {
            for (int i = 0; i < 180; i++)
            {
                
                Gl.glPushMatrix();
                Gl.glTranslated(circle.Center.X, circle.Center.Y, 0);
                Gl.glRotated(i, 0, 0, 1);

                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3fv(circle.OutlineColor);
                Gl.glVertex3d(circle.Radius, 0,0);
                Gl.glVertex3d(-circle.Radius, 0,0);
                Gl.glEnd();

                Gl.glPopMatrix();
                
            }
        }
        public static void drawCircle(circleObj circle)
        {
            Gl.glPushMatrix();
            Gl.glTranslated(circle.Center.X, circle.Center.Y, 0);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glColor3fv(circle.MyColor);

            Gl.glVertex3d(0, 0, 0);
            for (int i = 0; i < circle.vertices.Length/3; i++)
                Gl.glVertex3d((double)circle.vertices[i,0], (double)circle.vertices[i,1], (double)circle.vertices[i,2]);
            Gl.glEnd();
            Gl.glPopMatrix();

            /*
            for (int i = 0; i < 180; i++)
            {
                Gl.glPushMatrix();
                Gl.glTranslated(circle.Center.X, circle.Center.Y, 0);
                Gl.glRotated(i, 0, 0, 1);
                
                drawLine(new pointObj(circle.Radius, 0,0)
                    ,new pointObj(- circle.Radius,0, 0),circle.MyColor);
                Gl.glPopMatrix();
            }*/
        }
        //OFFSETS WILL BE TREATED AS REAL COORDINATES, NOT MULTIPLES OF TILESIZES OR ANY OTHER CONSTANT!!
        public static void drawSquare(int x_offset, int y_offset, double[] color)
        {
            /*Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3fv(color);
            Gl.glVertex2d(x_offset, y_offset);
            Gl.glVertex2d(x_offset + tileSize, y_offset);
            Gl.glVertex2d(x_offset + tileSize, y_offset + tileSize);
            Gl.glVertex2d(x_offset, y_offset + tileSize);
            Gl.glEnd();*/
        }
        public static void staticDrawWall(wallObj myWall)
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            for (int i = 0; i < myWall.MyHeight; i++)
            {
                for (int j = 0; j < myWall.MyWidth; j++)
                {
                    drawTile(myWall.MyTiles[i, j]);
                    drawTileOutline(myWall.MyTiles[i, j]);
                }
            }

        }

        public static Tile[,] createTileArray(int width, int height, tileObj[,] tiles, float[] color, double tileSize)
        {
            Tile[,] tileArray = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tileArray[i, j] = new Tile(tiles[i, j].MyOrigin, tiles[i, j].MyEnd);
                    tileArray[i, j].setColor(color);
                    tileArray[i, j].TileSize = tileSize;
                    ((Tile)tileArray[i, j]).OriginalColor = tileArray[i, j].MyColor;
                }
            }
            return tileArray;
        }

        /*
        void resetTiles()
        {
            for (int i = 0; i < wallSize / tileSize; i++)
            {
                for (int j = 0; j < wallSize / tileSize; j++)
                {
                    myTiles[i, j].setColor(colorBlack);
                }
            }
        }
        void drawBackgroundButtons()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            for (int i = 0; i < backgroundButtons.Length; i++)
            {
                Gl.glPushMatrix();
                Gl.glTranslated(backgroundButtons[i].myX, backgroundButtons[i].myY, 0);
                drawTile(backgroundButtons[i].getColor());
                Gl.glPopMatrix();
            }
        }
        void drawOCButtons()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            for (int i = 0; i < OCButtons.Length; i++)
            {
                Gl.glPushMatrix();
                Gl.glTranslated(OCButtons[i].myX, OCButtons[i].myY, 0);
                drawTile(OCButtons[i].getColor());
                Gl.glPopMatrix();
            }
        }*/
        public void drawWall(wallObj myWall)
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            for (int i = 0; i < myWall.MyHeight; i++)
            {
                for (int j = 0; j < myWall.MyWidth; j++)
                {                    
                    drawTile(myWall.MyTiles[i, j]);
                    drawTileOutline(myWall.MyTiles[i, j]);
                }
            }

        }
        public void drawWorld(IWorld world)
        {
            if (world != null && world.getEntities()!=null)
            {
                foreach (IDrawable drw in world.getEntities())
                    drw.draw();
            }
        }
        public static void drawText(double x,double y)
        {            
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            myFont.ftBeginFont();
            Gl.glColor3fv(colorBlue);
            Gl.glPushMatrix();
            Gl.glTranslated(-x / 2, y / 2, 0.0f);
            myFont.ftWrite("Background");
            Gl.glPopMatrix();
            myFont.ftEndFont();
        }       
        
       
        void setViewport(int myWidth, int myHeight)
        {
            Gl.glViewport(0, 0, myWidth, myHeight);
        }
        void setPerspective(double myWidth, double myHeight)
        {
            if (myHeight == 0)
                myHeight = 1;
            Glu.gluPerspective(90, (double)myWidth / myHeight, 0.1, 800);          // Calculate The Aspect Ratio Of The Window
        }
        #endregion

    }

    #region OBJECTS
    public abstract class Shape:IDrawable
    {
        protected int id;
        int tag;

        public int Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        protected IPoint origin;
        protected bool visible;

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

        public bool Visible
        {
            set { visible = value; }
        }
    }

    public class selectorObj
    {
        //int selectedObjectId=-1;
        
        ICanvas canvas;

        public ICanvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public selectorObj(ICanvas _canvas)
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
        public int  getSelectedObjectId(int[] selectionLocation,IWorld world)
        {
            int[] buffer = new int[512];
            int hits = 0;
            double[] position;

            Gl.glSelectBuffer(512, buffer);
            beginSelection(selectionLocation);

            foreach(IDrawable _obj in world.getEntities())
            {
                Gl.glLoadName(_obj.getId());
                Gl.glPushMatrix();
                position=_obj.getPosition();
                _obj.draw();
                Gl.glPopMatrix();
            }
        
            endSelection();

            hits = Gl.glRenderMode(Gl.GL_RENDER);

            //For now, return number with highest z value
            if (hits > 0)
            {
                int highest = buffer[1], highestId=buffer[3];
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
            processHits(hits, buffer);
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
    public class wallObj:IDrawable
    {
        protected tileObj[,] myTiles;
        int myHeight, myWidth, tileSize;
        Common.planeOrientation orientation;
        public float[] defaultColor, defaultOutlineColor;
        bool visible=true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        public int TileSize
        {
            get { return tileSize; }
            set { tileSize = value; }
        }
        public tileObj[,] MyTiles
        {
            get { return myTiles; }
            set { myTiles = value; }
        }
        public Common.planeOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        public int MyHeight
        {
            get { return myHeight; }
            set { myHeight = value; }
        }
        public int MyWidth
        {
            get { return myWidth; }
            set { myWidth = value; }
        }
        protected IPoint origin;

        public IPoint MyOrigin
        {
            get { return origin; }
            set { origin = value.copy(); }
        }

        public wallObj()
        {
            myTiles=new tileObj[0,0];
        }
        /* Wall starts at "origin", wallHeight and wallWidth is how many tiles it has in each dimension,
         * tileSize their size, orientation is 1 for wall perpendicular to X axis, 2 for Y axis, 3 for Z axis,
         * and color and outlineColor the tiles' colors*/
        public wallObj(IPoint origin, int wallHeight, int wallWidth,
            int _tileSize, int _orientation, float[] color, float[] outlineColor)
        {
            myHeight = wallHeight;
            myWidth = wallWidth;
            MyOrigin = origin;
            tileSize = _tileSize;
            orientation = (Common.planeOrientation)_orientation;
            myTiles = new tileObj[wallHeight, wallWidth];
            defaultColor = color;
            defaultOutlineColor = outlineColor;

            fillWall();
        }
        /* Wall starts at "origin", wallHeight and wallWidth is how many tiles it has in each dimension,
        * tileSize their size, orientation orientation of perpendicular line,
        * and color and outlineColor the tiles' colors*/
        public wallObj(IPoint  origin, int wallHeight, int wallWidth,
            int _tileSize, Common.planeOrientation _orientation, float[] color, float[] outlineColor)
        {
            myHeight = wallHeight;
            myWidth = wallWidth;
            MyOrigin = origin;
            tileSize = _tileSize;
            this.orientation = _orientation;
            myTiles = new tileObj[wallHeight, wallWidth];
            defaultColor = color;
            defaultOutlineColor = outlineColor;

            fillWall();
        }

        public virtual void fillWall()
        {
           myTiles=createTiles();
        }
        public bool Intercepts(IPoint src, IPoint dest)
        {
            foreach (tileObj tile in myTiles)
            {
                if (tile.Intercepts(src, dest))
                    return true;
            }
            return false;
        }
        public tileObj[,] createTiles()
        {
            tileObj[,] newTiles = new tileObj[myWidth, myHeight];
            IPoint currentTileOrigin = origin.copy(), currentTileEnd;
            #region CREATE_TILES_FOR_WALL
            switch (orientation)
            {
                case Common.planeOrientation.X: //Perpendicular to X axis
                    {
                        currentTileEnd = new pointObj(currentTileOrigin.X,
                            currentTileOrigin.Y + tileSize, currentTileOrigin.Z + tileSize);
                        for (int i = 0; i < myHeight; i++)
                        {

                            for (int j = 0; j < myWidth; j++)
                            {
                                currentTileOrigin.Z = currentTileOrigin.Z + tileSize;
                                currentTileEnd.Z = currentTileEnd.Z + tileSize;
                                newTiles[i, j] = new tileObj(currentTileOrigin, currentTileEnd, defaultColor, defaultOutlineColor);
                            }
                            currentTileOrigin.Y = currentTileOrigin.Y + tileSize;
                            currentTileEnd.Y = currentTileEnd.Y + tileSize;

                            currentTileOrigin.Z = origin.Z;
                            currentTileEnd.Z = currentTileOrigin.Z + tileSize;
                        }
                        break;
                    }
                case Common.planeOrientation.Y: //Perpendicular to Y axis
                    {
                        currentTileEnd = new pointObj(currentTileOrigin.X + tileSize,
                            currentTileOrigin.Y, currentTileOrigin.Z + tileSize);
                        for (int i = 0; i < myHeight; i++)
                        {
                            for (int j = 0; j < myWidth; j++)
                            {
                                currentTileOrigin.X = currentTileOrigin.X + tileSize;
                                currentTileEnd.X = currentTileEnd.X + tileSize;
                                newTiles[i, j] = new tileObj(currentTileOrigin, currentTileEnd, defaultColor, defaultOutlineColor);
                            }
                            currentTileOrigin.Z = currentTileOrigin.Z + tileSize;
                            currentTileEnd.Z = currentTileEnd.Z + tileSize;

                            currentTileOrigin.X = origin.X;
                            currentTileEnd.X = currentTileOrigin.X + tileSize;
                        }
                        break;
                    }
                case Common.planeOrientation.Z: //Perpendicular to Z axis
                    {
                        currentTileEnd = new pointObj(currentTileOrigin.X + tileSize,
                            currentTileOrigin.Y + tileSize, currentTileOrigin.Z);
                        for (int i = 0; i < myWidth; i++)
                        {
                            for (int j = 0; j < myHeight; j++)
                            {
                                newTiles[i, j] = new tileObj(currentTileOrigin, currentTileEnd, defaultColor, defaultOutlineColor);
                                currentTileOrigin.X = currentTileOrigin.X + tileSize;
                                currentTileEnd.X = currentTileEnd.X + tileSize;
                            }
                            currentTileOrigin.Y = currentTileOrigin.Y + tileSize;
                            currentTileEnd.Y = currentTileEnd.Y + tileSize;

                            currentTileOrigin.X = origin.X;
                            currentTileEnd.X = currentTileOrigin.X + tileSize;
                        }
                        break;
                    }
            }
            #endregion
            return newTiles;
        }

        public void setTileColor(int indexI, int indexJ, float[] color, float[] outlineColor)
        {
            if (myWidth > indexI && myHeight > indexJ)
            {
                myTiles[indexI, indexJ].MyColor = color;
                myTiles[indexI, indexJ].MyOutlineColor = outlineColor;
            }
        }
        void recolorWall(float[] color)
        {
            for (int i = 0; i < MyHeight; i++)
            {
                for (int j = 0; j < this.MyWidth; j++)
                {
                    MyTiles[i, j].setColor(color);
                }
            }
        }

        public void draw()
        {
            if(Visible)
                Common.staticDrawWall(this);
        }

        public int getId()
        {
            throw new NotImplementedException();
        }

        public double[] getPosition()
        {
            return MyOrigin.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }
    }
    public class tileObj:Shape
    {
        public IPoint end;
        Common.planeOrientation Orientation;
        private double tileSize;
        private float[] myColor;
        private float[] myOutlineColor;
        public int angle=0;
        

        public IPoint MyEnd
        {
            get { return end; }
            set { end = value.copy(); }
        }
        public IPoint MyOrigin
        {
            get { return origin; }
            set { origin = value.copy(); }
        }       
        public double TileSize
        {
            get { return tileSize; }
            set { tileSize = value; }
        }
        
        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        public float[] MyOutlineColor
        {
            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }

        public tileObj(IPoint origin, IPoint end, float[] color, float[] outlineColor)
        {
            MyOrigin = origin;
            MyEnd = end;
            myColor = color;
            myOutlineColor = outlineColor;
            setOrientation();
            setTileSize();
        }
        public tileObj()
        {
            myColor = new float[] { 0.0f, 0.0f, 0.0f };
            myOutlineColor = new float[] { 0.0f, 0.0f, 0.0f };
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
        public void setOrientation()
        {
            if(MyOrigin.X==MyEnd.X)
                Orientation=Common.planeOrientation.X;
            else if(MyOrigin.Y==MyEnd.Y)
                Orientation=Common.planeOrientation.Y;
            else if(MyOrigin.Z==MyEnd.Z)
                Orientation=Common.planeOrientation.Z;
            else 
                Orientation=Common.planeOrientation.None;
        }
        public void setTileSize()
        {
            switch (Orientation)
            {
                case Common.planeOrientation.X:
                    TileSize = MyEnd.Z - MyOrigin.Z;
                    break;
                case Common.planeOrientation.Y:
                    TileSize = MyEnd.Z - MyOrigin.Z;
                    break;
                case Common.planeOrientation.Z:
                    TileSize = MyEnd.X - MyOrigin.X;
                    break;
                default:
                    break;
            }
        }

        public void turn45()
        {
            angle++;
        }

        public bool Intercepts(IPoint src, IPoint dest)
        {
            //Assume we dont want tiles perp to Z
            double zStart = Math.Min(MyOrigin.Z, MyEnd.Z), zEnd = Math.Max(MyOrigin.Z, MyEnd.Z);
            double lineMinY = Math.Min(src.Y, dest.Y), lineMaxY = Math.Max(src.Y, dest.Y),
                lineMinX = Math.Min(src.X, dest.X), lineMaxX = Math.Max(src.X, dest.X);
            if (src.Z <zStart || src.Z>zEnd)
                return false;
            //Slope of line 
            double slope = 0;
            bool noSlope = false;
            if (src.X != dest.X)
                slope = (src.Y - dest.Y) / (src.X - dest.X);
            else
                noSlope = true;
            double spot, start, end;
            double equationResult;
            //Get checking point
            setOrientation();
            switch (Orientation)
            {
                case Common.planeOrientation.X://Vertical, so get Y positions
                    if (!noSlope)//Vertical lines wont intercept vertical tiles
                    {
                        spot = origin.X;
                        if (lineMaxX < spot || lineMinX > spot)//endpoints too high or too low
                            return false;
                        start = Math.Min(MyOrigin.Y, MyEnd.Y);
                        end = Math.Max(MyOrigin.Y, MyEnd.Y);
                        equationResult = slope * (spot - src.X) + src.Y;
                        return (start <= equationResult && equationResult <= end);
                    }
                    else
                        return false;
                case Common.planeOrientation.Y://Horizontal, so get X positions
                    spot = origin.Y;
                    if (lineMaxY < spot || lineMinY > spot)//endpoints too high or too low
                        return false;
                    start = Math.Min(MyOrigin.X, MyEnd.X);
                    end = Math.Max(MyOrigin.X, MyEnd.X);
                    if (noSlope)
                        equationResult = src.X;//Same X for all points
                    else
                        equationResult = (spot - src.Y) / slope + src.X;
                    return (start <= equationResult && equationResult <= end);
                default:
                    return false;
            }
        }

        public tileObj copy()
        {
            return new tileObj(origin.copy(), end.copy(), MyColor, MyOutlineColor);
        }

        public override void draw()
        {
            Common.drawTileAndOutline(this);
        }

    }
    public class rectangleObj:IDrawable
    {
        static int rectableObjIds = 1;
        public const int idType = 1;
        public int myId;
        bool visible=true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint bottomLeft,topLeft,bottomRight,topRight;
        Common.planeOrientation Orientation;

        public Common.planeOrientation Orientation1
        {
            get { return Orientation; }
        }
        private float[] myColor;
        private float[] myOutlineColor;

        public IPoint TopRight
        {
          get { return topRight; }
          set { topRight = value; }
        }

        public IPoint BottomRight
        {
          get { return bottomRight; }
          set { bottomRight = value; }
        }

        public IPoint TopLeft
        {
          get { return topLeft; }
          set { topLeft = value; }
        }

        public IPoint BottomLeft
        {
          get { return bottomLeft; }
          set { bottomLeft = value; }
        }       

        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }        

        public float[] MyOutlineColor
        {
            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }

        public void setOrientation()
        {
            if(BottomLeft.X==TopRight.X)
                Orientation=Common.planeOrientation.X;
            else if(BottomLeft.Y==TopRight.Y)
                Orientation=Common.planeOrientation.Y;
            else if(BottomLeft.Z==TopRight.Z)
                Orientation=Common.planeOrientation.Z;
            else 
                Orientation=Common.planeOrientation.None;
        }

        public rectangleObj(IPoint _bLeft, IPoint _tLeft,IPoint _bRight,IPoint _tRight
            , float[] color, float[] outlineColor)
        {
            BottomLeft=_bLeft;
            bottomRight=_bRight;
            TopLeft=_tLeft;
            TopRight=_tRight;
            myColor = color;
            myOutlineColor = outlineColor;
            setOrientation();
        }
        public rectangleObj()
        {
            myColor = new float[] { 0.0f, 0.0f, 0.0f };
            myOutlineColor = new float[] { 0.0f, 0.0f, 0.0f };
            myId = rectableObjIds;
            rectableObjIds++;
        }

        public rectangleObj copy()
        {
            return new rectangleObj(BottomLeft.copy(), TopLeft.copy(),bottomRight.copy(),topRight.copy(),
                MyColor, MyOutlineColor);
        }

        public void draw()
        {
            if(Visible)
            Common.drawRectangleAndOutline(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[]{bottomLeft.X,bottomLeft.Y,bottomLeft.Z};
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }
    }
    public class cubeObj:IDrawable
    {
        static int cubeObjIds = 1;
        public const int idType = 1;
        public int myId;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        int cubeSize;
        int angle = 0;

        /// <summary>
        /// As Multiples of 45 Degrees. So 3 means 135 Degrees
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        IPoint origin;
        IPoint rotationAxis;
        internal Stack<Rotation> Rotations;

        public IPoint RotationAxis
        {
            get { return rotationAxis; }
            set { rotationAxis = value; }
        }
        float[] myColor;
        float[] myOutlineColor;

        public float[] OutlineColor
        {
            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }
        tileObj tileFront, tileBack, tileLeft, tileRight, tileTop, tileBottom;

        public IPoint MyOrigin
        {
            get { return origin.copy(); }
            set { origin = value.copy(); }
        }

        public tileObj TileBottom
        {
            get { return tileBottom; }
            set { tileBottom = value; }
        }
        public tileObj TileTop
        {
            get { return tileTop; }
            set { tileTop = value; }
        }
        public tileObj TileRight
        {
            get { return tileRight; }
            set { tileRight = value; }
        }
        public tileObj TileLeft
        {
            get { return tileLeft; }
            set { tileLeft = value; }
        }
        public tileObj TileBack
        {
            get { return tileBack; }
            set { tileBack = value; }
        }
        public tileObj TileFront
        {
            get { return tileFront; }
            set { tileFront = value; }
        }

        public float[] Color
        {
            get { return myColor; }
            set { myColor = value; }
        }
        public int CubeSize
        {
            get { return cubeSize; }
            set { cubeSize = value; }
        }
        public void turn45()
        {
            //TileFront.turn45();
            //TileBack.turn45();
            //TileLeft.turn45();
            //TileRight.turn45();
            //TileTop.turn45();
            //TileBottom.turn45();
            angle++;
        }

        public cubeObj(){
            myId = cubeObjIds++;
            Rotations = new Stack<Rotation>();
        }
        public cubeObj(IPoint origin, int cubeSize, float[] color, float[] outlineColor)
        {
            myId = cubeObjIds++;
            this.origin = origin;
            this.cubeSize = cubeSize;
            this.myColor = color;
            this.myOutlineColor = outlineColor;
            createCubeTiles();
            rotationAxis = new pointObj(origin.X + cubeSize / 2, origin.Y + cubeSize / 2, 0);
            Rotations = new Stack<Rotation>();
        }
        public void createCubeTiles()
        {
            rotationAxis = new pointObj(origin.X + cubeSize / 2, origin.Y + cubeSize / 2, 0);
            tileFront = new tileObj(origin,
                new pointObj(origin.X + cubeSize, origin.Y, origin.Z + cubeSize),
                Color, OutlineColor);
            tileRight = new tileObj(new pointObj(origin.X + cubeSize, origin.Y, origin.Z),
                new pointObj(origin.X + cubeSize, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
            tileLeft = new tileObj(origin,
                new pointObj(origin.X, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
            tileBack = new tileObj(new pointObj(origin.X, origin.Y + cubeSize, origin.Z),
                new pointObj(origin.X + cubeSize, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
            tileBottom = new tileObj(origin,
                new pointObj(origin.X + cubeSize, origin.Y + cubeSize, origin.Z),
                Color, OutlineColor);
            tileTop = new tileObj(new pointObj(origin.X, origin.Y, origin.Z + cubeSize),
                new pointObj(origin.X + cubeSize, origin.Y + cubeSize, origin.Z + cubeSize),
                Color, OutlineColor);
        }

        public bool Intercepts(IPoint src, IPoint dest)
        {
            return (tileFront.Intercepts(src, dest) || tileBack.Intercepts(src, dest) ||
                tileLeft.Intercepts(src, dest) || tileRight.Intercepts(src, dest)
                ||tileBottom.Intercepts(src,dest)||tileTop.Intercepts(src,dest));
        }

        public void draw()
        {
            if(Visible)
            Common.drawCubeAndOutline(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[] { origin.X, origin.Y, origin.Z };
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }

        public void Rotate(lineObj line,double degrees)
        {
            Rotations.Push(new Rotation() { Axis = line, Degrees = degrees });
        }
    }
    public class rombusObj:IDrawable
    {
        static int rhombusObjIds = 1;
        public const int idType = 1;
        public int myId;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }



        public IPoint origin, end;

        public IPoint MyEnd
        {
            get { return end; }
            set { end = value.copy(); }
        }

        public IPoint MyOrigin
        {
            get { return origin; }
            set { origin = value.copy(); }
        }
        private int rombusSize;

        public int RombusSize
        {
            get { return rombusSize; }
            set { rombusSize = value; }
        }
        private float[] myColor;

        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        private float[] myOutlineColor;

        public float[] MyOutlineColor
        {

            get { return myOutlineColor; }
            set { myOutlineColor = value; }
        }

        public rombusObj(IPoint origin, IPoint end, float[] color, float[] outlineColor)
        {
            myId = rhombusObjIds++;
            MyOrigin = origin;
            MyEnd = end;
            myColor = color;
            myOutlineColor = outlineColor;
        }
        public rombusObj()
        {
            myId = rhombusObjIds++;
            myColor = new float[] { 0.0f, 0.0f, 0.0f };
            myOutlineColor = new float[] { 0.0f, 0.0f, 0.0f };
        }
        public rombusObj(tileObj tile)
        {
            myId = rhombusObjIds++;
            MyOrigin = tile.MyOrigin;
            MyEnd = tile.MyEnd;
            myColor = tile.MyColor;
            myOutlineColor = tile.MyOutlineColor;
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

        public rombusObj copy()
        {
            return new rombusObj(origin.copy(), end.copy(), MyColor, MyOutlineColor);
        }

        public void draw()
        {
            if(Visible)
            Common.drawRombusAndOutline(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[] { origin.X, origin.Y, origin.Z };
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }
    }
    public class circleObj:IDrawable
    {
        static int circleObjId = 1;
        public int myId;
        public double[,] vertices = new double[360,3];
        IPoint center;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint Center
        {
            get { return center; }
            set { center = value; }
        }
        double radius;
        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        float[] myColor;
        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        float[] outlineColor;

        public float[] OutlineColor
        {
            get { return outlineColor; }
            set { outlineColor = value; }
        }
        
        
        public circleObj(IPoint center, double radius, float[] color,float[] outlineColor)
        {
            Center = center;
            Radius = radius;
            MyColor = color;
            OutlineColor = outlineColor;
            myId = circleObjId++;
            for (int i = 0; i < vertices.Length/3; i++)
            {
                vertices[i,0] = Radius * Math.Cos(i * Math.PI / 180);
                vertices[i,1] = Radius * Math.Sin(i * Math.PI / 180);
                vertices[i,2]= 0;
            }
        }


        public void draw()
        {
            if(Visible)
            Common.drawCircle(this);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return new double[] { center.X, center.Y, center.Z };
        }

        public void setPosition(IPoint newPosition)
        {
            throw new NotImplementedException();
        }

        
    }
    public class lineObj : IDrawable
    {
        static int lineIds = 0;
        int myId;

        float[] myColor;
        public float[] MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        IPoint p1, p2;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public IPoint P2
        {
            get { return p2; }
            set { p2 = value; }
        }
        public IPoint P1
        {
            get { return p1; }
            set { p1 = value; }
        }

        /// <summary>
        /// If no color is entered,defaults to red
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="color"></param>
        public lineObj(IPoint p1, IPoint p2,float[] color=null)//defaulted to red
        {
            MyColor = color ?? Common.colorRed;
            P1 = p1;
            P2 = p2;

            myId = lineIds++;
        }

        public lineObj()
        {
            myId = lineIds++;
        }

        public void draw()
        {
            if(Visible)
            Common.drawLine(P1,P2,MyColor);
        }

        public int getId()
        {
            return myId;
        }

        public double[] getPosition()
        {
            return P2.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            P2=newPosition;
        }
    }
    public class parallepiped : IDrawable
    {
        IPoint origin, rotationAxis;
        int angle=0;
        double xWidth, yWidth, zWidth;
        float[] color, outlineColor;
        rectangleObj tileFront, tileBack, tileLeft, tileRight, tileTop, tileBottom;
        bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        

        #region Properties
        public IPoint RotationAxis
        {
            get { return rotationAxis; }
            set { rotationAxis = value; createTiles(); }
        }
        public rectangleObj TileBottom
        {
            get { return tileBottom; }
            set { tileBottom = value; }
        }
        public rectangleObj TileTop
        {
            get { return tileTop; }
            set { tileTop = value; }
        }
        public rectangleObj TileRight
        {
            get { return tileRight; }
            set { tileRight = value; }
        }
        public rectangleObj TileLeft
        {
            get { return tileLeft; }
            set { tileLeft = value; }
        }
        public rectangleObj TileBack
        {
            get { return tileBack; }
            set { tileBack = value; }
        }
        public rectangleObj TileFront
        {
            get { return tileFront; }
            set { tileFront = value; }
        }
        public IPoint Origin
        {
            get { return origin; }
        }
        /// <summary>
        /// As multiple of 45 degrees.
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        public float[] OutlineColor
        {
            get { return outlineColor; }
            set { outlineColor = value; }
        }

        public float[] Color
        {
            get { return color; }
            set { color = value; }
        }

        public double ZWidth
        {
            get { return zWidth; }
            set { zWidth = value; createTiles(); }
        }

        public double YWidth
        {
            get { return yWidth; }
            set { yWidth = value; createTiles(); }
        }

        public double XWidth
        {
            get { return xWidth; }
            set { xWidth = value; createTiles(); }
        }
        #endregion

        public parallepiped(IPoint origin, double xWidth, double yWidth, double zWidth,float[] color,
            float[] outlineColor=null )
        {
            this.origin = origin;
            this.xWidth = xWidth;
            this.yWidth = yWidth;
            this.zWidth = zWidth;
            rotationAxis = new pointObj(origin.X + xWidth / 2, origin.Y + yWidth / 2, 0);
            this.color = color;
            this.outlineColor = outlineColor ?? Common.colorBlack;
            createTiles();
        }

        private void createTiles()
        {
            IPoint b1 = origin, 
                b2 = new pointObj(b1.X + xWidth, b1.Y, b1.Z), 
                b3 = new pointObj(b1.X + xWidth, b1.Y + yWidth, b1.Z),
                b4 = new pointObj(b1.X, b1.Y + yWidth, b1.Z), 
                t1 = new pointObj(b1.X, b1.Y, b1.Z + zWidth), 
                t2 = new pointObj(b1.X + xWidth, b1.Y, b1.Z + zWidth), 
                t3 = new pointObj(b1.X + xWidth, b1.Y + yWidth, b1.Z + zWidth),
                t4 = new pointObj(b1.X, b1.Y + yWidth, b1.Z+zWidth);

            tileBottom = new rectangleObj(b1,b4,b2,b3,
                Color, OutlineColor);
            tileRight = new rectangleObj(b2,t2,b3,t3,
                Color, OutlineColor);
            tileLeft = new rectangleObj(b1,t1,b4,t4,
                Color, OutlineColor);
            tileTop = new rectangleObj(t1,t4,t2,t3,
                Color, OutlineColor);
            tileFront = new rectangleObj(b1,t1,b2,t2,
                Color, OutlineColor);
            tileBack = new rectangleObj(b3,t3,b4,t4,
                Color, OutlineColor);
        }


        public void draw()
        {
            if (Visible)
                Common.drawParallepiped(this);
        }

        public int getId()
        {
            return 0;
        }

        public double[] getPosition()
        {
            return Origin.toArray();
        }

        public void setPosition(IPoint newPosition)
        {
            origin=newPosition;
            createTiles();
        }
    }
    #endregion OBJECTS
}
