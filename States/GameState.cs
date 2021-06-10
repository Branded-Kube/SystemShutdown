using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SystemShutdown.AStar;
using SystemShutdown.BuildPattern;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;

namespace SystemShutdown.States
{
    // Lead author: Frederik
    // Contributor: Ras
    // Contributor: Lau
    // Contributor: Søren
    public class GameState : State
    {
        #region Fields
        private Texture2D backgroundSprite;
        public Vector2 backgroundPos;
        public Vector2 backgroundOrigin;

        public Texture2D cursorSprite;
        public Vector2 cursorPosition;
        private Texture2D modboard;

        public static SpriteFont font;
        public bool IsThreadsRunning;

        private Song nightMusic;

        private List<GameObject> gameObjects = new List<GameObject>();
        private PlayerBuilder playerBuilder;
        private CPUBuilder cpuBuilder;

        private int aliveEnemies = 0;
        private int days = 1;
        private Grid grid;
        private List<Collider> colliders = new List<Collider>();

        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;

        private Color _healthColor = Color.White;
        private Color _dmgColor = Color.White;
        private Color _killsColor = Color.White;
        public Color _msColor = Color.White;
        public Color _asColor = Color.White;

        private List<ProjectileEffect> effects = new List<ProjectileEffect>();
        private double enemySpawnTimer = 0.0;

        private float dmgTimer = 2f;
        private float healthTimer = 2f;
        private float asTimer = 2f;
        private float msTimer = 2f;
        private float killsTimer = 2f;
        private float countDown = 0.05f;
        public List<ProjectileEffect> Effects { get { return effects; } set { effects = value; } }
        public Color HealthColor { get { return _healthColor; } set { _healthColor = value; } }
        public Color DmgColor { get { return _dmgColor; } set { _dmgColor = value; } }
        public Color KillsColor { get { return _killsColor; } set { _killsColor = value; } }
        public Color MsColor { get { return _msColor; } set { _msColor = value; } }
        public Color AsColor { get { return _asColor; } set { _asColor = value; } }

        public int AliveEnemies { get { return aliveEnemies; } set { aliveEnemies = value; } }
        public List<Collider> Colliders { get { return colliders; } set { colliders = value; } }
        public CPUBuilder CpuBuilder { get { return cpuBuilder; } set { cpuBuilder = value; } }
        public PlayerBuilder PlayerBuilder { get { return playerBuilder; } set { playerBuilder = value; } }
        public Grid Grid { get { return grid; } set { grid = value; } }
        public List<GameObject> GameObjects { get { return gameObjects; } set { gameObjects = value; } }
        public Vector2 CursorPosition { get { return cursorPosition; } set { cursorPosition = value; } }
        public int Days { get { return days; } set { days = value; } }

        public bool dmgColorTimer { get; set; }
        public bool healthColorTimer { get; set; }
        public bool msColorTimer { get; set; }
        public bool asColorTimer { get; set; }
        public bool killsColorTimer { get; set; }
        public bool DmgColorTimer { get; set; }
        public bool HealthColorTimerGreen { get; set; }
        public bool HealthColorTimerRed { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Instantiates a Player and a CPU builder
        /// </summary>
        public GameState()
        {
            PlayerBuilder = new PlayerBuilder();
            CpuBuilder = new CPUBuilder();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lau
        /// changes the color of the dmg stat shown ingame
        /// </summary>
        public void ChangeDmgColor()
        {
            DmgColor = Color.YellowGreen;
        }
        /// <summary>
        /// Lau
        /// changes the color of the health stat shown ingame
        /// </summary>
        public void PlusHealthColor()
        {
            HealthColor = Color.YellowGreen;
        }

        public void MinusHealthColor()
        {
            HealthColor = Color.Red;
        }
        public void ChangeAsColor()
        {
            AsColor = Color.YellowGreen;

        }
        public void ChangeMsColor()
        {
            MsColor = Color.YellowGreen;
        }
        public void ChangeKillsColor()
        {
            KillsColor = Color.YellowGreen;
        }

        public override void LoadContent()
        {
            backgroundSprite = content.Load<Texture2D>("Backgrounds/circuitboard");
            cursorSprite = content.Load<Texture2D>("Textures/cursoren");
            modboard = content.Load<Texture2D>("Textures/modboard");

            // Backgrounds music
            //dayMusic = content.Load<Song>("Sounds/song1");

            nightMusic = content.Load<Song>("Sounds/song02");

            MediaPlayer.Play(nightMusic);
            MediaPlayer.IsRepeating = true;

            Director directorCPU = new Director(CpuBuilder);
            GameObjects.Add(directorCPU.Contruct());

            Director director = new Director(PlayerBuilder);
            GameObjects.Add(director.Contruct());

            //Awakes and Starts each gameobject
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Awake();
            }

            // Frederik

            font = content.Load<SpriteFont>("Fonts/font");
            Grid = new Grid();

            // Enables threads to be run and spawns the first wave of enemies
            IsThreadsRunning = true;
        }

        /// <summary>
        /// Ras
        /// Spawn Enemies acording to daynumber and day/ night cycle.
        /// Spawns a enemy each day for each days passed.
        /// Double spawns in night cycle
        /// If day is > 5, trojan anemies is spawned aswell
        /// maximum numbers of enemies is 50. 
        /// </summary>
        public void SpawnEnemiesAcordingToDayNumber()
        {
            for (int i = 0; i < Days && i < 10; i++)
            {
                if (aliveEnemies < 50)
                {
                    if (GameWorld.Instance.IsDay)
                    {
                        SpawnBugEnemies(SetEnemySpawnInCorner());
                        if (i >= 3)
                        {
                            SpawnTrojanEnemies(SetEnemySpawnInCorner());
                        }
                    }
                    else
                    {
                        SpawnBugEnemies(SetEnemySpawnInCorner());
                        SpawnBugEnemies(SetEnemySpawnInCorner());
                        if (i >= 3)
                        {
                            SpawnTrojanEnemies(SetEnemySpawnInCorner());
                        }
                    }
                }
            }
            Debug.WriteLine($"Enemies alive {aliveEnemies}");

        }
        /// <summary>
        /// Ras
        /// Sets enemy start position to one of 4 corners of the grid with a Random (1.5). 
        /// </summary>
        /// <returns></returns>
        public Vector2 SetEnemySpawnInCorner()
        {
            Random rndd = new Random();
            var rndpos = rndd.Next(1, 5);
            int x = 0;
            int y = 0;

            if (rndpos == 1)
            {
                x = 1;
                y = 1;
            }
            else if (rndpos == 2)
            {
                x = GameWorld.Instance.GameState.Grid.Width - 2;
                y = 1;
            }
            else if (rndpos == 3)
            {
                x = 1;
                y = GameWorld.Instance.GameState.Grid.Height - 2;
            }
            else if (rndpos == 4)
            {
                x = GameWorld.Instance.GameState.Grid.Width - 2;
                y = GameWorld.Instance.GameState.Grid.Height - 2;
            }
            return new Vector2(x * 100, y * 100);

        }
        public override void Update(GameTime gameTime)
        {
            enemySpawnTimer += GameWorld.Instance.DeltaTime;
            if (enemySpawnTimer >= 10)
            {
                SpawnEnemiesAcordingToDayNumber();
                enemySpawnTimer = 0.0;
            }

            backgroundPos = new Vector2(GameWorld.Instance.RenderTarget.Width / 2, GameWorld.Instance.RenderTarget.Height / 2);
            backgroundOrigin = new Vector2(backgroundSprite.Width / 2, backgroundSprite.Height / 2);

            ///<summary>
            ///Updates cursors position
            /// </summary>
            CursorPosition = new Vector2(PlayerBuilder.player.Distance.X - cursorSprite.Width / 2,
                PlayerBuilder.player.Distance.Y) + PlayerBuilder.player.GameObject.Transform.Position;
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            ///<summary>
            /// Goes to gameover menu and shuts down all Threads - Frederik
            /// </summary> 
            if (currentKeyState.IsKeyUp(Keys.Escape) && !previousKeyState.IsKeyUp(Keys.Escape))
            {
                ShutdownThreads();
                GameWorld.ChangeState(new GameOverState());
                GameWorld.Instance.IsDay = true;

                GameWorld.Instance.cyclebarDay.resetDay();
                GameWorld.Instance.cyclebarNight.resetNight();

            }


#if DEBUG
            // Spawns a bug in debug by pressing P
            if (currentKeyState.IsKeyDown(Keys.P) && !previousKeyState.IsKeyDown(Keys.P))
            {
                SpawnBugEnemies(SetEnemySpawnInCorner());
            }

#endif

            // Updates each gameobject
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(gameTime);
            }
            // Checks for collisions between gameobjects colliders
            Collider[] tmpColliders = colliders.ToArray();
            for (int i = 0; i < tmpColliders.Length; i++)
            {
                for (int j = 0; j < tmpColliders.Length; j++)
                {
                    tmpColliders[i].OnCollisionEnter(tmpColliders[j]);
                }
            }

            if (DmgColorTimer == true)
            {
                ChangeDmgColor();
                dmgTimer -= countDown;

                if (dmgTimer <= 0)
                {
                    DmgColorTimer = false;
                    Debug.WriteLine("IT WORKS!!!");
                    DmgColor = Color.White;
                    dmgTimer = 2f;
                }
            }
            if (HealthColorTimerGreen == true)
            {
                PlusHealthColor();
                healthTimer -= countDown;

                if (healthTimer <= 0)
                {
                    HealthColorTimerGreen = false;
                    Debug.WriteLine("IT WORKS for health aswell!!!");
                    HealthColor = Color.White;
                    healthTimer = 2f;
                }
            }
            if (msColorTimer == true)
            {
                ChangeMsColor();
                msTimer -= countDown;

                if (msTimer <= 0)
                {
                    msColorTimer = false;
                    Debug.WriteLine("IT WORKS for move speed aswell!!!");
                    MsColor = Color.White;
                    msTimer = 2f;
                }
            }
            if (asColorTimer == true)
            {
                ChangeAsColor();
                asTimer -= countDown;

                if (asTimer <= 0)
                {
                    asColorTimer = false;
                    Debug.WriteLine("IT WORKS for attack speed aswell!!!");
                    AsColor = Color.White;
                    asTimer = 2f;
                }
            }
            if (killsColorTimer == true)
            {
                ChangeKillsColor();
                killsTimer -= countDown;

                if (killsTimer <= 0)
                {
                    killsColorTimer = false;
                    Debug.WriteLine("IT WORKS for kills aswell!!!");
                    KillsColor = Color.White;
                    killsTimer = 2f;
                }
            }
            if (HealthColorTimerRed == true)
            {
                MinusHealthColor();
                healthTimer -= countDown;

                if (healthTimer <= 0)
                {
                    HealthColorTimerRed = false;
                    HealthColor = Color.White;
                    healthTimer = 2f;
                }
            }
            // Ras
            foreach (ProjectileEffect item in new List<ProjectileEffect>(effects))
            {
                item.Update(gameTime);
            }
            GameOver();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundSprite, backgroundPos, null, Color.White, 0, backgroundOrigin, 1f, SpriteEffects.None, 0.1f);

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(spriteBatch);
            }

            // Draws CPU Health
            spriteBatch.DrawString(font, $"CPU Health: {CpuBuilder.Cpu.Health}", new Vector2(CpuBuilder.Cpu.GameObject.Transform.Position.X - 120, CpuBuilder.Cpu.GameObject.Transform.Position.Y + 140), Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            var tmpeffects = effects;
            foreach (ProjectileEffect item in tmpeffects)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Draws Player stats
        /// </summary>
        public void DrawPlayerStats(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(modboard, new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 916, PlayerBuilder.Player.GameObject.Transform.Position.Y + 206), Color.White);
            spriteBatch.DrawString(font, $"  | Player Stats | ", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 845, PlayerBuilder.Player.GameObject.Transform.Position.Y + 306), Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, $"  Kills:  {PlayerBuilder.Player.Kills}", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 845, PlayerBuilder.Player.GameObject.Transform.Position.Y + 346), _killsColor, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, $"  Health: {PlayerBuilder.Player.Health}", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 845, PlayerBuilder.Player.GameObject.Transform.Position.Y + 371), _healthColor, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, $"  Damage:  {PlayerBuilder.Player.dmg}", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 845, PlayerBuilder.Player.GameObject.Transform.Position.Y + 396), _dmgColor, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, $"  Fire rate:  {PlayerBuilder.Player.Cooldown}", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 845, PlayerBuilder.Player.GameObject.Transform.Position.Y + 421), _asColor, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, $"  Speed:  {PlayerBuilder.Player.Speed}", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 845, PlayerBuilder.Player.GameObject.Transform.Position.Y + 446), _msColor, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, $"  Day:  {Days}", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X + 530, PlayerBuilder.Player.GameObject.Transform.Position.Y - 385), Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);

            if (!PlayerBuilder.player.HasUsedMap)
            {
                spriteBatch.DrawString(font, "Click M to hide the map", new Vector2(PlayerBuilder.Player.GameObject.Transform.Position.X - 870, PlayerBuilder.Player.GameObject.Transform.Position.Y - 150), Color.Red, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
            }
        }



        /// <summary>
        /// Ras
        /// Spawns a Trojan enemy at parameter position. 
        /// </summary>
        /// <param name="position"></param>
        private void SpawnTrojanEnemies(Vector2 position)
        {
            GameObject go = EnemyFactory.Instance.Create(position, "Trojan");
            AddGameObject(go);
        }
        /// <summary>
        /// Ras
        /// Spawns a Bug enemy at parameter position. 
        /// </summary>
        /// <param name="position"></param>
        public void SpawnBugEnemies(Vector2 position)
        {
            GameObject go = EnemyFactory.Instance.Create(position, "Bug");
            AddGameObject(go);
        }

        /// <summary>
        /// Runs Start and Awake on GameObject
        /// Adds the Gameobject to Gameobjects list
        /// Adds collider component to list of colliders
        /// </summary>
        /// <param name="go"></param>
        public void AddGameObject(GameObject go)
        {
            go.Awake();
            GameObjects.Add(go);
            Collider c = (Collider)go.GetComponent("Collider");
            if (c != null)
            {
                colliders.Add(c);
            }
        }

        /// <summary>
        /// Removes a Gameobject from Gameobjects list.
        /// </summary>
        /// <param name="go"></param>
        public void RemoveGameObject(GameObject go)
        {
            GameObjects.Remove(go);
        }

        /// <summary>
        /// If Player or CPU Health is 0 or below. 
        /// Shuts down all threads, change state from gamestate to gameOverState 
        /// </summary>
        public void GameOver()
        {
            if (GameWorld.Instance.GameState.CpuBuilder.Cpu.Health <= 0 || GameWorld.Instance.GameState.PlayerBuilder.Player.Health <= 0)
            {
                GameWorld.Instance.deathEffect.Play();
                ShutdownThreads();
                GameWorld.ChangeState(GameWorld.Instance.GameOverState);
                GameWorld.Instance.IsDay = true;
                GameWorld.Instance.cyclebarDay.resetDay();
                GameWorld.Instance.cyclebarNight.resetNight();
            }
        }

        /// <summary>
        /// Shutdown all enemy threads 
        /// </summary>
        public void ShutdownThreads()
        {
            IsThreadsRunning = false;
        }
        #endregion
    }
}