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
using SystemShutdown.Database;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class GameWorld : Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Fields
        //private static GameWorld instance;

        //public static GameWorld Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new GameWorld();
        //        }
        //        return instance;
        //    }
        //}

        public static ContentManager content;


        public static RenderTarget2D renderTarget;
        public static RenderTarget2D minimap;
        public float scale = 0.4444f;

        public float miniMapScale;

        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        //private List<GameObject1> gameObjects = new List<GameObject1>();
        //private Player1 player;
       // public List<Collider> Colliders { get; set; } = new List<Collider>();

        private State currentGameState;
        private static State nextGameState;
        public static GameState gameState;



        //public PlayerBuilder playerBuilder;

        //private Director director;

        //public Director Director
        //{
        //    get { return director; }
        //    set { director = value; }
        //}

        private Camera camera;

        private bool isGameState;
        private Repository repo;

        public static float DeltaTime { get; set; }
        Random rnd = new Random();

        #endregion

        #region Methods

        #region Constructor
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;

            var mapper = new Mapper();
            var provider = new SQLiteDatabaseProvider("Data Source=SystemShutdown.db;Version=3;new=true");

            repo = new Repository(provider, mapper);

            repo.Open();

            repo.AddMods("Dmg", rnd.Next(1, 5));
            repo.AddMods("Movespeed", 10);
            repo.AddMods("Attackspeed", 10);
            repo.AddMods("Health", 10);

            repo.Close();
        }
        #endregion

        /// <summary>
        /// ChangeState changes GameState
        /// </summary>
        /// <param name="state"></param>
        /// Frederik
        public static void ChangeState(State state)
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
            //playerBuilder = new PlayerBuilder();
            //director = new Director(playerBuilder);
            //gameObjects.Add(director.Contruct());

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Awake();
            //}
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads all GameStates
            //Frederik
            gameState = new GameState();
            currentGameState = new MenuState();

            currentGameState.LoadContent();
            nextGameState = null;
            // Loads Target Renderer: to run the game in the same resolution, no matter the pc
            // Frederik
            renderTarget = new RenderTarget2D(GraphicsDevice, 3500, 3500);

            minimap = renderTarget;

            camera = new Camera();

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Start();
            //}
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameState.ShutdownThreads();
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
                camera.Follow(gameState.playerBuilder);  

            }

            else
            {
                isGameState = false;
            }

            //InputHandler.Instance.Execute();

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Update(gameTime);
            //}

            //Collider[] tmpColliders = Colliders.ToArray();
            //for (int i = 0; i < tmpColliders.Length; i++)
            //{
            //    for (int j = 0; j < tmpColliders.Length; j++)
            //    {
            //        tmpColliders[i].OnCollisionEnter(tmpColliders[j]);
            //    }
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //spriteBatch.Begin();
            /// <summary>
            /// This will scale and adjust everything in game to our scale and no matter the size of the window,
            /// the game will always be running in 1080p resolution (or what resolution we choose)
            /// Frederik
            /// </summary>
            scale = 1f / (1080f / graphics.GraphicsDevice.Viewport.Height);
            miniMapScale = 0.1f / (1080f / graphics.GraphicsDevice.Viewport.Height);
            

            GraphicsDevice.SetRenderTarget(renderTarget);

            // Draw game
            currentGameState.Draw(gameTime, spriteBatch);

            GraphicsDevice.SetRenderTarget(null);

            // Draw TargetRenderer

            if (isGameState)
            {
                spriteBatch.Begin(transformMatrix: camera.Transform);

            }
            else
            {
              spriteBatch.Begin();
            }
            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Draw(spriteBatch);
            //}
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (isGameState)
            {
                spriteBatch.Draw(minimap, new Vector2(-camera.Transform.Translation.X, -camera.Transform.Translation.Y), null, Color.White, 0f, Vector2.Zero, miniMapScale, SpriteEffects.None, 0f);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void CollectMod()
        {

        }

        //public void AddGameObject(GameObject1 go)
        //{
        //    go.Awake();
        //    go.Start();
        //    gameObjects.Add(go);
        //    Collider c = (Collider)go.GetComponent("Collider");
        //    if (c != null)
        //    {
        //        Colliders.Add(c);
        //    }
        //}
        //public void RemoveGameObject(GameObject1 go)
        //{
        //    gameObjects.Remove(go);
        //}
        #endregion
    }
}