using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template.Basic_Drawing_Functions
{
    public class OpenGLPoint : IPoint
    {
        double myX, myY, myZ;

        public double Z
        {
            get { return myZ; }
            set { myZ = value; }
        }
        public double Y
        {
            get { return myY; }
            set { myY = value; }
        }
        public double X
        {
            get { return myX; }
            set { myX = value; }
        }

        public OpenGLPoint() { myX = 0; myY = 0; myZ = 0; }
        public OpenGLPoint(float X, float Y, float Z)
        {
            myX = X; myY = Y; myZ = Z;
        }
        /// <summary>
        /// Coordinates will be casted to double
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public OpenGLPoint(double X, double Y, double Z)
        {
            myX = (double)X; myY = (double)Y; myZ = (double)Z;
        }
        public OpenGLPoint(string X, string Y, string Z)
        {
            myX = Int32.Parse(X);
            myY = Int32.Parse(Y);
            myZ = Int32.Parse(Z);
        }
        public OpenGLPoint(double[] point)
        {
            if (point.Length > 2)
            {
                X = (double)point[0]; Y = (double)point[1]; Z = (double)point[2];
            }
            else
            {
                X = (double)point[0]; Y = (double)point[1]; Z = 0;
            }
        }
        public IPoint copy()
        {
            return new OpenGLPoint(myX, myY, Z);
        }
        public double[] toArray()
        {
            return new double[] { X, Y, Z };
        }
        public string toString()
        {
            return myX.ToString() + "," + myY.ToString() + "," + myZ.ToString();
        }
        public bool equals(IPoint point)
        {
            return point != null && (X == point.X && Y == point.Y && Z == point.Z);
        }
        public bool equals(double[] point)
        {
            return (X == point[0] && Y == point[1] && Z == point[2]);
        }
    }
}
