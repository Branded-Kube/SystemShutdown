using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Components;

namespace SystemShutdown.GameObjects
{
    public class GameObject : ButtonComponent, ICloneable
    {
        #region Fields & Properties
        //protected Texture2D _texture;
        public Transform Transform { get; private set; }
        private Dictionary<string, Component> components = new Dictionary<string, Component>();
        public string Tag { get; set; }

        public Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

        public Vector2 origin;
        public Vector2 currentDir;

        protected float rotation;
        protected Vector2 offset;
        public Vector2 position;
        public Rectangle rectangle;

        protected float timePassed;

        protected Vector2 velocity;
        protected float friction = 0.1f;

        public int Health { get; set; }
        public Color Colour { get; set; }

        protected float _Layer { get; set; }

        // Frederik
        public float Layer
        {
            get { return _Layer; }
            set
            {
                _Layer = value;
            }
        }
        #endregion

        #region Methods

        #region Constructor

        // Frederik
        public GameObject()
        {
            //_texture = texture;

            // Default origin in the centre of the sprite
            //origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            Transform = new Transform();
            Colour = Color.White;
        }
        #endregion

        public void AddComponent(Component component)
        {
            components.Add(component.ToString(), component);
            component.GameObject = this;
        }

        public Component GetComponent(string component)
        {
            return components[component];
        }

        public void Awake()
        {
            foreach (Component component in components.Values)
            {
                component.Awake();
            }
        }

        public void Start()
        {
            foreach (Component component in components.Values)
            {
                component.Start();
            }
        }

        public override void Update(GameTime gameTime)
        {
            //origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            foreach (Component component in components.Values)
            {
                if (component.IsEnabled)
                {
                    component.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Draw(sprite, position, null, Colour, rotation, origin, 1f, SpriteEffects.None, 0);
            foreach (Component component in components.Values)
            {
                if (component.IsEnabled)
                {
                    component.Draw(spriteBatch);
                }
            }
        }

        public void Destroy()
        {
            foreach (Component component in components.Values)
            {
                component.Destroy();
            }
            GameWorld.Instance.RemoveGameObject(this);
        }

        protected void Animate(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //Giver tiden, der er gået, siden sidste update
                timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                //Beregner currentIndex
                currentIndex = (int)(timeElapsed * fps);
                sprite = upWalk[currentIndex];

                //Checks if animation needs to restart
                if (currentIndex >= upWalk.Length - 1)
                {
                    //Resets animation
                    timeElapsed = 0;
                    currentIndex = 0;
                }
            }
            //else if (Keyboard.GetState().IsKeyDown(Keys.S))
            //{
            //    //Giver tiden, der er gået, siden sidste update
            //    timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            //    //Beregner currentIndex
            //    currentIndex = (int)(timeElapsed * fps);
            //    sprite = downWalk[currentIndex];

            //    //Checks if animation needs to restart
            //    if (currentIndex >= downWalk.Length - 1)
            //    {
            //        //Resets animation
            //        timeElapsed = 0;
            //        currentIndex = 0;
            //    }
            //}
            //else if (Keyboard.GetState().IsKeyDown(Keys.D))
            //{
            //    //Giver tiden, der er gået, siden sidste update
            //    timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            //    //Beregner currentIndex
            //    currentIndex = (int)(timeElapsed * fps);
            //    sprite = rightWalk[currentIndex];

            //    //Checks if animation needs to restart
            //    if (currentIndex >= rightWalk.Length - 1)
            //    {
            //        //Resets animation
            //        timeElapsed = 0;
            //        currentIndex = 0;
            //    }
            //}
            //else if (Keyboard.GetState().IsKeyDown(Keys.A))
            //{
            //    //Giver tiden, der er gået, siden sidste update
            //    timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            //    //Beregner currentIndex
            //    currentIndex = (int)(timeElapsed * fps);
            //    sprite = leftWalk[currentIndex];

            //    //Checks if animation needs to restart
            //    if (currentIndex >= leftWalk.Length - 1)
            //    {
            //        //Resets animation
            //        timeElapsed = 0;
            //        currentIndex = 0;
            //    }
            //}
        }

        public virtual void OnCollision(GameObject gameObject)
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
