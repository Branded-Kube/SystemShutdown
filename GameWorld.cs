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
        #region Fields

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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static GameWorld instance;

        private bool isGameState;
        private bool isDay;

        private float deltaTime;
        private float scale = 0.4444f;
        private float miniMapScale;

        private int screenWidth = 1920;
        private int screenHeight = 1080;
        private RenderTarget2D renderTarget;
        private RenderTarget2D minimap;
        private Camera camera;

        private State currentGameState;
        private static State nextGameState;
        private GameState gameState;
        private HowToState howToState;
        private MenuState menuState;
        private GameOverState gameOverState;
        private HighscoreState highscoreState;

        private CyclebarDay cyclebarDay;
        private CyclebarNight cyclebarNight;
        public Texture2D darkSprite;

        private Repository repo;

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

        public Repository Repo { get { return repo; } set { repo = value; } }
        public int ScreenWidth { get { return screenWidth; } set { screenWidth = value; } }
        public int ScreenHeight { get { return screenHeight; } set { screenHeight = value; } }
        public RenderTarget2D RenderTarget { get { return renderTarget; } set { renderTarget = value; } }
        public GameOverState GameOverState { get { return gameOverState; } set { gameOverState = value; } }
        public GameState GameState { get { return gameState; } set { gameState = value; } }
        public HighscoreState HighscoreState { get { return highscoreState; } set { highscoreState = value; } }
        public HowToState HowToState { get { return howToState; } set { howToState = value; } }
        public MenuState MenuState { get { return menuState; } set { menuState = value; } }
        public bool IsDay { get { return isDay; } set { isDay = value; } }
        public float DeltaTime { get { return deltaTime; } set { deltaTime = value; } }
        #endregion

        #region Methods

        #region Constructor
        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            var mapper = new Mapper();
            var provider = new SQLiteDatabaseProvider("Data Source=SystemShutdown.db;Version=3;new=true");

            Repo = new Repository(provider, mapper);
            Repo.Open();
            Repo.AddMods("Dmg"); //ID = 1
            Repo.AddMods("Movespeed"); //ID = 2
            Repo.AddMods("Attackspeed"); //ID = 3
            Repo.AddMods("Health"); //ID = 4

            Repo.AddEffects(5, "dmg1", 1);
            Repo.AddEffects(10, "dmg2", 1);
            Repo.AddEffects(15, "dmg3", 1);

            Repo.AddEffects(50, "MoveSpeed1", 2);
            Repo.AddEffects(100, "MoveSpeed2", 2);
            Repo.AddEffects(150, "MoveSpeed3", 2);

            Repo.AddEffects(100, "AttackSpeed1", 3);
            Repo.AddEffects(150, "AttackSpeed2", 3);
            Repo.AddEffects(300, "AttackSpeed3", 3);

            Repo.AddEffects(5, "Health1", 4);
            Repo.AddEffects(10, "Health2", 4);
            Repo.AddEffects(20, "Health3", 4);

            Repo.Close();
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

            // Load soundeffects
            walkEffect = Content.Load<SoundEffect>("Sounds/walk3");
            laserEffect = Content.Load<SoundEffect>("Sounds/laser1");
            laserEffect2 = Content.Load<SoundEffect>("Sounds/laser2");
            deathEffect = Content.Load<SoundEffect>("Sounds/dead");
            killEffect = Content.Load<SoundEffect>("Sounds/kill");
            killEffect2 = Content.Load<SoundEffect>("Sounds/kill2");
            killEffect3 = Content.Load<SoundEffect>("Sounds/kill3");
            enemyEffect = Content.Load<SoundEffect>("Sounds/enemy1");
            horseEffect = Content.Load<SoundEffect>("Sounds/horse");
            horseEffect2 = Content.Load<SoundEffect>("Sounds/horse2");
            pickedUp = Content.Load<SoundEffect>("Sounds/pickup");
            toggle = Content.Load<SoundEffect>("Sounds/toggle");
            toggle2 = Content.Load<SoundEffect>("Sounds/toggle2");
            clickButton = Content.Load<SoundEffect>("Sounds/click");
            clickButton2 = Content.Load<SoundEffect>("Sounds/click2");
            clickButton3 = Content.Load<SoundEffect>("Sounds/click3");
            clickButton4 = Content.Load<SoundEffect>("Sounds/click4");
            clickButton5 = Content.Load<SoundEffect>("Sounds/click5");
            darkSprite = Content.Load<Texture2D>("darksprite2");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameState.ShutdownThreads();
                //repo.Open();
                //repo.RemoveTables();
                //repo.Close();
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
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentGameState is GameState)
            {
                isGameState = true;
                camera.Follow(GameState.PlayerBuilder);

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

                if (!IsDay)
                {
                    spriteBatch.Draw(darkSprite, new Vector2(-camera.Transform.Translation.X + 1700, -camera.Transform.Translation.Y + 1700), null, Color.White * 0.4f, 0, GameWorld.Instance.GameState.backgroundOrigin, 1f, SpriteEffects.None, 1f);

                }
                if (GameState.PlayerBuilder.Player.showingMap)
                {
                    spriteBatch.Draw(minimap, new Vector2(-camera.Transform.Translation.X, -camera.Transform.Translation.Y), null, Color.White, 0f, Vector2.Zero, miniMapScale, SpriteEffects.None, 0f);
                }
                if (IsDay == false)
                {
                    if (cyclebarNight.currentBarNight <= 0)
                    {
                        IsDay = true;
                        cyclebarDay.currentBarDay = cyclebarDay.fullBarDay;
                        GameState.Days++;
                      //  GameState.SpawnEnemiesAcordingToDayNumber();
                    }
                    cyclebarNight.Draw(spriteBatch);
                }
                if (IsDay == true)
                {
                    if (cyclebarDay.currentBarDay <= 0)
                    {
                        IsDay = false;
                        cyclebarNight.currentBarNight = cyclebarNight.fullBarNight;
                    }
                    cyclebarDay.Draw(spriteBatch);
                    //GameState.SpawnEnemiesAcordingToDayNumber();

                }
                GameState.DrawPlayerStats(spriteBatch);

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
