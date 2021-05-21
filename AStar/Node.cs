using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.AStar
{
    public class Node
    {
            public bool Passable = true;
            public bool Closed = false;
            public bool Open = false;
            public bool Path = false;

            public int Cost = 1;
           // public bool alreadyOccupied = false;

             public int x = 0;
            public int y = 0;
            public double f = 0;
            public int g = 0;
            public double h = 0;
                // position til detection af walls imellem player og enemy
            public Vector2 position;

             public Node cameFrom = null;
            public Rectangle collisionRectangle;

        public void rectangle(Point position)
        {
            this.position = new Vector2(position.X, position.Y);
            collisionRectangle = new Rectangle(position, new Point(Grid.NodeSize, Grid.NodeSize));
            
        }
    }
}
