using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
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
        private float speed;
        private SpriteRenderer sr;
        private PlayerBuilder playerBuilder;
        private bool canShoot;
        private float shootTime;
        private float cooldown = 1f;

        private KeyboardState currentKey;

        private KeyboardState previousKey;

        private float laserSpeed;

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
        }

        public void Move(Vector2 velocity)
        {
            //currentDirX = 0;
            //currentDirY = 0;
            //if (velocity.X == 0 && velocity.Y != 0)
            //{
            //    currentDir.Y = velocity.Y;
            //    //currentDirY = velocity.Y;
            //    //currentDirX = 0;
            //}
            //if (velocity.Y == 0 && velocity.X != 0)
            //{
            //    currentDir.X = velocity.X;
            //    //currentDirX = velocity.X;
            //    //currentDirY = 0;
            //}
            currentDir = velocity;

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
            rectangle.X = (int)GameObject.Transform.Position.X;
            rectangle.Y = (int)GameObject.Transform.Position.Y;
            //rectangle.X = (int)position.X;
            //rectangle.Y = (int)position.Y;
            RotatePlayer(sr);
        }

        private void RotatePlayer(SpriteRenderer rotate)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D)/*currentDir.Y == -1 && currentDir.X == 1*/)
            {
                rotate.Rotation = (float)Math.PI / 4;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A)/*currentDir.Y == -1 && currentDir.X == -1*/)
            {
                rotate.Rotation = (float)Math.PI / 4 * 7;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D)/*currentDir.Y == 1 && currentDir.X == 1*/)
            {
                rotate.Rotation = (float)Math.PI * 3 / 4;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A)/*currentDir.Y == 1 && currentDir.X == -1*/)
            {
                rotate.Rotation = (float)Math.PI / 4 * 5;
            }

            else if (/*Keyboard.GetState().IsKeyDown(Keys.W)*/currentDir.Y == -1)
            {
                rotate.Rotation = (float)Math.PI * 2;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.S)*/currentDir.Y == 1)
            {
                rotate.Rotation = (float)Math.PI;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.D)*/currentDir.X == 1)
            {
                rotate.Rotation = (float)Math.PI / 2;
            }
            else if (/*Keyboard.GetState().IsKeyDown(Keys.A)*/currentDir.X == -1)
            {
                rotate.Rotation = (float)Math.PI * 3 / 2;
            }
        }

        public override void Awake()
        {
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width / 2, GameWorld.Instance.GraphicsDevice.Viewport.Height);
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }

        public void LoadContent(ContentManager content)
        {
            rectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(sprite.Width - 10, sprite.Height - 10));

            ////Load sprite sheet
            //upWalk = new Texture2D[3];

            ////Loop animaiton
            //for (int g = 0; g < upWalk.Length; g++)
            //{
            //    upWalk[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "GuyUp");
            //}
            ////When loop is finished return to first sprite/Sets default sprite
            //sr.Sprite = upWalk[0];

            //Load sprite sheet
            sr.sprites = new Texture2D[3];

            //Loop animaiton
            for (int g = 0; g < sr.sprites.Length; g++)
            {
                sr.sprites[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "GuyUp");
            }
            //When loop is finished return to first sprite/Sets default sprite
            sr.Sprite = sr.sprites[0];
        }

        public override void Update(GameTime gameTime)
        {
            shootTime += GameWorld.Instance.DeltaTime;

            if (shootTime >= cooldown)
            {
                canShoot = true;
            }

            if (IsDead)
            {
                return;
            }

            /*playerBuilder.*/Animate(gametime: gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead)
            {
                return;
            }
        }

        //public override void Start()
        //{
        //    SpriteRenderer sr = (SpriteRenderer)GameObject1.GetComponent("SpriteRenderer");
        //    sr.SetSprite("1GuyUp");
        //    sr.Origin = new Vector2(sr.Sprite.Width / 2, (sr.Sprite.Height / 2) + 35);
        //}

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
                projectileObject.Transform.Position += new Vector2(-3, -(sr.Sprite.Height + 40));
                GameWorld.Instance.AddGameObject(projectileObject);

                velocity *= laserSpeed;
                position += (velocity * GameWorld.Instance.DeltaTime);
                rectangle.X = (int)position.X;
                rectangle.Y = (int)position.Y;
            }
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision")
            {
                if (IsDead)
                {
                    return;
                }
            }
        }

        protected void Animate(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W)/*currentDir.Y == -1*/ || Keyboard.GetState().IsKeyDown(Keys.S)/*currentDir.Y == 1*/ || Keyboard.GetState().IsKeyDown(Keys.D)/*currentDir.X == 1*/ || Keyboard.GetState().IsKeyDown(Keys.A)/*currentDir.X == -1*/)
            {
                //Giver tiden, der er gået, siden sidste update
                timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                ////Beregner currentIndex
                //currentIndex = (int)(timeElapsed * fps);
                //sr.Sprite = upWalk[currentIndex];

                ////Checks if animation needs to restart
                //if (currentIndex >= upWalk.Length - 1)
                //{
                //    //Resets animation
                //    timeElapsed = 0;
                //    currentIndex = 0;
                //}

                //Beregner currentIndex
                currentIndex = (int)(timeElapsed * fps);
                sr.Sprite = sr.sprites[currentIndex];

                //Checks if animation needs to restart
                if (currentIndex >= sr.sprites.Length - 1)
                {
                    //Resets animation
                    timeElapsed = 0;
                    currentIndex = 0;
                }
            }
        }
    }
}
