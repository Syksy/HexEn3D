using System;

namespace HexEn3D
{
    public class xyz
    {
        // {x,y,z} coordinates in a continuous space
        private double x, y, z;

        public xyz(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public xyz()
        {
            this.x = 0.0;
            this.y = 0.0;
            this.z = 0.0;
        }
        // Getters
        public double getX()
        {
            return x;
        }
        public double getY()
        {
            return y;
        }
        public double getZ()
        {
            return z;
        }

        // Setters
        public void setX(double x)
        {
            this.x = x;
        }
        public void setY(double y)
        {
            this.y = y;
        }
        public void setZ(double z)
        {
            this.z = z;
        }

        // Other methods

        // Adding and subtracting two {x,y,z} vectors
        public static xyz operator +(xyz xyz1, xyz xyz2)
        {
            xyz tmpxyz = new xyz();
            tmpxyz.setX(xyz1.getX() + xyz2.getX());
            tmpxyz.setY(xyz1.getY() + xyz2.getY());
            tmpxyz.setZ(xyz1.getZ() + xyz2.getZ());
            return tmpxyz;
        }
        public static xyz operator -(xyz xyz1, xyz xyz2)
        {
            xyz tmpxyz = new xyz();
            tmpxyz.setX(xyz1.getX() - xyz2.getX());
            tmpxyz.setY(xyz1.getY() - xyz2.getY());
            tmpxyz.setZ(xyz1.getZ() - xyz2.getZ());
            return tmpxyz;
        }

        // Override String representation of the xyz-object
        public override String ToString()
        {
            return "{x=" + Math.Round(x, 3) + ", y=" + Math.Round(y, 3) + ", z=" + Math.Round(z, 3) + "}";
        }
    }
}