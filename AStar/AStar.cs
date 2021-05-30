using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.GameObjects;

namespace SystemShutdown.AStar
{
   // Ras
    class Astar
    {
        private List<Node> closedList = new List<Node>();
        private List<Node> openList = new List<Node>();
        private Node[,] enemyNodes;

        bool finished = true;
        
        public Astar()
        {
            enemyNodes = GameWorld.gameState.grid.nodes;
        }


        public void Start(Node start)
        {
            if (finished)
            {
                openList.Clear();
                closedList.Clear();
                openList.Add(start);
                finished = false;

            }
        }
    
        private Node[] GetNeighbors(Grid grid, Node node)
        {

            Node[] neighbors = { null, null, null, null, null, null, null, null };
            // Sets top neighbor
            // if y bigger than 0, (screen top border)
            if (node.y - 1 > 0)
            {
                neighbors[0] = GameWorld.gameState.grid.Node(node.x, node.y - 1);
            }
            // Sets bottom neighbor
            // if y is less than grid height
            if (node.y + 1 < grid.Height)
            {
                neighbors[1] = GameWorld.gameState.grid.Node(node.x, node.y + 1);
            }
            // Sets left neighbor
            // if x is bigger than 0 (screen left border)
            if (node.x - 1 > 0)
            {
                neighbors[2] = GameWorld.gameState.grid.Node(node.x - 1, node.y);
            }
            // Sets right neighbor
            // if x is less than width
            if (node.x + 1 < grid.Width)
            {
                neighbors[3] = GameWorld.gameState.grid.Node(node.x + 1, node.y);
            }
            // Sets top-left neighbor
            // if bigger than 0 (screen border) on both axis
            if (node.y - 1 > 0 && node.x - 1 > 0)
            {
                if (!neighbors[0].Passable && !neighbors[2].Passable)
                {
                }
                else
                {
                    neighbors[4] = GameWorld.gameState.grid.Node(node.x - 1, node.y - 1);

                }
            }
            // Sets bottom-right neighbor
            // if less than grid height and width on both axis
            if (node.x + 1 < grid.Width && node.y + 1 < grid.Height)
            {
                if (!neighbors[1].Passable && !neighbors[3].Passable)
                {
                }
                else
                {
                    neighbors[5] = GameWorld.gameState.grid.Node(node.x + 1, node.y + 1);
                }
            }
            // Sets bottom-left neighbor
            // if less than grid height and bigger than 0 (screen border)
            if (node.x - 1 > 0 && node.y + 1 < grid.Height)
            {
                if (!neighbors[1].Passable && !neighbors[2].Passable)
                {
                }
                else
                {
                    neighbors[6] = GameWorld.gameState.grid.Node(node.x - 1, node.y + 1);
                }
            }
            // Sets top-right neighbor
            // if inside border
            if (node.x + 1 < grid.Width && node.y - 1 > 0)
            {
                if (!neighbors[0].Passable && !neighbors[3].Passable)
                {
                }
                else
                {
                    neighbors[7] = GameWorld.gameState.grid.Node(node.x + 1, node.y - 1);
                }
               
            }
            return neighbors;
        }


        // Heuristics used
        // Euclidean Distance
        private int EuclideanDistance(int x0, int y0, int x1, int y1)
        {
            int x = Math.Abs(x1 - x0);
            int y = Math.Abs(y1 - y0);
            return (int)Math.Sqrt(x * x + y * y);
        }


        /// <summary>
        ///  Sets "start" location for astar, finds and sets the node in the openlist with lowest f value to currentnode 
        ///  
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="path"></param>
        public void Search(Grid grid, Node start, Node end, Stack<Node> path)
        {

            start.f = EuclideanDistance(start.x, start.y, end.x, end.y);

            while (!finished)
            {
                // Sets lowestIndex to the node with the lowest f value
                int lowestIndex = 0;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].f < openList[lowestIndex].f)
                    {
                        lowestIndex = i;
                    }

                    if (openList[i].f == openList[lowestIndex].f)
                    {
                        if (openList[i].g > openList[lowestIndex].g)
                        {
                            lowestIndex = i;
                        }
                    }
                }


                // current node is set to the node with the lowest f value in openlist
                Node current = openList[lowestIndex];
                if (current == end)
                {
                    Finish(current, path);
                }
              
                // removes current node from openList and adds it to closedList, flips bools 
                openList.Remove(current);
                closedList.Add(current);

                current.Open = false;
                current.Closed = true;

                UpdateNeighbors(ref current, grid, end);


                // solution not found, return path to node with greatest g value

                if (closedList.Count > (grid.Width * grid.Height) / 4)
                {
                    int greatestG = 0;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList[i].g > openList[greatestG].g)
                        {
                            greatestG = i;
                        }

                    }
                    current = openList[greatestG];
                    Finish(current, path);

                    return;
                }

            }

        }


        private void UpdateNeighbors(ref Node current, Grid grid, Node end)
        {

            Node[] neighbors = GetNeighbors(grid, current);

            // checks every neighbor in neighbors list and skips current neighbor if it is null, not !passable or in closedList.
            // else sets current neighbor f / g / h values and adds neighbor to openlist
            foreach (Node neighbor in neighbors)
            {
                
                if (neighbor == null)
                {
                    continue;
                }
                else if (!neighbor.Passable)
                {
                    continue;
                }
                else if (closedList.Contains(neighbor))
                {
                    continue;
                }
                //else if (neighbor.alreadyOccupied)
                //{
                //    continue;
                //}
                else
                {
                    // (tmpG is the distance from start to neighbor though current)
                    int tmpG = current.g + neighbor.Cost;

                    // adds neighbor to open list if it is not added already
                    // else skips if neighbor g value is equal or lower than current g value + neighbor cost (path is not better)
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    else if (tmpG >= neighbor.g)
                    {
                        continue;
                    }

                    // if neighbor is already added to open list and its g value is greater than tmpG  
                    // set g / h and f values on neighbor, also sets cameFrom node to current node 

                    neighbor.g = tmpG;

                    neighbor.h = EuclideanDistance(neighbor.x, neighbor.y, end.x, end.y);

                    neighbor.f = neighbor.g + neighbor.h;

                    neighbor.cameFrom = current;
                }
            }
        }

      
        /// <summary>
        /// Stops the astar, 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="path"></param>
        private void Finish(Node current, Stack<Node> path)
        {
            while (current.cameFrom != null)
            {
                current.Path = true;
                path.Push(current);
                current = current.cameFrom;

            }
            openList.Clear();
            closedList.Clear();
            finished = true;

        }


    }
}
