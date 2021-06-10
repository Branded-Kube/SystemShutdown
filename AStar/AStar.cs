using System;
using System.Collections.Generic;

namespace SystemShutdown.AStar
{
    // Lead author: Ras
    class Astar
    {
        private List<Node> closedList = new List<Node>();
        private List<Node> openList = new List<Node>();
        private bool finished = true;
        /// <summary>
        /// Clears Astar open and closed lists and sets finished bool to false
        /// </summary>
        public void Start()
        {
            if (finished)
            {
                openList.Clear();
                closedList.Clear();
                finished = false;
            }
        }
        /// <summary>
        /// Get the surrounding 8 nodes to the node enemy is positioned at. Checks if they are inside of grid.
        /// If neighbor is in a corner, checks if there is a wall on either side and excludes the neighbor if there is. 
        /// This causes the enemy to not be able to walk "over" corners
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node[] GetNeighbors(Grid grid, Node node)
        {
            Node[] neighbors = { null, null, null, null, null, null, null, null };
            // Sets top neighbor
            // if y bigger than 0, (screen top border)
            if (node.Y - 1 > 0)
            {
                neighbors[0] = GameWorld.Instance.GameState.Grid.Node(node.X, node.Y - 1);
            }
            // Sets bottom neighbor
            // if y is less than grid height
            if (node.Y + 1 < grid.Height)
            {
                neighbors[1] = GameWorld.Instance.GameState.Grid.Node(node.X, node.Y + 1);
            }
            // Sets left neighbor
            // if x is bigger than 0 (screen left border)
            if (node.X - 1 > 0)
            {
                neighbors[2] = GameWorld.Instance.GameState.Grid.Node(node.X - 1, node.Y);
            }
            // Sets right neighbor
            // if x is less than width
            if (node.X + 1 < grid.Width)
            {
                neighbors[3] = GameWorld.Instance.GameState.Grid.Node(node.X + 1, node.Y);
            }
            // Sets top-left neighbor
            // if bigger than 0 (screen border) on both axis
            if (node.Y - 1 > 0 && node.X - 1 > 0)
            {
                if (!neighbors[0].Passable || !neighbors[2].Passable)
                {
                }
                else
                {
                    neighbors[4] = GameWorld.Instance.GameState.Grid.Node(node.X - 1, node.Y - 1);

                }
            }
            // Sets bottom-right neighbor
            // if less than grid height and width on both axis
            if (node.X + 1 < grid.Width && node.Y + 1 < grid.Height)
            {
                if (!neighbors[1].Passable || !neighbors[3].Passable)
                {
                }
                else
                {
                    neighbors[5] = GameWorld.Instance.GameState.Grid.Node(node.X + 1, node.Y + 1);
                }
            }
            // Sets bottom-left neighbor
            // if less than grid height and bigger than 0 (screen border)
            if (node.X - 1 > 0 && node.Y + 1 < grid.Height)
            {
                if (!neighbors[1].Passable || !neighbors[2].Passable)
                {
                }
                else
                {
                    neighbors[6] = GameWorld.Instance.GameState.Grid.Node(node.X - 1, node.Y + 1);
                }
            }
            // Sets top-right neighbor
            // if inside border
            if (node.X + 1 < grid.Width && node.Y - 1 > 0)
            {
                if (!neighbors[0].Passable || !neighbors[3].Passable)
                {
                }
                else
                {
                    neighbors[7] = GameWorld.Instance.GameState.Grid.Node(node.X + 1, node.Y - 1);
                }
               
            }
            return neighbors;
        }

        /// <summary>
        /// Heuristics used
        /// Euclidean Distance
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        private int EuclideanDistance(int x0, int y0, int x1, int y1)
        {
            int x = Math.Abs(x1 - x0);
            int y = Math.Abs(y1 - y0);
            return (int)Math.Sqrt(x * x + y * y);

        }


        /// <summary>
        ///  Sets "start" location for astar, finds and sets the node in the openlist with lowest f value to currentnode 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="path"></param>
        public void Search(Node start, Node end, Stack<Node> path)
        {
            start.F = EuclideanDistance(start.X, start.Y, end.X, end.Y);
            
            openList.Add(start);

            while (!finished)
            {
                // Finds the node with lowest f value in the openList and sets its number i nthe list to LowestIndex
                // if nodes share a lowest f value, set the node with highest g value to lowestIndex
                int lowestIndex = 0;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].F < openList[lowestIndex].F)
                    {
                        lowestIndex = i;
                    }

                    if (openList[i].F == openList[lowestIndex].F)
                    {
                        if (openList[i].G > openList[lowestIndex].G)
                        {
                            lowestIndex = i;
                        }
                    }
                }
                // current node is set to the node lowestIndex. (the node with lowest f value)
                // if list is empty, break loop.
                // If a enemy have a goal that is not reachable a index out of bounds happens. This breaks the loop instead and causes the enemy
                // to stand still until a new goal is given. 
                Node current = null;
                if (openList.Count > 0)
                {
                    current = openList[lowestIndex];

                }
                else
                {
                    break;
                }
                // if current node is equal to goal node, run finish 
                if (current == end)
                {
                    Finish(current, path);
                }
              
                // removes current node from openList and adds it to closedList, and flips sets bools open to false and closed to true.
                openList.Remove(current);
                closedList.Add(current);


                UpdateNeighbors(ref current, end);
            }

        }

        /// <summary>
        /// Updates neighbor list. 
        /// checks every neighbor in neighbors list and skips current neighbor if it is null, not !passable or in closedList.
        /// else sets current neighbor f / g / h values and adds neighbor to openlist
        /// </summary>
        /// <param name="current"></param>
        /// <param name="end"></param>
        private void UpdateNeighbors(ref Node current, Node end)
        {
            Node[] neighbors = GetNeighbors(GameWorld.Instance.GameState.Grid, current);
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
                else
                {
                    // (tmpG is the distance from start to neighbor though current)
                    int tmpG = current.G + 1;
                    // adds neighbor to open list if it is not added already
                    // else skips current iteration if neighbor g value is equal or lower than current g value + neighbor cost (path is not better)
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    else if (tmpG >= neighbor.G)
                    {
                        continue;
                    }

                    // if neighbor is added to open list and its g value is greater than tmpG  
                    // sets g / h and f values on neighbor, also sets Neighbors cameFrom node to current node 

                    neighbor.G = tmpG;

                    neighbor.H = EuclideanDistance(neighbor.X, neighbor.Y, end.X, end.Y);

                    neighbor.F = neighbor.G + neighbor.H;

                    neighbor.CameFrom = current;
                }
            }
        }

      
        /// <summary>.
        /// Uses Camefrom to track backwards from finish to start position and pushes the paths to the enemys stack of paths. 
        /// When start position is reached it has a null Camefrom node and while loop is finished.
        /// After pushing paths, openlist and closedlist are cleared and finished bool is sat to true
        /// </summary>
        /// <param name="current"></param>
        /// <param name="path"></param>
        private void Finish(Node current, Stack<Node> path)
        {
            while (current.CameFrom != null)
            {
                path.Push(current);
                current = current.CameFrom;
            }
            openList.Clear();
            closedList.Clear();
            finished = true;

        }


    }
}
