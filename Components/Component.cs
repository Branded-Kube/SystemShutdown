using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.AStar;
using SystemShutdown.ComponentPattern;
//using SystemShutdown.ComponentPattern;

namespace SystemShutdown
{
    public abstract class Component
    {
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
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

        public Node node;

        public Node Node
        {
            get { return node; }
            set { node = value; }
        }

        public bool IsEnabled { get; set; } = true;

        public GameObject1 GameObject { get; set; }
 
        public virtual void Awake()
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
