using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.Sprites
{
    public class Player : PlayerObject
    {
        #region Fields
        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private float shootTimer = 0;
        #endregion

        #region Properties
        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Input Input { get; set; }
        #endregion

        #region Methods

        #region Constructor
        public Player(Texture2D texture)
            : base(texture)
        {
            speed = 3f;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (IsDead)
            {
                return;
            }

            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            var velocity = Vector2.Zero;
            _rotation = 0;

            if (currentKey.IsKeyDown(Input.Up))
            {
                velocity.Y = -speed;
            }
            else if (currentKey.IsKeyDown(Input.Down))
            {
                velocity.Y += speed;
                _rotation = MathHelper.ToRadians(180);
            }

            if (currentKey.IsKeyDown(Input.Left))
            {
                velocity.X -= speed;
                _rotation = MathHelper.ToRadians(-90);
            }
            else if (currentKey.IsKeyDown(Input.Right))
            {
                velocity.X += speed;
                _rotation = MathHelper.ToRadians(90);
            }

            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentKey.IsKeyDown(Input.Shoot) && shootTimer > 0.25f)
            {
                Shoot(speed * 2);
                shootTimer = 0f;
            }

            // Movement
            Position += velocity;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead)
            {
                return;
            }

            base.Draw(gameTime, spriteBatch);
        }

        public override void OnCollision(GameObject sprite)
        {
            if (IsDead)
            {
                return;
            }

            // Damage dealt by Bullets

            //if (sprite is Bullet && ((Bullet)sprite).Parent is Enemy)
            //{
            //    Health--;
            //}

            //if (sprite is Enemy)
            //{
            //    Health -= 3;
            //}
        }
        #endregion
    }
}
