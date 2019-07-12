using System;
using System.Collections.Generic;

namespace HexEn3D
{
    public static class HexMapper
    {
        // Constants related to Hex positioning on {x,y} plane
        private static double R = 1.0, H = Math.Sqrt(3)*R, W = 2*R, S = (3/2)*R;
        // Vague archetypes for hex types
        public static Dictionary<string, int> hexTypes
            = new Dictionary<string, int>
            {
                { "UNDEFINED", 0 },
                { "LIGHT_FOREST", 1 },
                { "THICK_FOREST", 2 },
                { "PLAINS", 3 },
                { "RIVER", 4 },
                { "WATER", 5 }
            };
        public static double[] hexTypeMoveCoefs
            = new double[]
            {
                1.0,
                1.5,
                2.0,
                1.2,
                2.5,
                5.0           
            };
        /*      /-----\       =
         *     /r\     \      |
         *    /___\     \     | h (height)
         *    \         /     |
         *     \       /      |
         *    x \-----/       =
         * 
         *    |=======|
         *     s (short)
         *    
         *    |=========|
         *     w (width)
         *    
         *    |=|
         *     t
         *    
         *    r = e.g. 1 (scalar)
         *    w = 2*r
         *    s = (3/2)*r
         *    t = (1/2)*r
         *    h = sqrt(3)*r (etc, h/2 = 0.5*sqrt(3)*r)
         *    x = {0,0} coordinate origo
         */
        /* 2 dim map origos
         * 
         *         /-----\         /-----\         /-----\        
         *        /r\     \       /       \       /       \        
         *       /___\     \_____/         \_____/         \       
         *       \         /     \         /     \         /      
         *        \{ 0, 3}/       \{ 2, 3}/       \{ 4, 3}/        
         *         \-----/         \-----/         \-----/         
         *         /     \         /     \         /     \         
         *        /       \{ 1, 2}/       \{ 3, 2}/       \       
         *       /         \_____/         \_____/         \     
         *       \         /     \         /     \         /     
         *        \{ 0, 2}/       \{ 2, 2}/       \{ 4, 2}/      
         *         \-----/         \-----/         \-----/       
         *         /     \         /     \         /     \      
         *        /       \{ 1, 1}/       \{ 3, 1}/       \      
         *       /         \_____/         \_____/         \     
         *       \         /     \         /     \         /     
         *        \{ 0, 1}/       \{ 2, 1}/       \{ 4, 1}/     
         *         \-----/         \-----/         \-----/                      =
         *         /     \         /     \         /     \                      |                 . multiples of 0.5*sqrt(3)*r
         *        /       \{ 1, 0}/       \{ 3, 0}/       \                     |                 .
         *       /         \_____/         \_____/         \   =                | sqrt(3)*r = h   .
         *       \         /     \         /     \         /   |                |                 .
         *        \{ 0, 0}/       \{ 2, 0}/       \{ 4, 0}/    | 0.5*sqrt(3)*r  |                 .
         *         \-----/         \-----/         \-----/     =                =                 =
         *         
         *       |=======|
         *           s
         *       |===============|
         *           w + r
         *       |=======================|
         *           w + s + r
         *       |===============================|
         *           w + w + r + r              
         *       |=======================================|
         *           w + w + r + r + s
         *       
         *       origin pseudocode for hex at { x, y} in world coordinates {a , b} on the horizontal plane
         *       {x=0,y=0} -> {a=0,b=0} else
         *       
         *       a <- if(x%%2==0){ % even x in {2,4,6, ...}
         *              (x/2)*w + (x/2)*r
         *            }else{ % uneven x in {1,3,5,7, ...}
         *              ((x-1)/2)*w + ((x-1)/2)*r + s
         *            }
         *       b <- if(x%%2==0){ % even x in {2,4,6, ...}
         *              y*(sqrt(3)*r)
         *            }else{ % uneven x in {1,3,5,7, ...}
         *              y*(sqrt(3)*r) + 0.5*sqrt(3)*r
         *            }
         *            
         *    r = e.g. 1 (scalar)
         *    w = 2*r
         *    s = (3/2)*r
         *    t = (1/2)*r
         *    h = sqrt(3)*r (etc, h/2 = 0.5*sqrt(3)*r)
         *    x = {0,0} coordinate origo
         *                
         */

        // Get movement cost for moving from Hex a to Hex b
        public static double getMoveCost(Hex a, Hex b)
        {
            double coef1 = getMoveCoef(a); // Movement cost multiplier from terrain type in a
            double coef2 = getMoveCoef(b); // Movement cost multiplier from terrain type in b
            double elevChange = getElevationDifference(a, b);
            double elevCoef = 0.0;
            if(elevChange >= 0) // going uphill or staying at same plane
            {
                elevCoef = 1.0 + elevChange;
            }
            else // going downhill
            {
                elevCoef = 1.0 / (1.0 + Math.Abs(elevChange));
            }
            return elevCoef*(coef1 + coef2);
        }

        // Get movement coefficient for a certain hexType
        public static double getMoveCoef(string hexType)
        {
            return hexTypeMoveCoefs[hexTypes[hexType]];
        }
        public static double getMoveCoef(Hex hex)
        {
            return hexTypeMoveCoefs[hexTypes[hex.getHexType()]];
        }
        // Get the elevation difference between two hexes; posive means going uphill from a to b
        public static double getElevationDifference(Hex a, Hex b)
        {
            return a.getElevation() - b.getElevation();
        }
        // Overloading with a single global hex
        public static xyz createGlobalOrigoMap(int x, int y)
        {
            xyz tmpxyz = new xyz();
            //if (x != 0 & y != 0)
            //{
                // x-coordinate dictates the alternating heights
                if (x % 2 == 0) // x-coord in {2,4,6,8, ...}
                {
                    tmpxyz.setX(((x / 2) * W) + ((x / 2) * R));
                    tmpxyz.setY(y * H);
                }
                else // x-coord in {1,3,5,7, ...}
                {
                    tmpxyz.setX((((x - 1) / 2) * W) + (((x - 1) / 2) * R) + ((3.0/2.0) * R));
                    tmpxyz.setY((y * H) + (0.5 * H));
                }
            //}
            return tmpxyz;
        }
        // {x,y} are the global origo coordinates; z-axis is omitted as presumably the Hex map is a single molded plane
        public static xyz[] createGlobalHexVertexCoord(int x, int y)
        {
            xyz globalCoord = createGlobalOrigoMap(x, y);
            // Hexagon has 6 coordinates for vertices; format array with origos
            xyz[] xyzs = new xyz[7] { globalCoord, globalCoord, globalCoord, globalCoord, globalCoord, globalCoord, globalCoord};
            xyz[] localshift = createLocalHexVertexCoord();
            for(int i=0; i<7; i++)
            {
                // Append local shift for specific vertex based on local shift to create a hexagon
                xyzs[i] += localshift[i];
            }
            return xyzs;
        }

        public static xyz[] createLocalHexVertexCoord()
        {
            // Hexagon has 6 coordinates for vertices; format array with {0,0,0}s
            xyz[] xyzs = new xyz[7] { new xyz(), new xyz(), new xyz(), new xyz(), new xyz(), new xyz(), new xyz()};
            /*  1. /-----\ 2.
             *    / \   / \
             *0. /___\6/___\ 3.
             *   \   / \   /
             *    \ /   \ /
             *  5. \-----/ 4.
             */
            // Vector operations for +/- defined in xyz.cs
            // Vertex #0
            xyzs[0] += new xyz(0.0 * R, 0.5 * Math.Sqrt(3) * R, 0.0);
            // Vertex #1
            xyzs[1] += new xyz(0.5 * R, 1.0 * Math.Sqrt(3) * R, 0.0);
            // Vertex #2
            xyzs[2] += new xyz(1.5 * R, 1.0 * Math.Sqrt(3) * R, 0.0);
            // Vertex #3
            xyzs[3] += new xyz(2.0 * R, 0.5 * Math.Sqrt(3) * R, 0.0);
            // Vertex #4
            xyzs[4] += new xyz(1.5 * R, 0.0 * Math.Sqrt(3) * R, 0.0);
            // Vertex #5
            xyzs[5] += new xyz(0.5 * R, 0.0 * Math.Sqrt(3) * R, 0.0);
            // Central vertex #6
            xyzs[6] += new xyz(1.0 * R, 0.5 * Math.Sqrt(3) * R, 0.0);
            // Return the local shift for each vertex as an array of length 7
            return xyzs;
        }
    }
}

