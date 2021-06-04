using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SystemShutdown.AStar;
using SystemShutdown.BuildPattern;
using SystemShutdown.Buttons;
using SystemShutdown.CommandPattern;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.GameObjects;
using SystemShutdown.ObjectPool;

namespace SystemShutdown.States
{
    public class GameState : State
    {
        #region Fields
        private SpriteBatch _spriteBatch;
        public Texture2D backgroundSprite;
        public Vector2 backgroundPos;
        public Vector2 backgroundOrigin;
        public Texture2D cursorSprite;
        public Vector2 cursorPosition;
        public static SpriteFont font;
        private List<Enemy> delEnemies;
        public bool running = true;
        private string enemyID = "";

        private CyclebarDay cyclebarDay;
        private CyclebarNight cyclebarNight;
        //private List<StateObject> stateObjects;
        public List<GameObject1> gameObjects = new List<GameObject1>();
       
        private InputHandler inputHandler;

        public PlayerBuilder playerBuilder;
        public EnemyFactory enemyFactory;

        public CPUBuilder cpuBuilder;

           public int aliveEnemies = 0;

        Texture2D rectTexture;
        public int days = 1;


        public Grid grid;

        public int NodeSize;

        private Component component;
        private Collider collision;
        private List<Collider> colliders;

        public List<Collider> Colliders { get; set; } = new List<Collider>();

        private Collider collide;
        private Collider Collider
        {
            get { return collide; }
            set { collide = value; }
        }

        Astar aStar;
        
        //public Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        private SpriteRenderer spriteRenderer;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

        //public Vector2 position;
        public Rectangle rectangle;
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
        protected Vector2 velocity;


        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;

        #endregion

        #region Methods

        #region Constructor
        public GameState()
        {
            delEnemies = new List<Enemy>();
            // cpu = new CPU();

            //Director director = new Director(new PlayerBuilder());
            //gameObjects.Add(director.Contruct());

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Awake();
            //}

            //gameObjects.Add(GameWorld.Instance.Director.Contruct());

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Awake();
            //}

            playerBuilder = new PlayerBuilder();
            cpuBuilder = new CPUBuilder();
            enemyFactory = new EnemyFactory();
        }
        #endregion

        public override void LoadContent()
        {
            backgroundSprite = content.Load<Texture2D>("Backgrounds/circuitboard");
            cursorSprite = content.Load<Texture2D>("Textures/cursoren");

            Director directorCPU = new Director(cpuBuilder);
            gameObjects.Add(directorCPU.Contruct());

            Director director = new Director(playerBuilder);
            gameObjects.Add(director.Contruct());
            
            //DirectorCPU directorCpu = new DirectorCPU(cpuBuilder);
            //gameObjects.Add(directorCpu.Contruct());

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }

            Player.DamagePlayer += Player_DamagePlayer;
            CPU.DamageCPU += CPU_DamageCPU;

            cyclebarDay = new CyclebarDay(content);
            cyclebarNight = new CyclebarNight(content);

            //camera = new Camera();
            //camera.Follow(playerBuilder);

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Start();
            //}
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }
            
            // Frederik
            inputHandler = new InputHandler();

            font = content.Load<SpriteFont>("Fonts/font");

            //stateObjects = new List<StateObject>()
            //{
            //    new StateObject()
            //    {
            //        position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
            //        origin = new Vector2(backgroundSprite.Width / 2, backgroundSprite.Height / 2),
            //    }
            //};

            grid = new Grid();
            NodeSize = grid.NodeSize;

            _spriteBatch = new SpriteBatch(GameWorld.Instance.GraphicsDevice);

            var playerTexture = GameWorld.Instance.GameState.playerBuilder.Player.GameObject.Tag;
            //var wallTexture = GameWorld.gameState.component.Node.GameObject.Tag;

            //var colliderTexture = "Collider"/*GameWorld.gameState.Collider.GameObject.Tag*/;

            colliders = new List<Collider>()
            {
                //new Player()
                //{

                //},
                
            };
            SpawnEnemiesAcordingToDayNumber();
        }

        private void CPU_DamageCPU(object source, Enemy enemy, EventArgs e)
        {
            cpuBuilder.Cpu.Health -= enemy.Dmg;

        }

        private void Player_DamagePlayer(object source, Enemy enemy,EventArgs e)
        {
            playerBuilder.player.Health -= enemy.Dmg;
        }

        public void SpawnEnemiesAcordingToDayNumber()
        {
            Debug.WriteLine($"{aliveEnemies}");


            //for (int i = 0; i < days && i < 10; i++)
            //{
            //    if (aliveEnemies < 50)
            //    {
            //        SpawnBugEnemies(SetSpawnInCorner());
            //        SpawnBugEnemies(SetSpawnInCorner());
            //        SpawnBugEnemies(SetSpawnInCorner());
            //        SpawnBugEnemies(SetSpawnInCorner());
            //        SpawnBugEnemies(SetSpawnInCorner());
            //        SpawnTrojanEnemies(SetSpawnInCorner());


            //    }
            //}
        }
        public Vector2 SetSpawnInCorner()
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
                x = GameWorld.Instance.GameState.grid.Width - 2;
                y = 1;
            }
            else if (rndpos == 3)
            {
                x = 1;
                y = GameWorld.Instance.GameState.grid.Height - 2;
            }
            else if (rndpos == 4)
            {
                x = GameWorld.Instance.GameState.grid.Width - 2;
                y = GameWorld.Instance.GameState.grid.Height - 2;
            }
            //Node tmpvector = GameWorld.gameState.grid.Node(x,y);
            return new Vector2(x * 100, y * 100);

        }
        public override void Update(GameTime gameTime)
        {
            backgroundPos = new Vector2(GameWorld.Instance.RenderTarget.Width / 2, GameWorld.Instance.RenderTarget.Height / 2);
            backgroundOrigin = new Vector2(backgroundSprite.Width / 2, backgroundSprite.Height / 2);

            ///<summary>
            ///Updates cursors position
            /// </summary>
            cursorPosition = new Vector2(playerBuilder.player.distance.X - cursorSprite.Width / 2, 
                playerBuilder.player.distance.Y) + playerBuilder.player.GameObject.Transform.Position;

            previousKeyState = currentKeyState;

            currentKeyState = Keyboard.GetState();
            ///<summary>
            /// Goes back to main menu and shuts down all Threads - Frederik
            /// </summary> 
            //if (Keyboard.GetState().IsKeyDown(Keys.Back))
            //{
            //    ShutdownThreads();
            //    GameWorld.ChangeState(new MenuState());
            //}
            if (currentKeyState.IsKeyDown(Keys.P) && !previousKeyState.IsKeyDown(Keys.P))
            {
                SpawnBugEnemies(SetSpawnInCorner());


            }

            //if (currentKeyState.IsKeyDown(Keys.RightShift) && !previousKeyState.IsKeyDown(Keys.RightShift))
            //{
            //    Mods mods = new Mods();
            //    mods.Create();

            //}

            // Rotates player
            playerBuilder.player.RotatePlayer();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }
            InputHandler.Instance.Execute();


            Collider[] tmpColliders = Colliders.ToArray();
            for (int i = 0; i < tmpColliders.Length; i++)
            {
                for (int j = 0; j < tmpColliders.Length; j++)
                {
                    tmpColliders[i].OnCollisionEnter(tmpColliders[j]);
                }
            }

            GameOver();
        }

        //public override void PostUpdate(GameTime gameTime)
        //{
        //    //// When sprites collide = attacks colliding with enemy (killing them) (unload game-specific content)

        //    //// If player is dead, show game over screen
        //    //// Frederik
        //    //if (players.All(c => c.IsDead))
        //    //{
        //    //    //highscores can also be added here (to be shown in the game over screen)

        //    //    _game.ChangeState(new GameOverState(_game, content));
        //    //}
        //}


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.Begin();

            // Frederik
            //float x = 10f;
            //foreach (var player in players)
            //{
            //    spriteBatch.DrawString(font, "Player: ", /*+ player name,*/ new Vector2(x, 10f), Color.White);
            //    spriteBatch.DrawString(font, "Health: ", /*+ health,*/ new Vector2(x, 30f), Color.White);
            //    spriteBatch.DrawString(font, "Score: ", /*+ score,*/ new Vector2(x, 50f), Color.White);

            //    x += 150;
            //}

            spriteBatch.Draw(backgroundSprite, backgroundPos, null, Color.White, 0, backgroundOrigin, 1f, SpriteEffects.None, 0.1f);

            // Frederik
            //foreach (var sprite in stateObjects)
            //{
            //    sprite.Draw(gameTime, spriteBatch);
            //}

            //Draw selected Enemy ID
            spriteBatch.DrawString(font, $"Enemy: {enemyID} selected", new Vector2(300, 100), Color.Black);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }

            //Vector2 gridPosition = new Vector2(0, 0);
            //Vector2 pos = gridPosition;
            //int margin = 0;

            //for (int j = 0; j < grid.Height; j++)
            //{
            //    pos.Y = j * (NodeSize + margin) + gridPosition.Y;
            //    for (int i = 0; i < grid.Width; i++)
            //    {
            //       // grid.Node(i, j).rectangle(new Point(i * 100, j * 100));

            //        pos.X = i * (NodeSize + margin) + gridPosition.X;
            //        //grid.Node(i, j).rectangle((int)pos.X, (int)pos.Y, rectTexture.Width, rectTexture.Height);

            //        if (grid.Node(i, j).Passable)
            //        {
            //            //if (goal.x == i && goal.y == j)
            //            //{
            //            //    //spriteBatch.Draw(rectTexture, pos, Color.Blue);
            //            //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.Blue);

            //            //}
            //            //else if (grid.Node(i, j).Path)
            //            //{
            //            //    //spriteBatch.Draw(rectTexture, pos, Color.LightBlue);
            //            //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.LightBlue);

            //            //}
            //            //else if (grid.Node(i, j).Open)
            //            //{
            //            //    //spriteBatch.Draw(rectTexture, pos, Color.LightCoral);
            //            //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.LightCoral);

            //            //}
            //            //else if (grid.Node(i, j).Closed)
            //            //{
            //            //    //spriteBatch.Draw(rectTexture, pos, Color.RosyBrown);
            //            //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.RosyBrown);

            //            //}
            //            //else
            //            //{
            //            //    //spriteBatch.Draw(rectTexture, pos, Color.White);
            //            //spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.White);

            //            //}
            //        }
            //        else
            //        {

            //           // spriteBatch.Draw(rectTexture, pos, Color.Gray);
            //           //spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.Gray);


            //        }
            //        //if (grid.Node(i, j).nodeOccupiedByEnemy)
            //        //{
            //        //    grid.Node(i, j).Passable = false;
            //        //}
            //        //else if (true)
            //        //{
            //        //    grid.Node(i, j).Passable = true;

            //        //}

            //    }
            //}

            //

            //Draws cursor
            spriteBatch.Draw(cursorSprite, cursorPosition, Color.White);
            spriteBatch.DrawString(font, $"{GameWorld.Instance.GameState.playerBuilder.Player.kills} kills", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X, playerBuilder.Player.GameObject.Transform.Position.Y + 0), Color.White);
            spriteBatch.DrawString(font, $"{GameWorld.Instance.GameState.playerBuilder.Player.Health} health points", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X, playerBuilder.Player.GameObject.Transform.Position.Y +20), Color.White);
            spriteBatch.DrawString(font, $"{GameWorld.Instance.GameState.playerBuilder.Player.dmg} dmg points", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X , playerBuilder.Player.GameObject.Transform.Position.Y +40), Color.White);
            spriteBatch.DrawString(font, $"{days} Days gone", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X, playerBuilder.Player.GameObject.Transform.Position.Y + 60), Color.White);
            spriteBatch.DrawString(font, $"{playerBuilder.Player.playersMods.Count} Mods", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X, playerBuilder.Player.GameObject.Transform.Position.Y + 80), Color.White);

            spriteBatch.DrawString(font, $"CPU health {cpuBuilder.Cpu.Health}", cpuBuilder.Cpu.GameObject.Transform.Position, Color.White);


            spriteBatch.End();

        }

        //private void SpawnMod()
        //{
        //    List<Mods> pickupAble;

        //    GameWorld.repo.Open();
        //    pickupAble = GameWorld.repo.FindMods(Mods.Id);
        //}



        //private Node GetRandomPassableNode()
        //{
        //    Random rndd = new Random();
        //    var tmppos = grid.nodes[rndd.Next(1, grid.Width), rndd.Next(1, grid.Height)];
        //    //var tmppos = grid.nodes[1,1];

        //    return tmppos;
        //}

        private void SpawnTrojanEnemies(Vector2 position)
        {
            //spawnTime += delta;
            //if (spawnTime >= cooldown)
            //{
           // GameObject1 go = EnemyPool.Instance.GetObject(position, "Trojan");
            GameObject1 go = EnemyFactory.Instance.Create(position, "Trojan");

            running = true;
            AddGameObject(go);

        }
        public void SpawnBugEnemies(Vector2 position)
        {
            //spawnTime += delta;
            //if (spawnTime >= cooldown)
            //{
            GameObject1 go = EnemyFactory.Instance.Create(position, "Bug");
            //GameObject1 go = EnemyPool.Instance.GetObject(position, "Bug");
        
            running = true;
            AddGameObject(go);


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

        public void GameOver()
        {
            if (GameWorld.Instance.GameState.cpuBuilder.Cpu.Health <= 0 || GameWorld.Instance.GameState.playerBuilder.Player.Health <= 0)
            {
                ShutdownThreads();
                GameWorld.Instance.Repository.Open();
                GameWorld.Instance.Repository.RemoveTables();
                GameWorld.Instance.Repository.Close();
                GameWorld.ChangeState(GameWorld.Instance.GameOverState);
            }
        }

        /// <summary>
        /// Shutdown all enemy threads and clears enemies from draw/update list
        /// Used both as a button for testing and at game exit
        /// </summary>
        public void ShutdownThreads()
        {
            running = false;
        }

        //protected void Animate(GameTime gametime)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
        //    {
        //        //Giver tiden, der er gået, siden sidste update
        //        timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

        ////        //Beregner currentIndex
        ////        currentIndex = (int)(timeElapsed * fps);
        ////        spriteRenderer.Sprite = upWalk[currentIndex];

        //        //Checks if animation needs to restart
        //        if (currentIndex >= upWalk.Length - 1)
        //        {
        //            //Resets animation
        //            timeElapsed = 0;
        //            currentIndex = 0;
        //        }
        //    }
        //}
        #endregion
    }
}