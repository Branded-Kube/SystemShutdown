using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemShutdown
{
    // Lead author: Frederik
    public abstract class State
    {

        protected ContentManager content = GameWorld.Instance.Content;

        #region Fields
        #endregion

        #region Constructor
        #endregion

        #region Methods
        #endregion

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);


        
    }
}