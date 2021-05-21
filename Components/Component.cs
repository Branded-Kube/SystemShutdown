using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.Components
{
    public abstract class Component
    {
        public Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

        public Vector2 position;
        //public Rectangle rectangle;
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
        protected Vector2 velocity;
        
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

        public bool IsEnabled { get; set; } = true;

        public GameObject1 GameObject { get; set; }

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Destroy()
        {
        }

        //protected void Animate(GameTime gametime)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
        //    {
        //        //Giver tiden, der er gået, siden sidste update
        //        timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

        //        //Beregner currentIndex
        //        currentIndex = (int)(timeElapsed * fps);
        //        sprite = upWalk[currentIndex];

        //        //Checks if animation needs to restart
        //        if (currentIndex >= upWalk.Length - 1)
        //        {
        //            //Resets animation
        //            timeElapsed = 0;
        //            currentIndex = 0;
        //        }
        //    }
        //}
    }
}
