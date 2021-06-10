using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SystemShutdown.Components
{
    // Lead author: Frederik
    public abstract class StateComponent
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}
