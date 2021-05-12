﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.AStar
{
    class Grid
    {

            public int Width = 100;
            public int Height = 50;
            private Node[,] nodes;

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
                        if ((x != 0 && y != 0) &&
                            rand.Next(0, 100) < 25)
                        {
                            nodes[x, y].Passable = false;
                        }

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

            public Node Node(int x, int y)
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                    return nodes[x, y];
                else
                    return null;
            }

            public void ResetState()
            {
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                    {
                        nodes[x, y].f = int.MaxValue;
                        nodes[x, y].g = 0;
                        nodes[x, y].h = 0;
                        nodes[x, y].cameFrom = null;
                        nodes[x, y].Path = false;
                        nodes[x, y].Open = false;
                        nodes[x, y].Closed = false;
                    }
            }
        }


    
}