using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.Components
{
    public abstract class Component
    {
        public Vector2 position;
        public Rectangle rectangle;
        public Vector2 previousPosition;
        public Vector2 currentDir;

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
    }
}
