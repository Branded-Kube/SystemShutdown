using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.AStar
{
    //Ras
    public class Node : Component, IGameListener
    {

             public bool Passable = true;
            public bool Closed = false;
            public bool Open = false;
            //public bool Path = false;

            //public int Cost = 1;
           // public bool alreadyOccupied = false;

             public int x = 0;
            public int y = 0;
            public double f = 0;
            public int g = 0;
            public double h = 0;
            public Node cameFrom = null;
            //public Rectangle collisionRectangle;


        public override void Awake()
        {
            GameObject.Tag = "Node";
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
