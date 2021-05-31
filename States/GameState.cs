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
        private List<Button2> buttons;
        public bool running = true;
        private Button2 spawnEnemyBtn;
        private Button2 cpuBtn;
        private Button2 activeThreadsBtn;
        private Button2 shutdownThreadsBtn;
        private CPU cpu;
        private string enemyID = "";
        private Texture2D cpuTexture;
        private Texture2D standardBtn;

        private CyclebarDay cyclebarDay;
        private CyclebarNight cyclebarNight;
        //private List<StateObject> stateObjects;
        private List<GameObject1> gameObjects = new List<GameObject1>();
       
        private InputHandler inputHandler;

        public PlayerBuilder playerBuilder;
        public EnemyFactory enemyFactory;

        public CPUBuilder cpuBuilder;

     
        Texture2D rectTexture;



        public Grid grid;

        public int NodeSize = Grid.NodeSize;

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
            buttons = new List<Button2>();
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
            //backgroundSprite = content.Load<Texture2D>("");
            cursorSprite = content.Load<Texture2D>("Textures/cursoren");

            Director director = new Director(playerBuilder);
            gameObjects.Add(director.Contruct());

            Director directorCPU = new Director(cpuBuilder);
            gameObjects.Add(directorCPU.Contruct());
            //DirectorCPU directorCpu = new DirectorCPU(cpuBuilder);
            //gameObjects.Add(directorCpu.Contruct());

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }
            //Frederik
            font = content.Load<SpriteFont>("Fonts/font");
            standardBtn = content.Load<Texture2D>("Controls/button");
            cpuTexture = content.Load<Texture2D>("Textures/box");
            shutdownThreadsBtn = new Button2(800, 840, "Shutdown Threads", standardBtn);
            activeThreadsBtn = new Button2(1000, 840, "Thread info", standardBtn);
            spawnEnemyBtn = new Button2(150, 10, "Spawn Enemy", standardBtn);
            cpuBtn = new Button2(700, 700, "CPU", cpuTexture);

            shutdownThreadsBtn.Click += ShutdownBtn_Clicked;
            activeThreadsBtn.Click += ActiveThreadsBtn_Clicked;
            buttons.Add(spawnEnemyBtn);
            buttons.Add(shutdownThreadsBtn);
            buttons.Add(activeThreadsBtn);
            buttons.Add(cpuBtn);

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

            _spriteBatch = new SpriteBatch(GameWorld.thisGameWorld.GraphicsDevice);

            var playerTexture = GameWorld.gameState.playerBuilder.Player.GameObject.Tag;
            //var wallTexture = GameWorld.gameState.component.Node.GameObject.Tag;

            //var colliderTexture = "Collider"/*GameWorld.gameState.Collider.GameObject.Tag*/;

            colliders = new List<Collider>()
            {
                //new Player()
                //{

                //},
                
            };
        }

        public override void Update(GameTime gameTime)
        {
            //backgroundPos = new Vector2(GameWorld.renderTarget.Width / 2, GameWorld.renderTarget.Height / 2);
            //backgroundOrigin = new Vector2(backgroundSprite.Width / 2, backgroundSprite.Height / 2);

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
                SpawnEnemies();


            }

            if (currentKeyState.IsKeyDown(Keys.RightShift) && !previousKeyState.IsKeyDown(Keys.RightShift))
            {
                Mods mods = new Mods();
                mods.Create();

            }

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


            foreach (var item in buttons)
            {
                item.Update();
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

            //spriteBatch.Draw(backgroundSprite, backgroundPos, null, Color.White, 0, backgroundOrigin, 1f, SpriteEffects.None, 0.1f);

            // Frederik
            //foreach (var sprite in stateObjects)
            //{
            //    sprite.Draw(gameTime, spriteBatch);
            //}

            foreach (var item in buttons)
            {
                item.Draw(spriteBatch);
            }

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

            spriteBatch.DrawString(font, $"{GameWorld.gameState.playerBuilder.Player.hp} health points", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X, playerBuilder.Player.GameObject.Transform.Position.Y +20), Color.White);
            spriteBatch.DrawString(font, $"{GameWorld.gameState.playerBuilder.Player.dmg} dmg points", new Vector2(playerBuilder.Player.GameObject.Transform.Position.X , playerBuilder.Player.GameObject.Transform.Position.Y +40), Color.White);

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


        private void SpawnEnemies()
        {
            //spawnTime += delta;
            //if (spawnTime >= cooldown)
            //{
            GameObject1 go = EnemyPool.Instance.GetObject();
        
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
            if (GameWorld.gameState.cpuBuilder.Cpu.Health <= 0 || GameWorld.gameState.playerBuilder.Player.Health <= 0)
            {
                ShutdownThreads();
                GameWorld.ChangeState(GameWorld.gameOverState);
            }
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
        //protected void Animate(GameTime gametime)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
        //    {
        //        //Giver tiden, der er g�et, siden sidste update
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