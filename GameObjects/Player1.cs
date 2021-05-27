using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using SystemShutdown.BuildPattern;
using SystemShutdown.CommandPattern;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
    public class Player1 : Component, IGameListener
    {
        public MouseState mouseState;
        static Semaphore MySemaphore = new Semaphore(0, 3);

        private float speed;
        private SpriteRenderer spriteRenderer;
        private PlayerBuilder playerBuilder;
        public Vector2 distance;
        private bool canShoot;
        private float shootTime;
        private float cooldown = 1f;


        public int dmg { get; set; }
        public int hp { get; set; }



       // public int health = 10;
       // public int dmg = 2;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private float laserSpeed;
        public Vector2 previousPosition;

        private float currentDirY;
        private float currentDirX;

        //public Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

       // public Vector2 position;
        public Rectangle rectangle;
        public Vector2 currentDir;
        protected float rotation;
        //protected Vector2 velocity;
        bool cantMove = false;
        public Vector2 lastVelocity;
        private LaserFactory laserFactory;

        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Player1()
        {
            this.speed = 400;
            canShoot = true;
            InputHandler.Instance.Entity = this;
            fps = 10f;

            Debug.WriteLine("Players semaphore releases (3)");
            MySemaphore.Release();
            dmg = 2;
            hp = 10;
        }

        public void Move(Vector2 velocity)
        {
            currentDir = velocity;

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
           // RotatePlayer();
        }

        public void RotatePlayer()
        {
            mouseState = Mouse.GetState();

            distance.X = mouseState.X - GameWorld.ScreenWidth / 2 + 45;
            distance.Y = mouseState.Y - GameWorld.ScreenHeight / 2 + 45;

            spriteRenderer.Rotation = (float)Math.Atan2(distance.Y, distance.X);
        }

        //private void RotatePlayer(SpriteRenderer rotate)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D)/*currentDir.Y == -1 && currentDir.X == 1*/)
        //    {
        //        rotate.Rotation = (float)Math.PI / 4;
        //    }

        //    else if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A)/*currentDir.Y == -1 && currentDir.X == -1*/)
        //    {
        //        rotate.Rotation = (float)Math.PI / 4 * 7;
        //    }
        //    else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D)/*currentDir.Y == 1 && currentDir.X == 1*/)
        //    {
        //        rotate.Rotation = (float)Math.PI * 3 / 4;
        //    }
        //    else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A)/*currentDir.Y == 1 && currentDir.X == -1*/)
        //    {
        //        rotate.Rotation = (float)Math.PI / 4 * 5;
        //    }

        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.W)*/currentDir.Y == -1)
        //    {
        //        rotate.Rotation = (float)Math.PI * 2;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.S)*/currentDir.Y == 1)
        //    {
        //        rotate.Rotation = (float)Math.PI;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.X == 1)
        //    {
        //        rotate.Rotation = (float)Math.PI / 2;
        //    }
        //    else if (/*Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.X == -1)
        //    {
        //        rotate.Rotation = (float)Math.PI * 3 / 2;
        //    }
        //}

        public override void Awake()
        {
            GameObject.Tag = "Player";

           // GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
            ////this.position = GameObject.Transform.Position;
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }

        public void LoadContent(ContentManager content)
        {
            //    rectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(sprite.Width - 10, sprite.Height - 10));

            //    //Load sprite sheet
            //    upWalk = new Texture2D[3];

            //    //Loop animaiton
            //    for (int g = 0; g < upWalk.Length; g++)
            //    {
            //        upWalk[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "GuyUp");
            //    }
            //    //When loop is finished return to first sprite/Sets default sprite
            //    sprite = upWalk[0];
        }

    public override void Update(GameTime gameTime)
        {
            shootTime += GameWorld.DeltaTime;
            //rectangle = new Rectangle((int)spriteRenderer.Origin.X, (int)spriteRenderer.Origin.Y,  spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
            lastVelocity = GameObject.Transform.Position;

            if (shootTime >= cooldown)
            {
                canShoot = true;
            }

            if (IsDead)
            {
                return;
            }

            //foreach (Bullet bullet in bullets)
            //{
            //    bullet.Update(gameTime);
            //}
            
            Animate(gametime: gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead)
            {
                return;
            }

            //foreach (Bullet bullet in bullets)
            //{
            //    bullet.Draw(spriteBatch);
            //}
        }

        public override void Start()
        {
            //SpriteRenderer sr = (SpriteRenderer)GameObject1.GetComponent("SpriteRenderer");
            //sr.SetSprite("1GuyUp");
            //sr.Origin = new Vector2(sr.Sprite.Width / 2, (sr.Sprite.Height / 2) + 35);
            //rectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(sprite.Width - 10, sprite.Height - 10));
            //rectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(GameWorld.Instance.playerBuilder.player.sprite.Width - 10, GameWorld.Instance.playerBuilder.player.sprite.Height - 10));


            ////Load sprite sheet
            //upWalk = new Texture2D[3];

            ////Loop animaiton
            //for (int g = 0; g < upWalk.Length; g++)
            //{
            //    upWalk[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "GuyUp");
            //}
            ////When loop is finished return to first sprite/Sets default sprite
            //GameWorld.Instance.playerBuilder.player.sprite = upWalk[0];
            
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
                //projectileObject.Transform.Position += new Vector2(-5, -(spriteRenderer.Sprite.Height));
                //if (currentDir.Y == -1)
                //{
                    projectileObject.Transform.Position += new Vector2(-5, -spriteRenderer.Sprite.Height);
                //}
                //else if (currentDir.Y == 1)
                //{
                //    projectileObject.Transform.Position -= new Vector2(5, -spriteRenderer.Sprite.Height);
                //}
                GameWorld.gameState.AddGameObject(projectileObject);

                velocity *= laserSpeed;
                //position += (velocity * GameWorld.DeltaTime);
                //rectangle.X = (int)position.X;
                //rectangle.Y = (int)position.Y;
            }
        }

        public void Shoot2()
        {
            //MouseState mouseState = Mouse.GetState();

            //if (mouseState.LeftButton == ButtonState.Pressed)
            //{
            //    bullets.Add(new Bullet(bullet, GameObject.Transform.Position, spriteRenderer.Rotation));
            //}
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Node")
            {
              GameObject.Transform.Position = lastVelocity;
            }
        }

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

        protected void Animate(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //Giver tiden, der er gået, siden sidste update
                timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                //Beregner currentIndex
                currentIndex = (int)(timeElapsed * fps);
                spriteRenderer.Sprite = upWalk[currentIndex];

                //Checks if animation needs to restart
                if (currentIndex >= upWalk.Length - 1)
                {
                    //Resets animation
                    timeElapsed = 0;
                    currentIndex = 0;
                }
            }
        }
    }
}
