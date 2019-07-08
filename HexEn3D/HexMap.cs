using System;

namespace HexEn3D
{
    public class HexMap
    {
        // Hex has 6 corners + inner vertex
        /*  1. /-----\ 2.
         *    / \   / \
         *0. /___\6/___\ 3.
         *   \   / \   /
         *    \ /   \ /
         *  5. \-----/ 4.
         */
         // Map coordinates {x,y}
         /*         -----           -----           -----
         *         /     \         /     \         /     \
         *        /       \       /       \       /       \
         *       /         \_____/         \_____/         \
         *       \         /     \         /     \         /
         *        \{ 0, 2}/       \{ 2, 2}/       \{ 4, 2}/
         *         \-----/         \-----/         \-----/
         *         /     \         /     \         /     \
         *        /       \{ 1, 1}/       \{ 3, 1}/       \
         *       /         \_____/         \_____/         \
         *       \         /     \         /     \         /
         *        \{ 0, 1}/       \{ 2, 1}/       \{ 4, 1}/
         *         \-----/         \-----/         \-----/
         *         /     \         /     \         /     \
         *        /       \{ 1, 0}/       \{ 3, 0}/       \
         *       /         \_____/         \_____/         \
         *       \         /     \         /     \         /
         *        \{ 0, 0}/       \{ 2, 0}/       \{ 4, 0}/
         *         \-----/         \-----/         \-----/
         */

        private Hex[,] map; // 2-dimensional Hex grid, z-axis to be determined by each hex
        // Storing effective {x,y} size because stored map has buffer of 1 tiles on all sides
        private int xsize;
        private int ysize;

        // Constructors

        public HexMap(int xsize, int ysize)
        {
            map = new Hex[xsize+2,ysize+2];
            for(int i = 0; i < xsize+2; i++)
            {
                for(int j = 0; j < ysize+2; j++)
                {
                    map[i, j] = new Hex();
                    map[i, j].setGlobalCoord(i, j);
                }
            }
            this.xsize = xsize;
            this.ysize = ysize;
        }
        public HexMap(int xsize, int ysize, double elevation)
        {
            map = new Hex[xsize+2,ysize+2];
            for (int i = 0; i < xsize+2; i++)
            {
                for (int j = 0; j < ysize+2; j++)
                {
                    map[i, j] = new Hex(elevation);
                    map[i, j].setGlobalCoord(i, j);
                }
            }
            this.xsize = xsize;
            this.ysize = ysize;
        }

        // Getters
        public xyz[] getGlobalVerticesAtMap(int x, int y)
        {
            // Skip buffers but act as if the origo was at {0,0}
            return map[x + 1, y + 1].getGlobalVertices(x, y);
        }
        public Hex[,] getMap()
        {
            return map;
        }
        public Hex getHexAt(int x, int y)
        {
            return map[x + 1, y + 1];
        }
        public int getXSize()
        {
            return xsize;
        }
        public int getYSize()
        {
            return ysize;
        }
        // Setters
        public void setElevationAt(int x, int y, double elevation)
        {
            // Adjusting per 1 tile buffer on sides
            map[x + 1,y + 1].setElevation(elevation);
        }
        public void setHexAt(int x, int y, Hex hex)
        {
            // Adjusting per 1 tile buffer on sides
            map[x + 1, y + 1] = hex;
        }

        // Other methods
        public void computeElevationCorners()
        {
            /*   1./-----\ 2.
             *    / \   / \
             *0. /___\_/___\ 3.
             *   \   / \   /
             *    \ /   \ /
             *  5. \-----/ 4.
             */
            // Add 1 buffer time to sides to avoid excess special cases
            // Do not compute elevations for the buffer sides
            for (int i=1; i<xsize+1; i++)
            {
                for(int j=1; j<ysize+1; j++)
                {
                    if (i % 2 == 0) // x coordinate is even
                    {
                        // Corner #0 - to the left
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i-1,j+0].getElevation() + map[i-1,j-1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            0 // Vertex index
                        );
                        // Corner #1 - to the top-left
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i-1,j+0].getElevation() + map[i+0,j+1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            1 // Vertex index
                        );
                        // Corner #2 - to the top-right
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+0,j+1].getElevation() + map[i+1,j+0].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            2 // Vertex index
                        );
                        // Corner #3 - to the right
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+1,j+0].getElevation() + map[i+1,j-1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            3 // Vertex index
                        );
                        // Corner #4 - to the bottom-right
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+1,j-1].getElevation() + map[i+0,j-1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            4 // Vertex index
                        );
                        // Corner #5 - to the bottom-left
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i-1,j-1].getElevation() + map[i+0,j-1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            5 // Vertex index
                        );
                    }
                    else // x coordinate is odds
                    {
                        // Corner #0 - to the left
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i-1,j+0].getElevation() + map[i-1,j+1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            0 // Vertex index
                        );
                        // Corner #1 - to the top-left
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i-1,j+1].getElevation() + map[i+0,j+1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            1 // Vertex index
                        );
                        // Corner #2 - to the top-right
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+0,j+1].getElevation() + map[i+1,j+1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            2 // Vertex index
                        );
                        // Corner #3 - to the right
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+1,j+1].getElevation() + map[i+1,j+0].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            3 // Vertex index
                        );
                        // Corner #4 - to the bottom-right
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+1,j+0].getElevation() + map[i+0,j-1].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            4 // Vertex index
                        );
                        // Corner #5 - to the bottom-left
                        map[i,j].setVertexZAt(
                            // New elevation
                            (map[i,j].getElevation() // Self
                                + map[i+0,j-1].getElevation() + map[i-1,j+0].getElevation()) // Neighbors                                
                                / 3.0, // Average at the triangle junction
                            5 // Vertex index
                        );
                    }
                }
            }
        }
        // Sets the border buffer hexes to disabled
        public void disableSides()
        {
            for(int i = 0; i < xsize + 2; i++)
            {
                // Bottom
                map[i, 0].setActive(false);
                // Top
                map[i, ysize+2].setActive(false);
            }
            for (int j = 0; j < ysize + 2; j++)
            {
                // Left
                map[0, j].setActive(false);
                // Right
                map[xsize+2, j].setActive(false);
            }
        }

        // Override String representation of the HexMap-object
        public override String ToString()
        {
            string str = "HexMap;\n";
            for (int i = 0; i < xsize+2; i++)
            {
                for (int j = 0; j < ysize + 2; j++)
                {
                    str += "Hex at {x=" + i + ",y=" + j + "}:\n" + map[i,j];
                }
            }
            return str;
        }
    }
}
