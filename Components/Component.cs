using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SystemShutdown.AStar;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown
{
    // Hovedforfatter: Frederik 
    public abstract class Component
    {
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
        public int Health { get; set; }
     
        public bool IsEnabled { get; set; } = true;

        public GameObject GameObject { get; set; }
 
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
