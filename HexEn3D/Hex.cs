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
        // Information of where the Hex is located on a map (not necessarily used); 
        // -1 default value indicates that this information has not been provided
        private int xglobal = -1;
        private int yglobal = -1;

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
        // Vertex indices in the global coordinate axes
        public xyz[] getGlobalVertices(int x, int y)
        {
            // Set global coordinates and return the override function array
            this.xglobal = x;
            this.yglobal = y;
            return this.getGlobalVertices();
        }
        // Obtain global coordinates if xglobal & yglobal have been set
        public xyz[] getGlobalVertices()
        {
            xyz[] xyzs = new xyz[6];
            if(xglobal != -1 & yglobal != -1)
            {
                // Global info available, mapping each vertex shifted by corresponding tile origo
                xyzs = HexMapper.createGlobalHexVertexCoord(xglobal, yglobal);
            }
            else
            {
                // No global info provided, using local coordinates instead as if located at {0,0} in tile map
                xyzs = this.getVertices();
            }
            return xyzs;
        }
        // Local vertex indices
        public xyz[] getVertices()
        {
            return this.vertices;
        }
        public xyz getVertexAt(int i)
        {
            return this.vertices[i];
        }
        public double getElevation()
        {
            return this.elevation;
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
        public void setGlobalCoord(int x, int y)
        {
            this.xglobal = x;
            this.yglobal = y;
        }
        // Other methods

        // Override String representation of the Hex-object
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
