using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        private State currentGameState;
        private State nextGameState;

        public void ChangeState(State state)
        {
            nextGameState = state;
        }

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentGameState = new MenuState(this, Content);
            currentGameState.LoadContent();
            nextGameState = null;
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextGameState != null)
            {
                currentGameState = nextGameState;
                currentGameState.LoadContent();

                nextGameState = null;
            }

            currentGameState.Update(gameTime);

            currentGameState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            currentGameState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}