using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown
{
    // Lead author: Frederik 
    public abstract class Component
    {
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
