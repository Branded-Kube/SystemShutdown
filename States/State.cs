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
    public abstract class State
    {
        #region Fields

        protected ContentManager content = GameWorld.Instance.Content;

        #endregion

        #region Methods

        #region Constructor
        public State()
        {
           
        }
        #endregion

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);


        #endregion
    }
}