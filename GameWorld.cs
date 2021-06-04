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
        #region Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static GameWorld instance;

        /// <summary>
        /// Used in MenuState to Exit Game - Frederik
        /// </summary>
       // private GameWorld thisGameWorld;


        private RenderTarget2D minimap;
        private float scale = 0.4444f;
        private float miniMapScale;
        private State currentGameState;
        private static State nextGameState;
        private Camera camera;
        private bool isGameState;
        private CyclebarDay cyclebarDay;
        private CyclebarNight cyclebarNight;
        private Random rnd = new Random();
        private CPU cpu;

        public HowToState HowToState { get; set; }
        public MenuState MenuState { get; set; }
        public HighscoreState HighscoreState { get; set; }
        public bool IsDay { get; set; }
        public int ScreenHeight { get; set; } = 1080;
        public int ScreenWidth { get; set; } = 1920;
        public RenderTarget2D RenderTarget { get; set; }
        public GameOverState GameOverState { get; set; }
        public Repository Repository { get; set; }
        public GameState GameState { get; set; }
        public ContentManager Content { get; set; }
        public float DeltaTime { get; set; }


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
        #endregion

        #region Methods

        #region Constructor
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            base.Content.RootDirectory = "Content";
            Content = base.Content;

            //thisGameWorld = this;

            var mapper = new Mapper();
            var provider = new SQLiteDatabaseProvider("Data Source=SystemShutdown.db;Version=3;new=true");

            Repository = new Repository(provider, mapper);

            Repository.Open();

            Repository.AddMods("Dmg"); //ID = 1
            Repository.AddMods("Movespeed"); //ID = 2
            Repository.AddMods("Attackspeed"); //ID = 3
            Repository.AddMods("Health"); //ID = 4

            Repository.AddEffects(5, "dmg1", 1);
            Repository.AddEffects(10, "dmg2", 1);
            Repository.AddEffects(15, "dmg3", 1);

            Repository.AddEffects(50, "MoveSpeed1", 2);
            Repository.AddEffects(100, "MoveSpeed2", 2);
            Repository.AddEffects(150, "MoveSpeed3", 2);

            Repository.AddEffects(100, "AttackSpeed1", 3);
            Repository.AddEffects(150, "AttackSpeed2", 3);
            Repository.AddEffects(300, "AttackSpeed3", 3);

            Repository.AddEffects(5, "Health1", 4);
            Repository.AddEffects(10, "Health2", 4);
            Repository.AddEffects(20, "Health3", 4);

            Repository.Close();
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

            cyclebarDay = new CyclebarDay(Content);
            cyclebarNight = new CyclebarNight(Content);
            IsDay = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads all GameStates
            //Frederik
            GameState = new GameState();
            HowToState = new HowToState();
            MenuState = new MenuState();
            GameOverState = new GameOverState();
            currentGameState = new MenuState();
            HighscoreState = new HighscoreState();
            currentGameState.LoadContent();
            nextGameState = null;
            ///<summary>
            /// Loads Target Renderer: to run the game in the same resolution, no matter the pc - Frederik
            /// </summary>
            RenderTarget = new RenderTarget2D(GraphicsDevice, 3400, 3400);

            minimap = RenderTarget;

            camera = new Camera();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameState.ShutdownThreads();
                Repository.Open();
                Repository.RemoveTables();
                Repository.Close();
                this.Exit();
            }

            ///<summary>
            /// Sets Mouse to visible/invisible, and Updates/Loads current gamestate
            /// </summary>
            
            // ras forslag ?
            //if (currentGameState != GameState)
            //{
            //    IsMouseVisible = true;
            //}
            //else
            //{
            //    IsMouseVisible = false;

            //}

            if (currentGameState is HowToState)
            {
                IsMouseVisible = true;
            }
            if (currentGameState is MenuState)
            {
                IsMouseVisible = true;
            }
            if (currentGameState is GameOverState)
            {
                IsMouseVisible = true;
            }
            if (currentGameState is GameState)
            {
                IsMouseVisible = false;
            }
            if (nextGameState != null)
            {
                currentGameState = nextGameState;
                currentGameState.LoadContent();

                nextGameState = null;
            }

            //Updates game
            currentGameState.Update(gameTime);

            //currentGameState.PostUpdate(gameTime);
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentGameState is GameState)
            {
                isGameState = true;
                camera.Follow(GameState.playerBuilder);

                if (IsDay == true)
                {
                    cyclebarDay.Update();
                }
                if (IsDay == false)
                {
                    cyclebarNight.Update();
                }

            }

            else
            {
                isGameState = false;
            }


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


            GraphicsDevice.SetRenderTarget(RenderTarget);

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

            spriteBatch.Draw(RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (isGameState)
            {
                if (GameState.playerBuilder.Player.showingMap)
                {
                    spriteBatch.Draw(minimap, new Vector2(-camera.Transform.Translation.X, -camera.Transform.Translation.Y), null, Color.White, 0f, Vector2.Zero, miniMapScale, SpriteEffects.None, 0f);
                }


                if (IsDay == false)
                {


                    if (cyclebarNight.currentBarNight <= 0)
                    {
                        //isNight = false;
                        IsDay = true;
                        cyclebarDay.currentBarDay = cyclebarDay.fullBarDay;
                        GameState.days++;
                        GameState.SpawnEnemiesAcordingToDayNumber();

                    }
                    cyclebarNight.Draw(spriteBatch);
                }
                if (IsDay == true)
                {

                    if (cyclebarDay.currentBarDay <= 0)
                    {
                        IsDay = false;
                        //isNight = true;
                        cyclebarNight.currentBarNight = cyclebarNight.fullBarNight;
                    }
                    cyclebarDay.Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void SetInitials()
        {
            Window.TextInput += Highscores.CreateUsernameInput;
        }
        #endregion
    }
}
