using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.GameObjects;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown
{
    class Pickup : Component, IGameListener
    {
        private Mods floormod;
        private static Pickup instance;


        public static Pickup Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Pickup();
                }
                return instance;
            }
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            ((IGameListener)floormod).Notify(gameEvent, component);
        }

    }
}
