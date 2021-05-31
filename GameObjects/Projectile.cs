using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.FactoryPattern
{
    class Projectile : Component, IGameListener
    {
        private float speed;
        public Vector2 velocity;
        bool alreadyCollider = false;
        public Projectile(float speed)
        {
            this.speed = speed;
        }
        public override string ToString()
        {
            return "Projectile";
        }

        public override void Awake()
        {
            GameObject.Tag = "Projectile";
            alreadyCollider = false;
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }
        private void Move()
        {
            GameObject.Transform.Translate(velocity * speed *  GameWorld.DeltaTime);
        }

        //public override void Destroy()
        //{
        //    GameWorld.gameState.Colliders.Remove((Collider)GameObject.GetComponent("Collider"));

        //}
        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Node")
            {
                GameObject.Destroy();
            }
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy" && !alreadyCollider)
            {
                GameObject.Destroy();
                component.GameObject.GetComponent("Enemy").Health -= GameWorld.gameState.playerBuilder.player.dmg;
                alreadyCollider = true;
            }
        }
    }
}
