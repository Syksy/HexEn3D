using System;

namespace HexEn3D
{
    public class Hex
    {
        // Hex has 6 corners
        /*  1. /-----\ 2.
         *    / \   / \
         *0. /___6_7___\ 3.
         *   \   / \   /
         *    \ /   \ /
         *  5. \-----/ 4.
         */
        // Additionally, for 3d plotting the mesh triangles require two additional points inside
        private xyz[] vertices; // Length 8
        // The height at which the hex is at
        private double elevation;
        // Whether the Hex is in active use
        private Boolean active;

        public Hex()
        {
            vertices = new xyz[8];
            for(int i=0; i<8; i++)
            {
                vertices[i] = new xyz(); // coords 0,0,0
            }
            this.elevation = 0.0; // By default on the z=0 plane
            this.active = true; // Created Hexes are active by default
        }
        public Hex(double elevation)
        {
            vertices = new xyz[8];
            for (int i = 0; i < 8; i++)
            {
                vertices[i] = new xyz(); // coords 0,0,elevation
            }
            this.elevation = elevation; // By default on the z=0 plane
            this.active = true; // Created Hexes are active by default
        }
        public Hex(xyz[] xyzs, double elevation)
        {
            if (xyzs.Length != 8) throw new System.ArgumentOutOfRangeException("Parameter xyzs should be of length 8 in Hex class.");
            this.vertices = xyzs;
            this.elevation = elevation; // Custom z-axis elevation
            this.active = true; // Created Hexes are active by default
        }

        // Getters
        public double getElevation()
        {
            return this.elevation;
        }
        public xyz[] getVertices()
        {
            return this.vertices;
        }
        public xyz getVertexAt(int i)
        {
            return this.vertices[i];
        }
        public Boolean getActive()
        {
            return this.active;
        }


        // Setters
        public void setElevation(double elevation)
        {
            // Central vertices 6 & 7 are always at hex elevation
            this.vertices[6].setZ(elevation);
            this.vertices[7].setZ(elevation);
            this.elevation = elevation;
        }
        public void setVertexAt(xyz xyztmp, int index)
        {
            if (index < 0 | index > 8) throw new System.ArgumentOutOfRangeException("Parameter index in Hyx.setVertexAt should be between 0 and 7.");
            this.vertices[index] = xyztmp;
        }
        public void setVertexXAt(double x, int index)
        {
            if (index < 0 | index > 8) throw new System.ArgumentOutOfRangeException("Parameter index in Hyx.setVertexXAt should be between 0 and 7.");
            this.vertices[index].setX(x);
        }
        public void setVertexYAt(double y, int index)
        {
            if (index < 0 | index > 8) throw new System.ArgumentOutOfRangeException("Parameter index in Hyx.setVertexYAt should be between 0 and 7.");
            this.vertices[index].setY(y);
        }
        public void setVertexZAt(double z, int index)
        {
            if (index < 0 | index > 8) throw new System.ArgumentOutOfRangeException("Parameter index in Hyx.setVertexZAt should be between 0 and 7.");
            this.vertices[index].setZ(z);
        }
        public void setVertices(xyz[] xyztmp)
        {
            if(xyztmp.Length!=8) throw new System.ArgumentOutOfRangeException("Parameter xyztmp in Hyx.setVertex should be a xyz[] array of length 8.");
            this.vertices = xyztmp;
        }
        public void setActive(bool active)
        {
            this.active = active;
        }

        // Other methods

        // Override String representation of the xyz-object
        public override String ToString()
        {
            string str = "Hex representation;\n";
            for(int i = 0; i < 8; i++)
            {
                str += "Vertex" + i + ": " + vertices[i] + "\n";
            }
            return str;
        }

    }
}
