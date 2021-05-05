using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static RenderTarget2D renderTarget;
        public float scale = 0.4444f;

        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

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
            /// <summary>
            /// Game now runs at 144 fps instead of 60 (for smoother gameplay testing)
            /// </summary>
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0f);
            /// <summary>
            /// It will make Update run consistenly at 144 fps, regardless of our Draw
            /// It will try to draw frames to macth 144 fps, but update will allways run at 144
            /// It will render the game at the same framerate as your monitor
            /// This will 'kindof' separate how the game runs (Update), and how the game renders (Draw)
            ///</ summary >
            IsFixedTimeStep = true;

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads all GameStates
            currentGameState = new MenuState(this, Content);
            currentGameState.LoadContent();
            nextGameState = null;

            // Loads Target Renderer: to run the game in the same resolution, no matter the pc
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextGameState != null)
            {
                currentGameState = nextGameState;
                currentGameState.LoadContent();

                nextGameState = null;
            }


            //Updates game
            currentGameState.Update(gameTime);

            currentGameState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            /// <summary>
            /// This will scale and adjust everything in game to our scale and no matter the size of the window,
            /// the game will always be running in 1080p resolution (or what resolution we choose)
            /// </summary>
            scale = 1f / (1080f / graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);

            // Draw game
            currentGameState.Draw(gameTime, spriteBatch);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw TargetRenderer
            spriteBatch.Begin();
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}