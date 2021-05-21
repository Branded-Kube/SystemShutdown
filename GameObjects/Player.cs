using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace SystemShutdown.GameObjects
{
    public class Player : MenuObject
    {

        static Semaphore MySemaphore = new Semaphore(0, 3);


        #region Fields
        private KeyboardState currentKey;

        //private KeyboardState previousKey;

        //private float speed;
        //private float laserSpeed;
        //public Vector2 previousPosition;

        private float currentDirY;
        private float currentDirX;
        private Vector2 currentDir;
        private string debug1 = "";
        private string debug2 = "";



        #endregion

        //#region Properties
        //public bool IsDead
        //{
        //    get
        //    {
        //        return Health <= 0;
        //    }
        //}

        //public Input Input { get; set; }
        //#endregion

        //#region Methods

        #region Constructor
        public Player()
        {
            this.speed = 600;
            fps = 10f;

            Debug.WriteLine("Players semaphore releases (3)");
            MySemaphore.Release();

        }
        #endregion



        //tells worker to wait for empty space using semaphore, and then start harvesting from the palmtree
        public void Enter(Object id)
        {
            int tmp = Thread.CurrentThread.ManagedThreadId;

            Debug.WriteLine($"Enemy {tmp} Waiting to enter (CPU)");
            MySemaphore.WaitOne();
            Debug.WriteLine("Enemy " + tmp + " Starts harvesting power (CPU)");
            Random randomNumber = new Random();
            Thread.Sleep(100 * randomNumber.Next(0, 150));
            Debug.WriteLine("Enemy " + tmp + " is leaving (CPU)");
            MySemaphore.Release();

        }


        public override void Update(GameTime gameTime)
        {
            if (IsDead)
            {
                return;
            }



            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

            Animate(gametime: gameTime);

            ////Right
            //if (currentDir.X == 1)
            //{
            //    rotation += 90f;
            //}
            ////Left
            //if (currentDir.X == -1)
            //{
            //    rotation += 90f;
            //}
            ////Down
            //if (currentDir.Y == 1)
            //{

        //}
        //#endregion

        //public override void Update(GameTime gameTime)
        //{
        //    if (IsDead)
        //    {
        //        return;
        //    }



        //    origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);

        //    Animate(gametime: gameTime);

        //    ////Right
        //    //if (currentDir.X == 1)
        //    //{
        //    //    rotation += 90f;
        //    //}
        //    ////Left
        //    //if (currentDir.X == -1)
        //    //{
        //    //    rotation += 90f;
        //    //}
        //    ////Down
        //    //if (currentDir.Y == 1)
        //    //{

        //    //}
        //    ////Up
        //    //if (currentDir.Y == -1)
        //    //{

        //    //}
        //    //Move(gameTime);
        //}

        //public void LoadContent(ContentManager content)
        //{
        //    sprite = content.Load<Texture2D>("1GuyUp");
        //    rectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(sprite.Width - 10, sprite.Height - 10));

        //    //Load sprite sheet
        //    upWalk = new Texture2D[3];

            spriteBatch.DrawString(GameWorld.gameState.font, debug1, new Vector2(300,500), Color.White);
            spriteBatch.DrawString(GameWorld.gameState.font, debug2, new Vector2(300, 300), Color.White);


            base.Draw(gameTime, spriteBatch);
        }

        public void Move(Vector2 velocity)
        {
            //currentDirX = 0;
            //currentDirY = 0;
            currentDir += velocity;

            if (velocity.Y == 0)
            {
                //currentDirY = velocity.Y;
                //currentDirX = 0;

                currentDir.X = velocity.X;
            }
            if (velocity.X == 0)
            {
                //currentDirX = velocity.X;
                //currentDirY = 0;
                currentDir.Y = velocity.Y;

            }
            //currentDir = velocity;



            if (/*Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.Y == -1 && currentDir.X == 1)
            {
                rotation = (float)Math.PI / 4;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.Y == -1 && currentDir.X == -1)
            {
                rotation = (float)Math.PI / 4 * 7;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.Y == 1 && currentDir.X == 1)
            {
                rotation = (float)Math.PI * 3 / 4;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.Y == 1 && currentDir.X == -1)
            {
                rotation = (float)Math.PI / 4 * 5;
            }

            else if (/*Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.X == 1)
            {
                rotation = (float)Math.PI / 2;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.X == -1)
            {
                rotation = (float)Math.PI * 3 / 2;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.S)*/currentDir.Y == 1)
            {
                rotation = (float)Math.PI;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.W)*/currentDir.Y == -1)
            {
                rotation = (float)Math.PI * 2;
            }

            debug1 = $"{velocity}";
            debug2 = $"{currentDir}";


            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
              // currentDir.Normalize();
            }
            velocity *= speed;

            position += (velocity * GameWorld.DeltaTime);

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

        }

        //    ////Load sprite sheet
        //    //leftWalk = new Texture2D[3];

        //    ////Loop animaiton
        //    //for (int j = 0; j < leftWalk.Length; j++)
        //    //{
        //    //    leftWalk[j] = content.Load<Texture2D>(j + 1 + "GuyLeft");
        //    //}
        //    ////When loop is finished return to first sprite/Sets default sprite
        //    //sprite = leftWalk[0];

        //}
        //public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    // Frederik
        //    if (IsDead)
        //    {
        //        return;
        //    }
        //    //spriteBatch.Draw(sprite, rectangle, Colour);

        //    base.Draw(gameTime, spriteBatch);
        //}

        //public void Move(Vector2 velocity)
        //{
        //    //currentDirX = 0;
        //    //currentDirY = 0;
        //    if (velocity.X == 0 && velocity.Y != 0)
        //    {
        //        currentDir.Y = velocity.Y;
        //        //currentDirY = velocity.Y;
        //        //currentDirX = 0;
        //    }
        //    if (velocity.Y == 0 && velocity.X != 0)
        //    {
        //        currentDir.X = velocity.X;
        //        //currentDirX = velocity.X;
        //        //currentDirY = 0;
        //    }
        //    //currentDir = velocity;

        //    if (velocity != Vector2.Zero)
        //    {
        //        velocity.Normalize();
        //    }
        //    velocity *= speed;
        //    position += (velocity * GameWorld.Instance.DeltaTime);
        //    rectangle.X = (int)position.X;
        //    rectangle.Y = (int)position.Y;
        //    if (/*Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.Y == -1 && currentDir.X == 1)
        //    {
        //        rotation = (float)Math.PI / 4;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.Y == -1 && currentDir.X == -1)
        //    {
        //        rotation = (float)Math.PI / 4 * 7;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.Y == 1 && currentDir.X == 1)
        //    {
        //        rotation = (float)Math.PI * 3 / 4;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.Y == 1 && currentDir.X == -1)
        //    {
        //        rotation = (float)Math.PI / 4 * 5;
        //    }

        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.X == 1)
        //    {
        //        rotation = (float)Math.PI / 2;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.X == -1)
        //    {
        //        rotation = (float)Math.PI * 3 / 2;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.S)*/currentDir.Y == 1)
        //    {
        //        rotation = (float)Math.PI;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.W)*/currentDir.Y == -1)
        //    {
        //        rotation = (float)Math.PI * 2;
        //    }

        //}

        //public void Shoot(Vector2 velocity, Vector2 position)
        //{

        //    velocity *= laserSpeed;
        //    position += (velocity * GameWorld.Instance.DeltaTime);
        //    rectangle.X = (int)position.X;
        //    rectangle.Y = (int)position.Y;
        //}


        ////public void Move(GameTime gameTime)
        ////{
        ////    ///<summary>
        ////    /// Movement speed will be consistent no matter the framerate
        ////    /// Frederik
        ////    ///</summary>
        ////    timePassed = gameTime.ElapsedGameTime.Milliseconds;
        ////    float tangentialVelocity = timePassed / 4;

        ////    previousKey = currentKey;
        ////    currentKey = Keyboard.GetState();

        ////    /// <summary>
        ////    /// Rotation rectangle, Player rotation, movement & speed
        ////    /// Frederik
        ////    /// </summary>
        ////    //Player is able to move
        ////    position = velocity + position;

        ////    if (currentKey.IsKeyDown(Input.Right))
        ////    {
        ////        rotation += 0.1f;
        ////    }
        ////    if (currentKey.IsKeyDown(Input.Left))
        ////    {
        ////        rotation -= 0.1f;
        ////    }

        ////    if (currentKey.IsKeyDown(Input.Up))
        ////    {
        ////        velocity.X = (float)Math.Cos(rotation) * tangentialVelocity;
        ////        velocity.Y = (float)Math.Sin(rotation) * tangentialVelocity;
        ////    }
        ////    //Stops movement when key released & adds friction
        ////    else if (velocity != Vector2.Zero)
        ////    {
        ////        float k = velocity.X;
        ////        float l = velocity.Y;

        ////        velocity.X = k -= friction * k;
        ////        velocity.Y = l -= friction * l;
        ////    }

        ////    //var velocity = Vector2.Zero;
        ////    //rotation = 0;

        ////    //if (currentKey.IsKeyDown(Input.Up))
        ////    //{
        ////    //    velocity.Y = -movementSpeed;
        ////    //    velocity.Normalize();
        ////    //}
        ////    //if (currentKey.IsKeyDown(Input.Down))
        ////    //{
        ////    //    velocity.Y += movementSpeed;
        ////    //    rotation = MathHelper.ToRadians(180);
        ////    //    velocity.Normalize();
        ////    //}
        ////    //if (currentKey.IsKeyDown(Input.Left))
        ////    //{
        ////    //    velocity.X -= movementSpeed;
        ////    //    rotation = MathHelper.ToRadians(-90);
        ////    //    velocity.Normalize();
        ////    //}
        ////    //if (currentKey.IsKeyDown(Input.Right))
        ////    //{
        ////    //    velocity.X += movementSpeed;
        ////    //    rotation = MathHelper.ToRadians(90);
        ////    //    velocity.Normalize();
        ////    //}

        ////    //if (currentKey.IsKeyDown(Input.Up) && currentKey.IsKeyDown(Input.Right))
        ////    //{
        ////    //    rotation = MathHelper.ToRadians(45);
        ////    //}
        ////    //if (currentKey.IsKeyDown(Input.Up) && currentKey.IsKeyDown(Input.Left))
        ////    //{
        ////    //    rotation = MathHelper.ToRadians(-45);
        ////    //}
        ////    //if (currentKey.IsKeyDown(Input.Down) && currentKey.IsKeyDown(Input.Right))
        ////    //{
        ////    //    rotation = MathHelper.ToRadians(-225);
        ////    //}
        ////    //if (currentKey.IsKeyDown(Input.Down) && currentKey.IsKeyDown(Input.Left))
        ////    //{
        ////    //    rotation = MathHelper.ToRadians(225);
        ////    //}

        ////    //// Movement
        ////    //position += velocity;
        ////}

        //// Frederik
        //public override void OnCollision(GameObject sprite)
        //{
        //    if (IsDead)
        //    {
        //        return;
        //    }
        //}
        //#endregion
    }
}
