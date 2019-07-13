using System;

namespace HexEn3D
{
    // Path is a collection of adjacent hexes that create e.g. an optimal path in minimizing required movement effort
    public class Path
    {
        // Desired path and corresponding map
        private HexMap map; // The hex map in which one may move
        private int x0, x1, y0, y1; // 0 are origins, 1 are destination coordinates
        // Computed costs or path
        private double movementCost = 0.0; // Total movement cost from moving from point a to point b
        private int hexSteps = 0; // Total count of hexes traversed
        private Hex[] hexPath; // Hexes in sequence
        private int currentIter = 1; // Current i:th step
        private int maxIter = 100; // Maximum hex distance travel allowed
        private bool finished = false; // haven't reached destination yet

        // Constructors

        public Path(HexMap map, int x0, int y0, int x1, int y1)
        {
            this.map = map;
            // Initial step is at {x0,y0}
            // First iter at origin
            hexPath = new Hex[maxIter];
            hexPath[0] = map.getHexAt(x0,y0);
            // Set origin and destination
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
            // Steps until we reach max iterations or reach destination
            while(currentIter < maxIter & !finished)
            {
                bruteWalkStep();
            }
        }

        // Setters

        // Getters

        // Get the array of Hex-class objects in the path
        public Hex[] getHexPath()
        {
            return hexPath;
        }
        // Return total movement cost
        public double getPathMovementCost()
        {
            return movementCost;
        }
        // Return whether we reached destination
        public bool getFinished()
        {
            return finished;
        }
        // Return whether the iterator reached max iterations       
        public bool getAtMaxIter()
        {
            return currentIter == maxIter;
        }
        // Return amount of hex steps taken during the path
        public int getHexSteps()
        {
            return hexSteps;
        }


        // Other methods

        // Simple brute force algorithm that tries to walk from origin towards destination by a single step
        public void bruteWalkStep()
        {
            int currentx = hexPath[currentIter - 1].getGlobalX()-1;
            int currenty = hexPath[currentIter - 1].getGlobalY()-1;
            // Stepping towards destination
            // Even x
            if (currentx % 2 == 1)
            {
                if (currentx < x1 & currenty == y1)
                {
                    addStep(currentx, currenty, currentx + 1, currenty);
                }
                else if (currentx > x1 & currenty == y1)
                {
                    addStep(currentx, currenty, currentx - 1, currenty);
                }
                else if (currentx == x1 & currenty < y1)
                {
                    addStep(currentx, currenty, currentx, currenty + 1);
                }
                else if (currentx == x1 & currenty > y1)
                {
                    addStep(currentx, currenty, currentx, currenty - 1);
                }
                else if (currentx < x1 & currenty < y1)
                {
                    // Illegal move in a hexagonal grid!
                    //addStep(currentx, currenty, currentx + 1, currenty + 1);
                    addStep(currentx, currenty, currentx + 1, currenty);
                }
                else if (currentx > x1 & currenty > y1)
                {
                    addStep(currentx, currenty, currentx - 1, currenty - 1);
                }
                else if (currentx < x1 & currenty > y1)
                {
                    addStep(currentx, currenty, currentx + 1, currenty - 1);
                }
                else if (currentx > x1 & currenty < y1)
                {
                    // Illegal move in a hexagonal grid!
                    //addStep(currentx, currenty, currentx - 1, currenty + 1);
                    addStep(currentx, currenty, currentx - 1, currenty);
                }
                else
                {
                    finished = true; // Reached destination
                }
            }
            else // x is odd
            {
                if (currentx < x1 & currenty == y1)
                {
                    addStep(currentx, currenty, currentx + 1, currenty);
                }
                else if (currentx > x1 & currenty == y1)
                {
                    addStep(currentx, currenty, currentx - 1, currenty);
                }
                else if (currentx == x1 & currenty < y1)
                {
                    addStep(currentx, currenty, currentx, currenty + 1);
                }
                else if (currentx == x1 & currenty > y1)
                {
                    addStep(currentx, currenty, currentx, currenty - 1);
                }
                else if (currentx < x1 & currenty < y1)
                {
                    addStep(currentx, currenty, currentx + 1, currenty + 1);
                }
                else if (currentx > x1 & currenty > y1)
                {
                    // Illegal move in a hexagonal grid!
                    //addStep(currentx, currenty, currentx - 1, currenty - 1);
                    addStep(currentx, currenty, currentx - 1, currenty);
                }
                else if (currentx < x1 & currenty > y1)
                {
                    // Illegal move in a hexagonal grid!
                    //addStep(currentx, currenty, currentx + 1, currenty - 1);
                    addStep(currentx, currenty, currentx + 1, currenty);
                }
                else if (currentx > x1 & currenty < y1)
                {
                    addStep(currentx, currenty, currentx - 1, currenty + 1);
                }
                else
                {
                    finished = true; // Reached destination
                }
            }
        }
        // Add a new step from {xorigin,yorigin} to {xdest,ydest}
        public void addStep(int xorigin, int yorigin, int xdest, int ydest)
        {
            if (currentIter < maxIter)
            {
                hexPath[currentIter] = map.getHexAt(xdest, ydest);
                addMovementCost(HexMapper.getMoveCost(map.getHexAt(xorigin, yorigin), map.getHexAt(xdest, ydest)));
                currentIter++;
                hexSteps++;
            }
        }
        // Add new cost to the existing movement cost
        public void addMovementCost(double addCost)
        {
            movementCost += addCost;
        }

        // Override String representation of the Path-object
        public override String ToString()
        {
            string str = "Path for a HexMap (of total movement cost " + movementCost + ", iterations " + currentIter + " of maxIter " + maxIter + ");\n";
            for (int i = 0; i <= hexSteps; i++)
            {
                str += "step " + i + " hex: " + hexPath[i] + "\n";
            }
            return str;
        }
    }
}