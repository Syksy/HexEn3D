using System;

namespace HexEn3D
{
    class Hex
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
            for(int i=0; i<8; i++)
            {
                vertices[i] = new xyz(); // coords 0,0,0
            }
            this.elevation = 0.0; // By default on the z=0 plane
            this.active = true; // Created Hexes are active by default
        }
        public Hex(xyz[] xyzs, double elevation)
        {
            if (xyzs.Length != 8) throw new System.ArgumentOutOfRangeException("Parameter xyzs should be of length 8 in Hex class.");
            for (int i = 0; i < 8; i++)
            {
                vertices[i] = new xyz(); // coords from the provided table
            }
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


        // Setters
        public void setElevation(double elevation)
        {
            this.elevation = elevation;
        }
        public void setVertex(xyz xyztmp, int index)
        {
            if (index < 0 | index > 8) throw new System.ArgumentOutOfRangeException("Parameter index in Hyx.setVertex should be between 0 and 7.");
            this.vertices[index] = xyztmp;
        }
        public void setVertex(xyz[] xyztmp)
        {
            if(xyztmp.Length!=8) throw new System.ArgumentOutOfRangeException("Parameter xyztmp in Hyx.setVertex should be a xyz[] array of length 8.");
            this.vertices = xyztmp;
        }
    }
}
