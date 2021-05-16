using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.CommandPattern;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
    class Player1 : Component, IGameListener
    {
        private float speed;
        private SpriteRenderer spriteRenderer;
        private bool canShoot;
        private float shootTime;
        private float cooldown = 1f;

        public Player1()
        {
            this.speed = 400;
            canShoot = true;
            InputHandler.Instance.Entity = this;
        }

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }

        public override void Awake()
        {
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2, GameWorld.Instance.GraphicsDevice.Viewport.Height);
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }

        public override void Update(GameTime gameTime)
        {
            shootTime += GameWorld.DeltaTime;

            if (shootTime >= cooldown)
            {
                canShoot = true;
            }
        }

        public override string ToString()
        {
            return "Player";
        }

        public void Shoot()
        {
            if (canShoot)
            {
                canShoot = false;
                shootTime = 0;
                GameObject1 projectileObject = LaserFactory.Instance.Create("Player");
                projectileObject.Transform.Position = GameObject.Transform.Position;
                projectileObject.Transform.Position += new Vector2(-3, -(spriteRenderer.Sprite.Height + 40));
                GameWorld.Instance.AddGameObject(projectileObject);
            }
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision")
            {

            }
        }
    }
}
