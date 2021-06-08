using Microsoft.Xna.Framework;
using System;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;

namespace SystemShutdown.AStar
{
    //Ras
    public class Grid
    {
        private Node[,] nodes;
        public int NodeSize { get; set; } = 100;
        public int Height { get; set; } = 35;
        public int Width { get; set; }= 35;

        /// <summary>
        /// Creates a grid of nodes.
        /// Random chance to set a node to not passable. 
        /// Gameobject node is created at Not passable nodes. (Walls) 
        /// Walls are cleared or manually created at specific node positions.
        /// </summary>
        public Grid()
        {
            Random rand = new Random();
            nodes = new Node[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    // Creates a grid of nodes each with a x.y cordinate
                    nodes[x, y] = new Node();
                    nodes[x, y].X = x;
                    nodes[x, y].Y = y;

                    //random walls, 30% to make a node not passable (wall)
                    if ((x != 1 && y != 1) && (x != Width - 2 && y != Height - 2) &&
                        rand.Next(1, 100) < 31)
                    {
                        nodes[x, y].Passable = false;
                    }

                    // clears mid square for walls
                    if (x > 12 && x < 22 && y > 12 && y < 22)
                    {
                        nodes[x, y].Passable = true;
                    }

                    // clears Corners 2x2 (enemy spawn area)
                    if (x <= 2 && y <= 2 || x <= 2 && y >= Height - 3 || x >= Width - 3 && y <= 2 || x >= Width - 3 && y >= Height - 3)
                    {
                        nodes[x, y].Passable = true;
                    }

                    // sets mid square borders
                    // Bottom
                    if (x > Width / 2 - 5 && x < Width / 2 + 5 && y == Height / 2 + 5)
                    {
                        nodes[x, y].Passable = false;
                    }
                    // Top
                    if (x > Width / 2 - 5 && x < Width / 2 + 5 && y == Height / 2 - 5)
                    {
                        nodes[x, y].Passable = false;
                    }
                    // Left
                    if (y > Height / 2 - 5 && y < Height / 2 + 5 && x == Width / 2 - 5)
                    {
                        nodes[x, y].Passable = false;
                    }
                    // Right
                    if (y > Height / 2 - 5 && y < Height / 2 + 5 && x == Width / 2 + 5)
                    {
                        nodes[x, y].Passable = false;
                    }

                    // Clears a line from mid to each side from walls
                    // Top and Bottom
                    if (x == Width / 2 && y < Height && y > 0)
                    {
                        nodes[x, y].Passable = true;
                    }
                    // Left and Right
                    if (y == Height / 2 && x < Width && x > 0)
                    {
                        nodes[x, y].Passable = true;
                    }
                    // Creates a node gameobject (wall )for each node at outerborder
                    if (y == 0 || y == Height - 1 || x == 0 || x == Width - 1)
                    {
                        nodes[x, y].Passable = false;
                    }
                    // Creates a node gameobject (wall) for each node that are not passable
                    if (nodes[x, y].Passable == false)
                    {
                        GameObject1 nodeGO = new GameObject1();
                        SpriteRenderer nodeSR = new SpriteRenderer("Textures/wall");
                        nodeGO.AddComponent(nodeSR);
                        nodeGO.Transform.Position = new Vector2(x * 100, y * 100);
                        nodeSR.Origin = new Vector2(nodeSR.Sprite.Width / 2, (nodeSR.Sprite.Height) / 2);
                        nodeGO.AddComponent(new Collider(nodeSR, nodes[x, y]) { CheckCollisionEvents = false });
                        nodeGO.AddComponent(nodes[x, y]);
                        GameWorld.Instance.GameState.AddGameObject(nodeGO);
                    }

                }

            // fills out any passable node completely surrounded by walls 
            foreach (var item in nodes)
            {
                Node nodesLeft = new Node();
                Node nodesRight = new Node();
                Node nodesTop = new Node();
                Node nodesBottom = new Node();

                nodesLeft = Node(item.X - 1, item.Y);
                nodesRight = Node(item.X + 1, item.Y);
                nodesTop = Node(item.X, item.Y - 1);
                nodesBottom = Node(item.X, item.Y + 1);
                if (nodesLeft != null)
                {
                    if (nodesLeft == null || nodesLeft.Passable == false)
                    {
                        if (nodesRight == null || nodesRight.Passable == false)
                        {
                            if (nodesTop == null || nodesTop.Passable == false)
                            {
                                if (nodesBottom == null || nodesBottom.Passable == false)
                                {
                                    item.Passable = false;
                                    GameObject1 nodeGO = new GameObject1();
                                    SpriteRenderer nodeSR = new SpriteRenderer("Textures/wall");
                                    nodeGO.AddComponent(nodeSR);
                                    nodeGO.Transform.Position = new Vector2(item.X * 100, item.Y * 100);
                                    nodeSR.Origin = new Vector2(nodeSR.Sprite.Width / 2, (nodeSR.Sprite.Height) / 2);

                                    nodeGO.AddComponent(new Collider(nodeSR, nodes[item.X, item.Y]) { CheckCollisionEvents = false });
                                    nodeGO.AddComponent(nodes[item.Y, item.Y]);
                                    GameWorld.Instance.GameState.AddGameObject(nodeGO);
                                }
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Gets a node at position x.y in grid. Else return null
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Node Node(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return nodes[x, y];
            else
                return null;
        }
        /// <summary>
        /// Resets each nodes values 
        /// </summary>
        public void ResetState()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    nodes[x, y].F = int.MaxValue;
                    nodes[x, y].G = 0;
                    nodes[x, y].H = 0;
                    nodes[x, y].CameFrom = null;
                }
        }
    }
}
