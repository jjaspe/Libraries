using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canvas_Window_Template.Interfaces;

namespace Canvas_Window_Template.Drawables
{
    public class PointObj:IPoint

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

        public PointObj() { myX = 0; myY = 0; myZ = 0; }
        public PointObj(double X, double Y, double Z)
        {
            myX = X; myY = Y; myZ = Z;
        }
        public PointObj(string X, string Y, string Z)
        {
            myX = Int32.Parse(X);
            myY = Int32.Parse(Y);
            myZ = Int32.Parse(Z);
        }
        
        public PointObj(double[] point)
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
            return new PointObj(myX, myY, Z);
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
