using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.AStar
{
    public class Node : Component, IGameListener
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

        public override void Awake()
        {
            GameObject.Tag = "Node";

            //GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
            // this.position = GameObject.Transform.Position;
            // spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }
        public override string ToString()
        {
            return "Node";
        }
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {
                Debug.WriteLine("!");
            }
        }
    }
}
