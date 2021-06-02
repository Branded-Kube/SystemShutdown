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

        static Semaphore MySemaphore = new Semaphore(0, 5);

        private SpriteRenderer spriteRenderer;
        public Vector2 distance;
        private bool canShoot;
        private float shootTime;
        private float cooldown = 0.1f;

        private bool canToggleMap;
        private float ShowMapTime;
        private float mapCooldown = 1;

        public Vector2 velocity = new Vector2(0f, 0f);

        public Stack<Mods> playersMods = new Stack<Mods>();
        public int dmg { get; set; }
        public int hp { get; set; }
        public int kills = 0;


        public delegate void DamageEventHandler(object source, EventArgs e);
        public static event DamageEventHandler DamagePlayer;

        public bool showingMap;

        protected Texture2D[] sprites, upWalk;
        protected float fps;

        public Rectangle rectangle;
        public Vector2 lastVelocity;


        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Player()
        {
            
            canShoot = true;
            canToggleMap = true;
            InputHandler.Instance.Entity = this;
            fps = 10f;

            Debug.WriteLine("Players semaphore releases (3)");
            MySemaphore.Release(5);
            Health = 100;
            this.speed = 600;
            dmg = 50;
            hp = 10;
        }

        

        //public void Move(Vector2 velocity)
        //{
        //    currentDir = velocity;

        //    if (velocity != Vector2.Zero)
        //    {
        //        velocity.Normalize();
        //    }
        //    velocity *= speed;
        //    GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
        //}

        public void Move()
        {
            KeyboardState keyState = Keyboard.GetState();


            if (keyState.IsKeyDown(Keys.A))
            {
                velocity.X = -1;
            }

            else if (keyState.IsKeyDown(Keys.D))
            {
                velocity.X = 1;

            }

            if (keyState.IsKeyDown(Keys.W))
            {
                velocity.Y = -1;
            }

            else if (keyState.IsKeyDown(Keys.S))
            {
                velocity.Y = 1;
            }

           
        }

        /// <summary>
        /// Rotates player sprite towards mouse cursor
        /// </summary>
        public void RotatePlayer()
        {
            distance.X = mouseState.X - GameWorld.ScreenWidth / 2 + 45;
            distance.Y = mouseState.Y - GameWorld.ScreenHeight / 2 + 45;

            spriteRenderer.Rotation = (float)Math.Atan2(distance.Y, distance.X);
        }

        public override void Awake()
        {
            GameObject.Tag = "Player";

            GameObject.Transform.Position = new Vector2(2200, 1700);

            //GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
            //this.position = GameObject.Transform.Position;
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");

        }


        public override void Update(GameTime gameTime)
        {

            shootTime += GameWorld.DeltaTime;
            ShowMapTime += GameWorld.DeltaTime;
            lastVelocity = GameObject.Transform.Position;

            if (shootTime >= Cooldown / 1000)
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
            if (/*lastMouseState.LeftButton == ButtonState.Released &&*/ mouseState.LeftButton == ButtonState.Pressed && canShoot)
            {
                Shoot();
            }

            Move();
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= Speed * GameWorld.DeltaTime;

            
                foreach (GameObject1 gameObject in GameWorld.gameState.gameObjects)
                {
                    if (gameObject.Tag == "Node")
                    {

                    Collider nodeCollider = (Collider)gameObject.GetComponent("Collider");

                    Collider playerCollider = (Collider)GameObject.GetComponent("Collider");

                    if ((velocity.X > 0 && playerCollider.IsTouchingLeft(nodeCollider)) ||
                         (velocity.X < 0 && playerCollider.IsTouchingRight(nodeCollider)))
                    {
                        velocity.X = 0;
                    }


                    if ((velocity.Y > 0 && playerCollider.IsTouchingTop(nodeCollider)) ||
                         (velocity.Y < 0 && playerCollider.IsTouchingBottom(nodeCollider)))
                    {
                        velocity.Y = 0;

                    }

                }



            }



            // /*collision.GameObject.Transform.Position*/ GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position += velocity;

            // velocity *= speed* GameWorld.DeltaTime;
            GameObject.Transform.Translate(velocity );
           // GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position += velocity ;

            velocity = Vector2.Zero;


            
        }
        public void ApplyAllMods()
        {

            this.speed = 600;
            dmg = 50;
            hp = 10;
            foreach (Mods mods in playersMods)
            {
                if (mods.ModFKID == 1)
                {
                    dmg += mods.Effect;

                }
                //if (mods.ModFKID == 2)
                //{
                //    dmg += mods.Effect;

                //}
                //if (mods.ModFKID == 3)
                //{
                //    dmg += mods.Effect;

                //}

            }
        }
        public override void Start()
        {
            
        }

        public override string ToString()
        {
            return "Player";
        }

        /// <summary>
        /// Ras 
        /// </summary>
        public void Shoot()
        {
            if (canShoot)
            {
                canShoot = false;
                shootTime = 0;
                GameObject1 laserObject = ProjectileFactory.Instance.Create(GameObject.Transform.Position, "default");

                Vector2 movement = new Vector2(GameWorld.gameState.cursorPosition.X, GameWorld.gameState.cursorPosition.Y) - laserObject.Transform.Position;
                if (movement != Vector2.Zero)
                    movement.Normalize();
                Projectile tmpPro = (Projectile)laserObject.GetComponent("Projectile");
                SpriteRenderer tmpSpriteRenderer = (SpriteRenderer)laserObject.GetComponent("SpriteRenderer");
                Collider tmpCollider = (Collider)laserObject.GetComponent("Collider");

                tmpSpriteRenderer.Rotation = spriteRenderer.Rotation;

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
            //if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Node")
            //{
            //  GameObject.Transform.Position = lastVelocity;
            //}
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Pickup")
            {
                Mods tmpmod = (Mods)component.GameObject.GetComponent("Pickup");
                if (tmpmod.ModFKID == 4)
                {
                    Health += tmpmod.Effect;
                }
                else
                {
                    playersMods.Push(tmpmod);
                }
                ApplyAllMods();
                component.GameObject.Destroy();
            }
        }

        public void Enter(Object id)
        {
            int tmp = Thread.CurrentThread.ManagedThreadId;
            
            Debug.WriteLine($"Enemy {tmp} Waiting to enter (CPU)");
            MySemaphore.WaitOne();
            Debug.WriteLine("Enemy " + tmp + " Starts harvesting power (CPU)");
            Random randomNumber = new Random();

            DamagePlayer(null, EventArgs.Empty);
            Thread.Sleep(100 * randomNumber.Next(0, 15));

            Debug.WriteLine("Enemy " + tmp + " is leaving (CPU)");
            MySemaphore.Release();

        }
    }
}
