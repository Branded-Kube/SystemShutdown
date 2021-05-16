using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemShutdown.BuildPattern;
using SystemShutdown.Buttons;
using SystemShutdown.CommandPattern;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class GameWorld : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Fields
        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        public static ContentManager content;

        public static RenderTarget2D renderTarget;
        public float scale = 0.4444f;

        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        private List<GameObject1> gameObjects = new List<GameObject1>();
        private Player1 player;
        public List<Collider> Colliders { get; set; } = new List<Collider>();

        private State currentGameState;
        private State nextGameState;
        public static GameState gameState;


        private Camera camera;

        private bool isGameState;

        public float DeltaTime { get; set; }

        #endregion

        #region Methods

        #region Constructor
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
        }
        #endregion

        /// <summary>
        /// ChangeState changes GameState
        /// </summary>
        /// <param name="state"></param>
        /// Frederik
        public void ChangeState(State state)
        {
            nextGameState = state;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            //GameObject1 go = new GameObject1();

            //player = new Player1();

            //go.AddComponent(player);
            //go.AddComponent(new SpriteRenderer());

            //gameObjects.Add(go);

            Director director = new Director(new PlayerBuilder());
            gameObjects.Add(director.Contruct());

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads all GameStates
            //Frederik
            gameState = new GameState(this, Content);
            currentGameState = new MenuState(this, Content);

            currentGameState.LoadContent();
            nextGameState = null;
            // Loads Target Renderer: to run the game in the same resolution, no matter the pc
            // Frederik
            renderTarget = new RenderTarget2D(GraphicsDevice, 4096, 4096);
            camera = new Camera();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Frederik
            if (nextGameState != null)
            {
                currentGameState = nextGameState;
                currentGameState.LoadContent();

                nextGameState = null;

            }
            //Updates game
            currentGameState.Update(gameTime);

            currentGameState.PostUpdate(gameTime);
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentGameState is GameState)
            {
                isGameState = true;
                //camera.Follow((GameState)currentGameState);

            }

            else
            {
                isGameState = false;
            }

            InputHandler.Instance.Execute();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }

            Collider[] tmpColliders = Colliders.ToArray();
            for (int i = 0; i < tmpColliders.Length; i++)
            {
                for (int j = 0; j < tmpColliders.Length; j++)
                {
                    tmpColliders[i].OnCollisionEnter(tmpColliders[j]);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            /// <summary>
            /// This will scale and adjust everything in game to our scale and no matter the size of the window,
            /// the game will always be running in 1080p resolution (or what resolution we choose)
            /// Frederik
            /// </summary>
            scale = 1f / (1080f / graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);

            // Draw game
            currentGameState.Draw(gameTime, spriteBatch);

            GraphicsDevice.SetRenderTarget(null);

            // Draw TargetRenderer

            //if (isGameState)
            //{
            //    //spriteBatch.Begin(transformMatrix: camera.Transform);
            //}

            //else
            //{
                spriteBatch.Begin();
            //}
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AddGameObject(GameObject1 go)
        {
            go.Awake();
            go.Start();
            gameObjects.Add(go);
            Collider c = (Collider)go.GetComponent("Collider");
            if (c != null)
            {
                Colliders.Add(c);
            }
        }
        public void RemoveGameObject(GameObject1 go)
        {
            gameObjects.Remove(go);
        }
        #endregion
    }
}