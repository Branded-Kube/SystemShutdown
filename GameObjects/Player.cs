using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.GameObjects
{
    public class Player : GameObject
    {
        #region Fields
        private KeyboardState currentKey;

        private KeyboardState previousKey;
        private float speed;
        

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
        public Player()
        {
            this.speed = 100;

        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (IsDead)
            {
                return;
            }

           // Move(gameTime);
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Textures/pl1");
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            if (IsDead)
            {
                return;
            }

            base.Draw(gameTime, spriteBatch);
        }


        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            position += (velocity * GameWorld.DeltaTime);
        }


        //public void Move(GameTime gameTime)
        //{
        //    ///<summary>
        //    /// Movement speed will be consistent no matter the framerate
        //    /// Frederik
        //    ///</summary>
        //    timePassed = gameTime.ElapsedGameTime.Milliseconds;
        //    float tangentialVelocity = timePassed / 4;

        //    previousKey = currentKey;
        //    currentKey = Keyboard.GetState();

        //    /// <summary>
        //    /// Rotation rectangle, Player rotation, movement & speed
        //    /// Frederik
        //    /// </summary>
        //    //Player is able to move
        //    position = velocity + position;

        //    if (currentKey.IsKeyDown(Input.Right))
        //    {
        //        rotation += 0.1f;
        //    }
        //    if (currentKey.IsKeyDown(Input.Left))
        //    {
        //        rotation -= 0.1f;
        //    }

        //    if (currentKey.IsKeyDown(Input.Up))
        //    {
        //        velocity.X = (float)Math.Cos(rotation) * tangentialVelocity;
        //        velocity.Y = (float)Math.Sin(rotation) * tangentialVelocity;
        //    }
        //    //Stops movement when key released & adds friction
        //    else if (velocity != Vector2.Zero)
        //    {
        //        float k = velocity.X;
        //        float l = velocity.Y;

        //        velocity.X = k -= friction * k;
        //        velocity.Y = l -= friction * l;
        //    }

        //    //var velocity = Vector2.Zero;
        //    //rotation = 0;

        //    //if (currentKey.IsKeyDown(Input.Up))
        //    //{
        //    //    velocity.Y = -movementSpeed;
        //    //    velocity.Normalize();
        //    //}
        //    //if (currentKey.IsKeyDown(Input.Down))
        //    //{
        //    //    velocity.Y += movementSpeed;
        //    //    rotation = MathHelper.ToRadians(180);
        //    //    velocity.Normalize();
        //    //}
        //    //if (currentKey.IsKeyDown(Input.Left))
        //    //{
        //    //    velocity.X -= movementSpeed;
        //    //    rotation = MathHelper.ToRadians(-90);
        //    //    velocity.Normalize();
        //    //}
        //    //if (currentKey.IsKeyDown(Input.Right))
        //    //{
        //    //    velocity.X += movementSpeed;
        //    //    rotation = MathHelper.ToRadians(90);
        //    //    velocity.Normalize();
        //    //}

        //    //if (currentKey.IsKeyDown(Input.Up) && currentKey.IsKeyDown(Input.Right))
        //    //{
        //    //    rotation = MathHelper.ToRadians(45);
        //    //}
        //    //if (currentKey.IsKeyDown(Input.Up) && currentKey.IsKeyDown(Input.Left))
        //    //{
        //    //    rotation = MathHelper.ToRadians(-45);
        //    //}
        //    //if (currentKey.IsKeyDown(Input.Down) && currentKey.IsKeyDown(Input.Right))
        //    //{
        //    //    rotation = MathHelper.ToRadians(-225);
        //    //}
        //    //if (currentKey.IsKeyDown(Input.Down) && currentKey.IsKeyDown(Input.Left))
        //    //{
        //    //    rotation = MathHelper.ToRadians(225);
        //    //}

        //    //// Movement
        //    //position += velocity;
        //}

        // Frederik
        public override void OnCollision(GameObject sprite)
        {
            if (IsDead)
            {
                return;
            }
        }
        #endregion
    }
}
