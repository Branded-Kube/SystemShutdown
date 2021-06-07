using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using SystemShutdown.Database;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class GameWorld : Game
    {
        public  GraphicsDeviceManager graphics;
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
        public  ContentManager content;


        public  RenderTarget2D renderTarget;
        public  RenderTarget2D minimap;
        public float scale = 0.4444f;

        public float miniMapScale;

        public  int ScreenWidth = 1920;
        public  int ScreenHeight = 1080;

        private State currentGameState;
        private static State nextGameState;
        public  GameState gameState;
        public  HowToState howToState;
        public  MenuState menuState;
        public  GameOverState gameOverState;
        public  HighscoreState highscoreState;

        private Camera camera;

        private bool isGameState;

        public bool isDay;
        private CyclebarDay cyclebarDay;
        private CyclebarNight cyclebarNight;
        public  Repository repo;

        public SoundEffect walkEffect;
        public SoundEffect laserEffect;
        public SoundEffect laserEffect2;
        public SoundEffect deathEffect;
        public SoundEffect killEffect;
        public SoundEffect killEffect2;
        public SoundEffect killEffect3;
        public SoundEffect enemyEffect;
        public SoundEffect horseEffect;
        public SoundEffect horseEffect2;
        public SoundEffect pickedUp;
        public SoundEffect toggle;
        public SoundEffect toggle2;
        public SoundEffect clickButton;
        public SoundEffect clickButton2;
        public SoundEffect clickButton3;
        public SoundEffect clickButton4;
        public SoundEffect clickButton5;

        public  float DeltaTime { get; set; }
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

            repo.AddEffects(5, "dmg1", 1);
            repo.AddEffects(10, "dmg2", 1);
            repo.AddEffects(15, "dmg3", 1);

            repo.AddEffects(50, "MoveSpeed1", 2);
            repo.AddEffects(100, "MoveSpeed2", 2);
            repo.AddEffects(150, "MoveSpeed3", 2);

            repo.AddEffects(100, "AttackSpeed1", 3);
            repo.AddEffects(150, "AttackSpeed2", 3);
            repo.AddEffects(300, "AttackSpeed3", 3);

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Loads all GameStates
            //Frederik
            gameState = new GameState();
            howToState = new HowToState();
            menuState = new MenuState();
            gameOverState = new GameOverState();
            currentGameState = new MenuState();
            highscoreState = new HighscoreState();
            currentGameState.LoadContent();
            nextGameState = null;
            ///<summary>
            /// Loads Target Renderer: to run the game in the same resolution, no matter the pc - Frederik
            /// </summary>
            renderTarget = new RenderTarget2D(GraphicsDevice, 3400, 3400);

            minimap = renderTarget;

            camera = new Camera();

            // Load soundeffects
            walkEffect = content.Load<SoundEffect>("Sounds/walk3");
            laserEffect = content.Load<SoundEffect>("Sounds/laser1");
            laserEffect2 = content.Load<SoundEffect>("Sounds/laser2");
            deathEffect = content.Load<SoundEffect>("Sounds/dead");
            killEffect = content.Load<SoundEffect>("Sounds/kill");
            killEffect2 = content.Load<SoundEffect>("Sounds/kill2");
            killEffect3 = content.Load<SoundEffect>("Sounds/kill3");
            enemyEffect = content.Load<SoundEffect>("Sounds/enemy1");
            horseEffect = content.Load<SoundEffect>("Sounds/horse");
            horseEffect2 = content.Load<SoundEffect>("Sounds/horse2");
            pickedUp = content.Load<SoundEffect>("Sounds/pickup");
            toggle = content.Load<SoundEffect>("Sounds/toggle");
            toggle2 = content.Load<SoundEffect>("Sounds/toggle2");
            clickButton = content.Load<SoundEffect>("Sounds/click");
            clickButton2 = content.Load<SoundEffect>("Sounds/click2");
            clickButton3 = content.Load<SoundEffect>("Sounds/click3");
            clickButton4 = content.Load<SoundEffect>("Sounds/click4");
            clickButton5 = content.Load<SoundEffect>("Sounds/click5");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameState.ShutdownThreads();
                repo.Open();
                repo.RemoveTables();
                repo.Close();
                this.Exit();
            }

            ///<summary>
            /// Sets Mouse to visible/invisible, and Updates/Loads current gamestate
            /// </summary>
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
                camera.Follow(gameState.PlayerBuilder);

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
                if (gameState.PlayerBuilder.Player.showingMap)
                {
                    spriteBatch.Draw(minimap, new Vector2(-camera.Transform.Translation.X, -camera.Transform.Translation.Y), null, Color.White, 0f, Vector2.Zero, miniMapScale, SpriteEffects.None, 0f);
                }


                if (isDay == false)
                {


                    if (cyclebarNight.currentBarNight <= 0)
                    {
                        isDay = true;
                        cyclebarDay.currentBarDay = cyclebarDay.fullBarDay;
                        gameState.Days++;
                        gameState.SpawnEnemiesAcordingToDayNumber();

                    }
                    cyclebarNight.Draw(spriteBatch);
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
