using System;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.AStar
{
    // Lead author: Ras
    public class Node : Component, IGameListener
    {
        #region Fields
        public bool Passable { get; set; } = true;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public double F { get; set; } = 0;
        public int G { get; set; } = 0;
        public double H { get; set; } = 0;
        public Node CameFrom { get; set; } = null;
        #endregion

        #region Methods
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
        #endregion
    }
}
