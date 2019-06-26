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
        private xyz corner0;
        private xyz corner1;
        private xyz corner2;
        private xyz corner3;
        private xyz corner4;
        private xyz corner5;
        // Additionally, for 3d plotting the mesh triangles require two additional points inside
        private xyz corner6;
        private xyz corner7;


    }
}
