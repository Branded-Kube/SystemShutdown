using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.FactoryPattern
{
    class Projectile2 : Component, IGameListener
    {
        private float speed;
        public Vector2 velocity;

        public float Height { get; set; }
        public Projectile2(float speed)
        {
            this.speed = speed;
            //this.velocity = velocity;

          

        }
        public override string ToString()
        {
            return "Laser";
        }

        public override void Awake()
        {
            GameObject.Tag = "Laser";
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            Destroy1();
        }
        private void Move()
        {
            GameObject.Transform.Translate(velocity * speed *  GameWorld.DeltaTime);

            


        }

        private void Destroy1()
        {
            if (GameObject.Transform.Position.Y <= -Height)
            {
                GameObject.Destroy();
            }
        }

        public override void Destroy()
        {
            GameWorld.gameState.Colliders.Remove((Collider)GameObject.GetComponent("Collider"));

        }

        public Projectile2 Clone()
        {
            return (Projectile2)this.MemberwiseClone();
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Platform")
            {
                GameObject.Destroy();
            }
        }
    }
}
