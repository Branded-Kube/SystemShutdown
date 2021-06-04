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
    // Ras & ? 
    class Projectile : Component, IGameListener
    {
        private float speed;
        private Vector2 velocity;
        private bool alreadyCollided = false;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
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
            alreadyCollided = false;
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }
        private void Move()
        {
            GameObject.Transform.Translate(velocity * speed * GameWorld.Instance.DeltaTime);
        }
      
        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
        }

        /// <summary>
        /// Destroys itself on impact with a Node object (wall) or enemy
        /// If enemy, damages enemys health with players dmg. Bool AlreadyCollided causes projektile to only hit 1 enemy at a time. 
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Node")
            {
                GameObject.Destroy();
            }
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy" /*&& !alreadyCollider*/)
            {
                GameObject.Destroy();
                component.GameObject.GetComponent("Enemy").Health -= GameWorld.Instance.GameState.playerBuilder.player.dmg;
                alreadyCollided = true;
            }
        }
    }
}
