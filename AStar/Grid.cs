using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;

namespace SystemShutdown.AStar
{
    public class Grid
    {
        public static int NodeSize = 100;

        public int Width = 35;
            public int Height = 35;
            public Node[,] nodes;
            
            public Grid()
            {
                Random rand = new Random();
                nodes = new Node[Width, Height];
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                    {
                        nodes[x, y] = new Node();
                        nodes[x, y].x = x;
                        nodes[x, y].y = y;
                    //


                    //


                    if ((x != 0 && y != 0) && (x != Width -1 && y != Height -1) &&
                        rand.Next(1, 25) < 5)
                    {
                        nodes[x, y].Passable = false;
                    }

                    // for testing only clears y 2 where player spawns
                    if (y == 2 )
                    {
                        nodes[x, y].Passable = true;
                    }


                    //if (y == 0 || y == Height - 1)
                    //{
                    //    nodes[x, y].Passable = false;
                    //}
                    //if (x == 0 || x == Width - 1)
                    //{
                    //    nodes[x, y].Passable = false;
                    //}

                    if (nodes[x, y].Passable == false)
                    {
                        GameObject1 go = new GameObject1();
                        SpriteRenderer sr = new SpriteRenderer();
                        go.AddComponent(sr);
                        //go.Transform.Position = new Vector2(rnd.Next(0, GameWorld.Instance.GraphicsDevice.Viewport.Width), 0);
                        go.Transform.Position = new Vector2(x * 100, y * 100);

                        sr.SetSprite("1GuyUp");
                        // nodes[x, y] = new Node(new Rectangle(new Point(100, 100), new Point(100, 100)));
                        go.AddComponent(new Collider(sr, nodes[x, y]) { CheckCollisionEvents = true });
                        go.AddComponent(nodes[x, y]);
                        GameWorld.gameState.AddGameObject(go);
                    }



                    //if (nodes[x, y].Passable == false)
                    //{
                    //    nodes[x, y].rectangle(new Point(x, y));
                    //}



                    //if (x > Width / 4 && x < 7 * Width / 8 && y == Height / 2)
                    //{
                    //    nodes[x, y].Passable = false;
                    //}
                    //if (x == 7 * Width / 8 && y < 7 * Height / 8 && y > Height / 4)
                    //{
                    //    nodes[x, y].Passable = false;
                    //}
                }
        }

            //public Node Node(int x, int y)
            //{
            //    if (x >= 0 && x < Width && y >= 0 && y < Height)
            //        return nodes[x, y];
            //    else
            //        return null;
            //}

            //public void ResetState()
            //{
            //    for (int y = 0; y < Height; y++)
            //        for (int x = 0; x < Width; x++)
            //        {
            //            nodes[x, y].f = int.MaxValue;
            //            nodes[x, y].g = 0;
            //            nodes[x, y].h = 0;
            //            nodes[x, y].cameFrom = null;
            //            nodes[x, y].Path = false;
            //            nodes[x, y].Open = false;
            //            nodes[x, y].Closed = false;
            //        }
            //}
        }


    
}
