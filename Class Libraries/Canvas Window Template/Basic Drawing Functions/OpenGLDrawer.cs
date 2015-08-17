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
using Canvas_Window_Template.Drawables.Shapes;

namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    /// <summary>
    /// Use this class to draw primitives (tiles,cubes,walls, etc).
    /// However, the intended use is to create an IWorld implementation, add drawables to it (tiles,cubes,walls) and 
    /// call draw on the IWorld;
    /// </summary>
    public class OpenGLDrawer
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
                OpenGLDrawer.myFont = new FTFont(fontResource, out Errors);
                // INITIALISE FONT AS A PER_CHARACTER TEXTURE MAPPED FONT
                OpenGLDrawer.myFont.ftRenderToTexture(24, 196);

                OpenGLDrawer.myFont.FT_ALIGN = FTFontAlign.FT_ALIGN_CENTERED;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }


        #region DRAW FUNCTIONS
        public static float[] colorToArray(System.Drawing.Color color)
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

        public static void drawTile(OpenGLTile tile)
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
                        center = new OpenGLPoint(tile.MyOrigin.X, tile.MyOrigin.Y + tile.TileSize / 2,
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
                        center = new OpenGLPoint(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y,
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
                        center = new OpenGLPoint(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y + tile.TileSize / 2,
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
        public static void drawTileOutline(OpenGLTile tile)
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
                        center = new OpenGLPoint(tile.MyOrigin.X, tile.MyOrigin.Y + tile.TileSize / 2,
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
                        center = new OpenGLPoint(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y,
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
                        center = new OpenGLPoint(tile.MyOrigin.X + tile.TileSize/2, tile.MyOrigin.Y + tile.TileSize / 2,
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
        public static void drawTileAndOutline(OpenGLTile tile)
        {
            drawTile(tile);
            drawTileOutline(tile);
        }
        

        public static void drawRectangle(Rectangle rect)
        {
            if(rect.Orientation1!=OpenGLDrawer.planeOrientation.None)
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
        public static void drawRectangleOutline(Rectangle rect)
        {
            if(rect.Orientation1!=OpenGLDrawer.planeOrientation.None)
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
        public static void drawRectangleAndOutline(Rectangle rect)
        {
            drawRectangle(rect);
            drawRectangleOutline(rect);
        }

        public static void drawRombus(OpenGLRombus rombus)
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
        public static void drawRombusOutline(OpenGLRombus rombus)
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
        public static void drawRombusAndOutline(OpenGLRombus rombus)
        {
            drawRombus(rombus);
            drawRombusOutline(rombus);
        }

        public static void drawCubeOutline(OpenGLCube cube)
        {
            //Draw Faces
            Gl.glPushMatrix();
            translate(cube.RotationAxis);
            rotate(cube.Angle * 45);
            translate(new OpenGLPoint(-cube.RotationAxis.X, -cube.RotationAxis.Y, 0));
            drawTileOutline(cube.TileFront);
            drawTileOutline(cube.TileBack);
            drawTileOutline(cube.TileLeft);
            drawTileOutline(cube.TileRight);
            drawTileOutline(cube.TileBottom);
            drawTileOutline(cube.TileTop);  
            Gl.glPopMatrix();
        }
        public static void drawCube(OpenGLCube cube)
        {
            //Draw Faces
            Gl.glPushMatrix();
            translate(cube.RotationAxis);
            rotate(cube.Angle * 45);
            translate(new OpenGLPoint(-cube.RotationAxis.X, -cube.RotationAxis.Y, 0));
            drawTile(cube.TileFront);
            drawTile(cube.TileBack);
            drawTile(cube.TileLeft);
            drawTile(cube.TileRight);
            drawTile(cube.TileBottom);
            drawTile(cube.TileTop);
            Gl.glPopMatrix();
        }
        public static void drawCubeAndOutline(OpenGLCube cube)
        {
            //Draw Faces
            Gl.glPushMatrix();
            foreach(Rotation rotation in cube.Rotations)
            {
                double x = rotation.Axis.P2.X - rotation.Axis.P1.X,
                    y = rotation.Axis.P2.Y - rotation.Axis.P1.Y,
                    z = rotation.Axis.P2.Z - rotation.Axis.P1.Z;
                translate(rotation.Axis.P1);
                rotate(rotation.Degrees,new OpenGLPoint(x,y,z));
                translate(new OpenGLPoint(-rotation.Axis.P1.X, -rotation.Axis.P1.Y, -rotation.Axis.P1.Z));
            }
            //translate(cube.RotationAxis);
            //rotate(cube.Angle * 45);
            //translate(new OpenGLPoint(-cube.RotationAxis.X, -cube.RotationAxis.Y, 0));
            drawTileAndOutline(cube.TileFront);
            drawTileAndOutline(cube.TileBack);
            drawTileAndOutline(cube.TileLeft);
            drawTileAndOutline(cube.TileRight);
            drawTileAndOutline(cube.TileBottom);
            drawTileAndOutline(cube.TileTop);
            Gl.glPopMatrix();
        }

        public static void drawParallepiped(OpenGLParallepiped p)
        {
            //Draw Faces
            Gl.glPushMatrix();
            translate(p.RotationAxis);
            rotate(p.Angle * 45);
            translate(new OpenGLPoint(-p.RotationAxis.X, -p.RotationAxis.Y, 0));
            drawRectangleAndOutline(p.TileBottom);
            drawRectangleAndOutline(p.TileBack);
            drawRectangleAndOutline(p.TileFront);            
            drawRectangleAndOutline(p.TileLeft);
            drawRectangleAndOutline(p.TileRight);            
            drawRectangleAndOutline(p.TileTop);
            
            Gl.glPopMatrix();
        }

        public static void drawCircleOutline(OpenGLCircle circle)
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
        public static void drawCircle(OpenGLCircle circle)
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
                
                drawLine(new OpenGLPoint(circle.Radius, 0,0)
                    ,new OpenGLPoint(- circle.Radius,0, 0),circle.MyColor);
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
        public static void staticDrawWall(OpenGLWall myWall)
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

        public static Tile[,] createTileArray(int width, int height, OpenGLTile[,] tiles, float[] color, double tileSize)
        {
            Tile[,] tileArray = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tileArray[i, j] = new Tile(tiles[i, j].MyOrigin, tiles[i, j].MyEnd);
                    tileArray[i, j].SetColor(color);
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
        public void drawWall(OpenGLWall myWall)
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
}
