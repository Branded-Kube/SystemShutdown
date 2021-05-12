using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemShutdown.Buttons;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Fields

        public static ContentManager content;
        public static SpriteFont font;
        private List<Enemy> enemies;
        private List<Enemy> delEnemies;
        private List<Button2> buttons;
        private Enemy enemy;
        public static bool running = true;
        private Button2 spawnEnemyBtn;
        private Button2 cpuBtn;
        private Button2 activeThreadsBtn;
        private Button2 shutdownThreadsBtn;
        private CPU cpu;
        private string enemyID = "";
        private Texture2D cpuTexture;
        private Texture2D standardBtn;

        public static RenderTarget2D renderTarget;
        public float scale = 0.4444f;

        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        private State currentGameState;
        private State nextGameState;

        private Camera camera;

        private bool isGameState;

        public static float DeltaTime { get; set; }

        #endregion

        #region Methods

        #region Constructor
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            content = Content;
            enemies = new List<Enemy>();
            delEnemies = new List<Enemy>();
            buttons = new List<Button2>();
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
            cpu = new CPU();

            /// <summary>
            /// Game runs at 60 fps
            /// Frederik
            /// </summary>
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0f);
            /// <summary>
            /// It will make Update run consistenly at 60 fps, regardless of our Draw
            /// It will try to draw frames to macth 60 fps, but update will allways run at 60
            /// It will render the game at the same framerate as your monitor
            /// This will 'kindof' separate how the game runs (Update), and how the game renders (Draw)
            /// Frederik
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

            font = content.Load<SpriteFont>("Fonts/font");
            standardBtn = content.Load<Texture2D>("Controls/button");
            cpuTexture = content.Load<Texture2D>("Textures/box");
            shutdownThreadsBtn = new Button2(800, 840, "Shutdown Threads", standardBtn);
            activeThreadsBtn = new Button2(1000, 840, "Thread info", standardBtn);
            spawnEnemyBtn = new Button2(500, 660, "Spawn Enemy", standardBtn);
            cpuBtn = new Button2(700, 700, "CPU", cpuTexture);

            spawnEnemyBtn.Click += SpawnEnemyBtn_Clicked;
            shutdownThreadsBtn.Click += ShutdownBtn_Clicked;
            activeThreadsBtn.Click += ActiveThreadsBtn_Clicked;
            buttons.Add(shutdownThreadsBtn);
            buttons.Add(activeThreadsBtn);
            buttons.Add(cpuBtn);


            //Loads all GameStates
            //Frederik

            currentGameState = new MenuState(this, Content);

            currentGameState.LoadContent();
            nextGameState = null;
            // Loads Target Renderer: to run the game in the same resolution, no matter the pc
            // Frederik
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            camera = new Camera();

        }

        protected override void Update(GameTime gameTime)
        {
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
                camera.Follow((GameState)currentGameState);

            }

            else
            {
                isGameState = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ShutdownThreads();
                Exit();
            }

            foreach (var button in buttons)
            {
                button.Update();
            }

            if (!enemies.Any())
            {

            }
            else
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.ThreadRunning == false)
                    {
                        enemies.Remove(enemy);
                    }
                }
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update();
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

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw TargetRenderer

            if (isGameState)
            {
                spriteBatch.Begin(transformMatrix: camera.Transform);
            }

            else
            {
                spriteBatch.Begin();
            }

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }
            //Draw selected Enemy ID
            spriteBatch.DrawString(font, $"Enemy: {enemyID} selected", new Vector2(300, 100), Color.Black);

            if (!enemies.Any())
            {

            }
            else
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.Draw(spriteBatch);
                }
            }


            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds an enemy when button is clicked, and also adds enemy to the other list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpawnEnemyBtn_Clicked(object sender, EventArgs e)
        {
            running = true;

            enemy = new Enemy($"Enemy ");
            enemy.Start();
            enemy.ClickSelect += Enemy_ClickSelect;
            enemies.Add(enemy);
            delEnemies.Add(enemy);
        }

        /// <summary>
        /// Enables clicking on the CPU, and sets enemy ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enemy_ClickSelect(object sender, EventArgs e)
        {
            cpuBtn.Click += CPU_Clicked;

            enemy = (Enemy)sender;
            int ID = enemy.id;
            Debug.WriteLine(ID);
            enemyID = ID.ToString();
        }

        /// <summary>
        /// Toggles bool on latest clicked enemy and removes click events on CPU
        /// Enemy thread enters CPU 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CPU_Clicked(object sender, EventArgs e)
        {
            enemy.Harvesting = true;

            cpuBtn.Click -= CPU_Clicked;
        }

        /// <summary>
        /// Shows all threads aktive and Total number
        /// Used for debugging purpose only and is not part of the game. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveThreadsBtn_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process procces = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.ProcessThreadCollection threadCollection = procces.Threads;
            string threads = string.Empty;
            foreach (System.Diagnostics.ProcessThread proccessThread in threadCollection)
            {
                threads += string.Format("Thread Id: {0}, ThreadState: {1}\r\n", proccessThread.Id, proccessThread.ThreadState);
            }
            Debug.WriteLine($"{threads}");
            int number = Process.GetCurrentProcess().Threads.Count;
            Debug.WriteLine($"Total number of aktive threads: {number}");
        }

        /// <summary>
        /// Shutdown all enemy threads and clears enemies from draw/update list
        /// Used both as a button for testing and at game exit
        /// </summary>
        public void ShutdownThreads()
        {
            running = false;
            enemies.Clear();
        }
        /// <summary>
        /// Calls ShutdownThreads method on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShutdownBtn_Clicked(object sender, EventArgs e)
        {
            ShutdownThreads();
        }
        #endregion
    }
}