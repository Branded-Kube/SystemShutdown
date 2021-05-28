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
    public class Player : Component, IGameListener
    {
        public MouseState mouseState;
        public MouseState lastMouseState;

        static Semaphore MySemaphore = new Semaphore(0, 3);

        private float speed;
        private SpriteRenderer spriteRenderer;
        private PlayerBuilder playerBuilder;
        public Vector2 distance;
        private bool canShoot;
        private float shootTime;
        private float cooldown = 1;

        private bool canToggleMap;
        private float ShowMapTime;
        private float mapCooldown = 1;


        public int dmg { get; set; }
        public int hp { get; set; }



        // public int health = 10;
        // public int dmg = 2;

        public bool showingMap;

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
        private ProjectileFactory laserFactory;

        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Player()
        {
            this.speed = 400;
            canShoot = true;
            canToggleMap = true;
            InputHandler.Instance.Entity = this;
            fps = 10f;

            Debug.WriteLine("Players semaphore releases (3)");
            MySemaphore.Release();
            dmg = 50;
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
        }

        public void RotatePlayer()
        {

            distance.X = mouseState.X - GameWorld.ScreenWidth / 2 + 45;
            distance.Y = mouseState.Y - GameWorld.ScreenHeight / 2 + 45;

            spriteRenderer.Rotation = (float)Math.Atan2(distance.Y, distance.X);
        }

        public override void Awake()
        {
            GameObject.Tag = "Player";

            GameObject.Transform.Position = new Vector2(1000, 1000);

            //GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
            //this.position = GameObject.Transform.Position;
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");

        }


        public override void Update(GameTime gameTime)
        {
            shootTime += GameWorld.DeltaTime;
            ShowMapTime += GameWorld.DeltaTime;
            lastVelocity = GameObject.Transform.Position;

            if (shootTime >= cooldown)
            {
                canShoot = true;
            }

            if (ShowMapTime >= mapCooldown)
            {
                canToggleMap = true;
            }

            // The active state from the last frame is now old
            lastMouseState = mouseState;

            // Get the mouse state relevant for this frame
            mouseState = Mouse.GetState();
            // Recognize a single click of the left mouse button
            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                Shoot();
            }


        }

        public override void Start()
        {
            
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
                GameObject1 laserObject = ProjectileFactory.Instance.Create("Player");

                laserObject.Transform.Position = GameObject.Transform.Position;
                Vector2 movement = new Vector2(GameWorld.gameState.cursorPosition.X, GameWorld.gameState.cursorPosition.Y) - laserObject.Transform.Position;
                if (movement != Vector2.Zero)
                    movement.Normalize();
                Projectile tmpPro = (Projectile)laserObject.GetComponent("Projectile");
                SpriteRenderer tmpSpriteRenderer = (SpriteRenderer)laserObject.GetComponent("SpriteRenderer");
                Collider tmpCollider = (Collider)laserObject.GetComponent("Collider");

               tmpSpriteRenderer.Rotation = spriteRenderer.Rotation +1.6f;

                tmpPro.velocity = movement;
                
                GameWorld.gameState.AddGameObject(laserObject);
            }
        }

        public void ToggleMap()
        {
            if (canToggleMap)
            {
                canToggleMap = false;
                ShowMapTime = 0;

                if (showingMap == false)
                {
                    showingMap = true;
                }

                else
                    showingMap = false;

            }
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
