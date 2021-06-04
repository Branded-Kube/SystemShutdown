using Microsoft.Xna.Framework;
using System;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.AStar
{
    //Ras
    public class Node : Component, IGameListener
    {
        public int Y { get; set; } = 0;
        public int X { get; set; } = 0;
        public bool Open { get; set; } = false;
        public bool Closed { get; set; } = false;
        public bool Passable { get; set; } = false;
        public double F { get; set; } = 0;
        public int G { get; set; } = 0;
        public double H { get; set; } = 0;
        public Node CameFrom { get; set; } = null;

        public override void Awake()
        {
            GameObject.Tag = "Node";
        }
        public override string ToString()
        {
            return "Node";
        }
        /// <summary>
        /// Not used, player is responsible for handling of collision to disable collision checks between nodes (walls)
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        public void Notify(GameEvent gameEvent, Component component)
        {
        }
    }
}
