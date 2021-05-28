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

        private State currentGameState;
        private static State nextGameState;
        public static GameState gameState;

        private Camera camera;
        
        private bool isGameState;
        private bool isDay;
        private bool isNight;
        private CyclebarDay cyclebarDay;
        private CyclebarNight cyclebarNight;
        public static Repository repo;

        public static float DeltaTime { get; set; }
        Random rnd = new Random();
        private CPU cpu;

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

            repo.AddMods("Dmg"); //ID = 1
            repo.AddMods("Movespeed"); //ID = 2
            repo.AddMods("Attackspeed"); //ID = 3
            repo.AddMods("Health"); //ID = 4

            repo.AddEffects(2, "dmg1", 1);
            repo.AddEffects(5, "dmg2", 1);
            repo.AddEffects(10, "dmg3", 1);

            repo.AddEffects(100, "MoveSpeed1", 2);
            repo.AddEffects(200, "MoveSpeed2", 2);
            repo.AddEffects(300, "MoveSpeed3", 2);

            repo.AddEffects(10, "AttackSpeed1", 3);
            repo.AddEffects(20, "AttackSpeed2", 3);
            repo.AddEffects(30, "AttackSpeed3", 3);

            repo.AddEffects(5, "Health1", 4);
            repo.AddEffects(10, "Health2", 4);
            repo.AddEffects(20, "Health3", 4);

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

            cyclebarDay = new CyclebarDay(content);
            cyclebarNight = new CyclebarNight(content);
            isDay = true;
            isNight = false;

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

            //cyclebarDay = new CyclebarDay(content);
            //cyclebarNight = new CyclebarNight(content);

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
                IsMouseVisible = false;
            }
            //Updates game
            currentGameState.Update(gameTime);

            currentGameState.PostUpdate(gameTime);
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentGameState is GameState)
            {
                isGameState = true;
                camera.Follow(gameState.playerBuilder);

                //if (isDay == true)
                //{
                //    cyclebarDay.Update();
                //    if (cyclebarDay.currentBarDay <= 0)
                //    {
                //        isDay = false;
                //        isNight = true;
                //    }
                //}
                //if (isNight == true)
                //{
                //    isDay = false;
                //    cyclebarNight.Update();
                //    if (cyclebarNight.currentBarNight <= 0)
                //    {
                //        isNight = false;
                //        isDay = true;
                //    }
                //}
                
                if (isDay == true)
                {
                    cyclebarDay.Update();
                }
                if (isDay == false)
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
           
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (isGameState)
            {
                if (gameState.playerBuilder.Player.showingMap)
                {
                    spriteBatch.Draw(minimap, new Vector2(-camera.Transform.Translation.X, -camera.Transform.Translation.Y), null, Color.White, 0f, Vector2.Zero, miniMapScale, SpriteEffects.None, 0f);
                }

                if (isDay == true)
                {
                    

                    if (cyclebarDay.currentBarDay <= 0)
                    {
                        isDay = false;
                        //isNight = true;
                        cyclebarNight.currentBarNight = cyclebarNight.fullBarNight;
                    }
                    cyclebarDay.Draw(spriteBatch);
                }

                if (isDay == false)
                {
                    

                    if (cyclebarNight.currentBarNight <= 0)
                    {
                        //isNight = false;
                        isDay = true;
                        cyclebarDay.currentBarDay = cyclebarDay.fullBarDay;
                    }
                    cyclebarNight.Draw(spriteBatch);
                }
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}