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
//using SystemShutdown.CommandPattern;
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

        static Semaphore MySemaphore;

        private SpriteRenderer spriteRenderer;
        public Vector2 distance;
        private bool canShoot;
        private float shootTime;
        public float cooldown = 2000f;

        private bool canToggleMap;
        private float ShowMapTime;
        private float mapCooldown = 1;

        public Vector2 velocity = new Vector2(0f, 0f);

        public Stack<Mods> playersMods = new Stack<Mods>();
        public int dmg { get; set; }
        public int hp { get; set; }
        public int kills = 0;
         public int speed = 250;

        public delegate void DamageEventHandler(object source, Enemy enemy, EventArgs e);
        public static event DamageEventHandler TakeDamagePlayer;

        public bool showingMap;

        //protected Texture2D[] sprites, upWalk;
        //protected float fps;
        //private float timeElapsed;
        //private int currentIndex;

        public Rectangle rectangle;
        public Vector2 lastVelocity;

        private KeyboardState oldState;
        private KeyboardState newState;

        private bool isLooped;
        private bool hasShot;

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
            isLooped = false;
            hasShot = false;
            GameWorld.Instance.GameState.PlayerBuilder.fps = 8f;
            // Closes old semaphore and creates a new one (New gamestate bug, return to menu and resume
            if (MySemaphore != null)
            {
                MySemaphore.Close();
                MySemaphore = null;
            }
            Debug.WriteLine("Players semaphore releases (10)");
            MySemaphore = new Semaphore(0, 10);
            MySemaphore.Release(10);
            Health = 100;
            this.speed = 150;
            dmg = 50;
            TakeDamagePlayer += Player_DamagePlayer;

        }
        private void Player_DamagePlayer(object source, Enemy enemy, EventArgs e)
        {
            Health -= enemy.Dmg;
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

        public void Move(KeyboardState keyState)
        {
            oldState = newState;
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.A))
            {
                velocity.X = -1;

                if (isLooped == true && newState.IsKeyUp(Keys.D) && newState.IsKeyUp(Keys.W) && newState.IsKeyUp(Keys.S))
                {
                    GameWorld.Instance.walkEffect.Play();
                }

                if (oldState.IsKeyDown(Keys.A))
                {
                    isLooped = false;
                }

                if (oldState.IsKeyUp(Keys.A))
                {
                    isLooped = true;
                }
            }

            else if (newState.IsKeyDown(Keys.D))
            {
                velocity.X = 1;

                if (isLooped == true && newState.IsKeyUp(Keys.A) && newState.IsKeyUp(Keys.W) && newState.IsKeyUp(Keys.S))
                {
                    GameWorld.Instance.walkEffect.Play();
                }

                if (oldState.IsKeyDown(Keys.D))
                {
                    isLooped = false;
                }

                if (oldState.IsKeyUp(Keys.D))
                {
                    isLooped = true;
                }
            }

            if (newState.IsKeyDown(Keys.W))
            {
                velocity.Y = -1;

                if (isLooped == true && newState.IsKeyUp(Keys.A) && newState.IsKeyUp(Keys.D) && newState.IsKeyUp(Keys.S))
                {
                    GameWorld.Instance.walkEffect.Play();
                }

                if (oldState.IsKeyDown(Keys.W))
                {
                    isLooped = false;
                }

                if (oldState.IsKeyUp(Keys.W))
                {
                    isLooped = true;
                }
            }

            else if (newState.IsKeyDown(Keys.S))
            {
                velocity.Y = 1;

                if (isLooped == true && newState.IsKeyUp(Keys.A) && newState.IsKeyUp(Keys.D) && newState.IsKeyUp(Keys.W))
                {
                    GameWorld.Instance.walkEffect.Play();
                }

                if (oldState.IsKeyDown(Keys.S))
                {
                    isLooped = false;
                }

                if (oldState.IsKeyUp(Keys.S))
                {
                    isLooped = true;
                }
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed * GameWorld.Instance.DeltaTime;
        }

        /// <summary>
        /// Rotates player sprite towards mouse cursor
        /// </summary>
        public void RotatePlayer()
        {
            distance.X = mouseState.X - GameWorld.Instance.ScreenWidth / 2 + 45;
            distance.Y = mouseState.Y - GameWorld.Instance.ScreenHeight / 2 + 45;

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

        private void PlayerMovementCollider()
        {
            foreach (GameObject1 gameObject in GameWorld.Instance.GameState.GameObjects)
            {
                if (gameObject.Tag == "Node")
                {
                    Collider nodeCollider = (Collider)gameObject.GetComponent("Collider");

                    Collider playerCollider = (Collider)GameObject.GetComponent("Collider");

                    if ((velocity.X > 0 && playerCollider.IsTouchingLeft(nodeCollider)))
                    {
                        velocity.X = 0;
                    }

                    if ((velocity.X < 0 && playerCollider.IsTouchingRight(nodeCollider)))
                    {
                        velocity.X = 0;
                    }

                    if ((velocity.Y > 0 && playerCollider.IsTouchingTop(nodeCollider)))
                    {
                        velocity.Y = 0;

                    }
                    if ((velocity.Y < 0 && playerCollider.IsTouchingBottom(nodeCollider)))
                    {
                        velocity.Y = 0;
                    }
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            RotatePlayer();
            shootTime += GameWorld.Instance.DeltaTime;
            ShowMapTime += GameWorld.Instance.DeltaTime;
            lastVelocity = GameObject.Transform.Position;


            if (speed > 400)
            {
                speed = 400;
            }
            if (cooldown < 500)
            {
                cooldown = 500;
            }
            if (shootTime >= cooldown / 1000)
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

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A)|| keyState.IsKeyDown(Keys.W)|| keyState.IsKeyDown(Keys.S)|| keyState.IsKeyDown(Keys.D))
            {
                Move(keyState);
                
                PlayerMovementCollider();
                GameObject.Transform.Translate(velocity);
                velocity = Vector2.Zero;
            }

            if (keyState.IsKeyDown(Keys.M))
            {
                ToggleMap();
            }
        }
        //public void ApplyAllMods()
        //{

        //    this.speed = 600;
        //    dmg = 50;
        //    hp = 10;
        //    foreach (Mods mods in playersMods)
        //    {
        //        if (mods.ModFKID == 1)
        //        {
        //            dmg += mods.Effect;

        //        }
        //        if (mods.ModFKID == 2)
        //        {
        //            if (speed > 500)
        //            {
        //                speed += mods.Effect;
        //            }
        //        }
        //        if (mods.ModFKID == 3)
        //        {
        //            if (cooldown > 0.1)
        //            {
        //                cooldown -= mods.Effect;
        //            }

        //        }

        //    }
        //}
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
                if (hasShot == false)
                {
                    GameWorld.Instance.laserEffect.Play();
                    hasShot = true;
                }
                else if (hasShot == true)
                {
                    GameWorld.Instance.laserEffect2.Play();
                    hasShot = false;
                }
                canShoot = false;
                shootTime = 0;
                GameObject1 laserObject = ProjectileFactory.Instance.Create(GameObject.Transform.Position, "default");
                Vector2 movement = new Vector2(GameWorld.Instance.GameState.CursorPosition.X, GameWorld.Instance.GameState.CursorPosition.Y) - laserObject.Transform.Position;
                if (movement != Vector2.Zero)
                    movement.Normalize();
                Projectile tmpPro = (Projectile)laserObject.GetComponent("Projectile");
                SpriteRenderer tmpSpriteRenderer = (SpriteRenderer)laserObject.GetComponent("SpriteRenderer");
                Collider tmpCollider = (Collider)laserObject.GetComponent("Collider");
                tmpSpriteRenderer.Rotation = spriteRenderer.Rotation;
                tmpPro.Velocity = movement;
                GameWorld.Instance.GameState.AddGameObject(laserObject);
            }
        }

        public void ToggleMap()
        {
            if (canToggleMap)
            {
                GameWorld.Instance.toggle.Play();
                canToggleMap = false;
                ShowMapTime = 0;

                if (showingMap == false)
                {
                    GameWorld.Instance.toggle2.Play();
                    showingMap = true;
                }
                else
                {
                    showingMap = false;
                }
            }
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy")
            {
                Enemy tmpEnemy = (Enemy)component.GameObject.GetComponent("Enemy");

                if (!tmpEnemy.IsTrojan)
                {
                    tmpEnemy.AttackingPlayer = true;
                }
            }
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Pickup")
            {
                GameWorld.Instance.pickedUp.Play();
                component.GameObject.Destroy();
            }
        }

        public void Enter(Object id, Enemy enemy)
        {
            int tmp = Thread.CurrentThread.ManagedThreadId;
            
            //Debug.WriteLine($"Enemy {tmp} Waiting to enter (CPU)");
            MySemaphore.WaitOne();
            //Debug.WriteLine("Enemy " + tmp + " Starts harvesting power (CPU)");
            Random randomNumber = new Random();

            TakeDamagePlayer(null, enemy, EventArgs.Empty);
            Thread.Sleep(100 * randomNumber.Next(0, 15));

            //Debug.WriteLine("Enemy " + tmp + " is leaving (CPU)");
            MySemaphore.Release();

        }
    }
}
