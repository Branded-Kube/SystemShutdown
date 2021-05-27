using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace DesignPaterns.Components
{
    class Projectile : Component, IGameListener
    {
        private float speed;
        private Vector2 velocity;
        public float Height { get; set; }
        public Projectile(float speed, Vector2 velocity)
        {
            this.speed = speed;
            this.velocity = velocity;
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

        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
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
