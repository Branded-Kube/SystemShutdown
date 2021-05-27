using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
    public class Projectile1 : Component, IGameListener
    {
        //private Player1 player;
        //private SpriteRenderer spriteRenderer;
        public SpriteRenderer tmpSpriteRenderer;

        private float startTime;

        private Vector2 direction;

        private float speed;
        //private Vector2 velocity;
        //public float Height { get; set; }
        public Projectile1(float speed, Vector2 velocity)
        {
            this.speed = speed;
            this.velocity = velocity;
            //currentDir = velocity;
        }
        public override void Awake()
        {
            GameObject.Tag = "Laser";
        }

        public override void Update(GameTime gameTime)
        {
            //Move();
            //Destroy1();
            tmpSpriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");

            MouseState mouseState = Mouse.GetState();
            direction = new Vector2(mouseState.X, mouseState.Y) - tmpSpriteRenderer.Origin;
            direction.Normalize();
            GameObject.Transform.Position = tmpSpriteRenderer.Origin + speed * (/*gameTime.TotalGameTime.TotalMilliseconds*/GameWorld.DeltaTime - startTime) * direction;
            
            
        }
        private void Move()
        {
            GameObject.Transform.Translate(velocity * speed * GameWorld.DeltaTime);
        }

        //private void Destroy1()
        //{
        //    if (GameObject.Transform.Position.Y <= -Height)
        //    {
        //        GameObject.Destroy();
        //    }
        //}

        public override void Destroy()
        {
            GameWorld.gameState.Colliders.Remove((Collider)GameObject.GetComponent("Collider"));

        }

        public Projectile1 Clone()
        {
            return (Projectile1)this.MemberwiseClone();
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" /*&& component.GameObject.Tag == "Platform"*/)
            {
                GameObject.Destroy();
            }
        }
    }
}
