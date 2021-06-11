using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
    // Lead author: Frederik
    // Contributor: Ras
    // Contributor: Lau
    // Contributor: Søren
    public class Player : Component, IGameListener
    {
        #region Fields
        public delegate void DamageEventHandler(object source, Enemy enemy, EventArgs e);
        public static event DamageEventHandler TakeDamagePlayer;

        private MouseState mouseState;
        private MouseState lastMouseState;

        private static Semaphore MySemaphore;

        private SpriteRenderer spriteRenderer;
        private bool canShoot;
        private bool canToggleMap;
        private bool isLooped;
        private bool hasShot;

        private float shootTime;
        private float cooldown = 2000f;
        private float ShowMapTime;
        private float mapCooldown = 1;
        private int kills = 0;
        private int speed = 250;

        public Vector2 velocity = new Vector2(0f, 0f);
        public Vector2 Distance;

        private bool showingMap = true;
        private bool hasUsedMap;

        private KeyboardState oldState;
        private KeyboardState newState;

        public int dmg { get; set; }
        public int Speed { get { return speed; } set { speed = value; } }
        public int Kills { get { return kills; } set { kills = value; } }
        public float Cooldown { get { return cooldown; } set { cooldown = value; } }
        public bool ShowingMap { get { return showingMap; } private set {; } }
        public bool HasUsedMap { get { return hasUsedMap; } private set {; } }
        #endregion

        #region Constructor
        public Player()
        {
            canShoot = true;
            canToggleMap = true;
            isLooped = false;
            hasShot = false;
            GameWorld.Instance.GameState.PlayerBuilder.fps = 8f;
            // Closes old semaphore and creates a new one (New gamestate bug, return to menu and resume)
            if (MySemaphore != null)
            {
                MySemaphore.Close();
                MySemaphore = null;
            }
            //Debug.WriteLine("Players semaphore releases (10)");
            MySemaphore = new Semaphore(0, 10);
            MySemaphore.Release(10);
            Health = 100;
            Speed = 150;
            dmg = 50;
            TakeDamagePlayer += Player_DamagePlayer;

        }
        #endregion

        #region Methods

        /// <summary>
        /// Ras
        /// Player Health minus Enemy damage.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="enemy"></param>
        /// <param name="e"></param>
        private void Player_DamagePlayer(object source, Enemy enemy, EventArgs e)
        {
            Health -= enemy.Dmg;
        }

        /// <summary>
        /// Ras & Frederik
        /// Moves the player. WASD style. 
        /// </summary>
        /// <param name="keyState"></param>
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
            velocity *= Speed * GameWorld.Instance.DeltaTime;
        }

        /// <summary>
        /// Rotates player sprite towards mouse cursor
        /// </summary>
        public void RotatePlayer()
        {
            Distance.X = mouseState.X - GameWorld.Instance.ScreenWidth / 2 + 45;
            Distance.Y = mouseState.Y - GameWorld.Instance.ScreenHeight / 2 + 45;

            spriteRenderer.Rotation = (float)Math.Atan2(Distance.Y, Distance.X);
        }

        public override void Awake()
        {
            GameObject.Tag = "Player";

            GameObject.Transform.Position = new Vector2(2200, 1700);
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");

        }

        /// <summary>
        /// Ras & Frederik
        /// Checks for collisions on each side. 
        /// </summary>
        private void PlayerMovementCollider()
        {
            foreach (GameObject gameObject in GameWorld.Instance.GameState.GameObjects)
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


            if (Speed > 400)
            {
                Speed = 400;
            }
            if (Cooldown < 500)
            {
                Cooldown = 500;
            }
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
            if (mouseState.LeftButton == ButtonState.Pressed && canShoot)
            {
                Shoot();
            }

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.D))
            {
                Move(keyState);
                GameWorld.Instance.GameState.PlayerBuilder.Animate(gameTime);
                PlayerMovementCollider();
                GameObject.Transform.Translate(velocity);
                velocity = Vector2.Zero;
            }

            if (keyState.IsKeyDown(Keys.M))
            {
                ToggleMap();
            }
        }

        public override string ToString()
        {
            return "Player";
        }

        /// <summary>
        /// Ras 
        /// Creates a projektile gameobject with a moving vector in dircting player is facing
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
                GameObject projectileObject = ProjectileFactory.Instance.Create(GameObject.Transform.Position, "default");
                Vector2 movement = new Vector2(GameWorld.Instance.GameState.CursorPosition.X, GameWorld.Instance.GameState.CursorPosition.Y) - projectileObject.Transform.Position;
                if (movement != Vector2.Zero)
                    movement.Normalize();
                Projectile tmpPro = (Projectile)projectileObject.GetComponent("Projectile");
                SpriteRenderer tmpSpriteRenderer = (SpriteRenderer)projectileObject.GetComponent("SpriteRenderer");
                Collider tmpCollider = (Collider)projectileObject.GetComponent("Collider");
                tmpSpriteRenderer.Rotation = spriteRenderer.Rotation;
                tmpPro.Velocity = movement;
                GameWorld.Instance.GameState.AddGameObject(projectileObject);
            }
        }
        /// <summary>
        /// Will toggle the map if the cooldown is over - Søren
        /// </summary>
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
                    hasUsedMap = true;
                }
            }
        }

        /// <summary>
        /// Collision with Enemy:
        /// Enemy attack bool sat to true and thread runs players enter method. Player health stat is reduced and colored red 
        /// Collision with Pickup:
        /// Pickup is destroyed and a sound effect is played
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy")
            {
                Enemy tmpEnemy = (Enemy)component.GameObject.GetComponent("Enemy");

                if (!tmpEnemy.IsTrojan)
                {
                    tmpEnemy.AttackingPlayer = true;
                    GameWorld.Instance.GameState.HealthColorTimerRed = true;
                }
            }
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Pickup")
            {
                GameWorld.Instance.pickedUp.Play();
                component.GameObject.Destroy();
            }
        }

        /// <summary>
        /// Method which enemy threads use. Accessor to shared resource.
        /// semaphore lock tells thread to wait if semaphore is full. 
        /// Tells semaphore to release 1 after resource has been accessed
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enemy"></param>
        public void Enter(Object id, Enemy enemy)
        {
            int tmp = Thread.CurrentThread.ManagedThreadId;
            //Debug.WriteLine($"Enemy {tmp} Waiting to enter (Player)");
            MySemaphore.WaitOne();
            //Debug.WriteLine("Enemy " + tmp + " damages (Player)");
            Random randomNumber = new Random();

            TakeDamagePlayer(null, enemy, EventArgs.Empty);
            Thread.Sleep(100 * randomNumber.Next(0, 15));

            //Debug.WriteLine("Enemy " + tmp + " is leaving (Player)");
            MySemaphore.Release();
        }
        #endregion
    }
}
