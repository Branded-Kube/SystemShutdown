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
using SystemShutdown.GameObjects;
using SystemShutdown.ObjectPool;

namespace SystemShutdown.States
{
    public class GameState : State
    {
        #region Fields

        public static SpriteFont font;
        private List<Enemy> enemies;
        private List<Enemy> delEnemies;
        private List<Button2> buttons;
        public static bool running = true;
        private Button2 spawnEnemyBtn;
        private Button2 cpuBtn;
        private Button2 activeThreadsBtn;
        private Button2 shutdownThreadsBtn;
        private CPU cpu;
        private string enemyID = "";
        private Texture2D cpuTexture;
        private Texture2D standardBtn;        

        private List<Player1> players;

        //private List<GameObject> gameObjects;
        private List<MenuObject> menuObjects/* = new List<GameObject>()*/;
        private List<GameObject1> gameObjects = new List<GameObject1>();

        private List<Component> playerObjects;

        //public List<Collider> Colliders { get; set; } = new List<Collider>();

        public int playerCount = 1;

        //public Player1 player1Test;

        //private Player1 player2Test;

        private InputHandler inputHandler;

        public PlayerBuilder playerBuilder;

       // private Director director;

        //public Director Director
        //{
        //    get { return director; }
        //    set { director = value; }
        //}

        //// Astar 
        Texture2D rectTexture;



        public Grid grid;

        public int NodeSize = Grid.NodeSize;



        public List<Collider> Colliders { get; set; } = new List<Collider>();


        Astar aStar;
        //EnemyAstar enemyA;
        //



        //public Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

        public Vector2 position;
        public Rectangle rectangle;
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
        protected Vector2 velocity;


        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;


       // private Camera camera;


        //public Player1 Player1Test
        //{
        //    get { return player1Test; }
        //    set { player1Test = value; }
        //}

        //public Player1 Player2Test
        //{
        //    get { return player2Test; }
        //    set { player2Test = value; }
        //}

        //private static GameState instance;
        //public static GameState Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new GameState();
        //        }
        //        return instance;
        //    }
        //}

        #endregion

        #region Methods

        #region Constructor
        public GameState()
        {
            enemies = new List<Enemy>();
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
            ////director = new Director(playerBuilder);
            //gameObjects.Add(director.Contruct());



        }
        #endregion

        public override void LoadContent()
        {

            Director director = new Director(playerBuilder);
            gameObjects.Add(director.Contruct());


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

            //spawnEnemyBtn.Click += SpawnEnemyBtn_Clicked;
            shutdownThreadsBtn.Click += ShutdownBtn_Clicked;
            activeThreadsBtn.Click += ActiveThreadsBtn_Clicked;
            buttons.Add(spawnEnemyBtn);
            buttons.Add(shutdownThreadsBtn);
            buttons.Add(activeThreadsBtn);
            buttons.Add(cpuBtn);

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
            //var playerTexture = _content.Load<Texture2D>("Textures/pl1");
            inputHandler = new InputHandler();

            font = content.Load<SpriteFont>("Fonts/font");

            menuObjects = new List<MenuObject>()
            {
                new MenuObject()
                {
                   // sprite = content.Load<Texture2D>("Backgrounds/game"),
                    //sprite = content.Load<Texture2D>(""),

                    //Layer = 0.0f,
                    //position = new Vector2(GameWorld.renderTarget.Width / 2, GameWorld.renderTarget.Height / 2),
                    position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
                }
            };

            //playerObjects = new List<Component>()
            //{
            //player1Test = new Player1();
            //};
            
            //{
            //    //sprite = content.Load<Texture2D>("Textures/pl1"),
            //    //Colour = Color.Blue,
            //   // position = new Vector2(GameWorld.renderTarget.Width / 2 /*- (player1Test.sprite.Width / 2 + 200)*/, GameWorld.renderTarget.Height / 2/* - (player1Test.sprite.Height / 2)*/),
            //    position = new Vector2(105,205),

            //    //position = new Vector2(GameWorld.ScreenWidth/ 2 /*- (player1Test.sprite.Width / 2 + 200)*/, GameWorld.ScreenHeight / 2/* - (player1Test.sprite.Height / 2)*/),
            //    //Layer = 0.3f,
            //    //Health = 10,
            //};

            ////player1Test.LoadContent(content);

            //player2Test = new Player()
            //{
            //    sprite = content.Load<Texture2D>("Textures/pl1"),
            //    //Colour = Color.Green,
            //    //position = new Vector2(GameWorld.renderTarget.Width / 2 /*- (player2Test.sprite.Width / 2 - 200)*/, GameWorld.renderTarget.Height / 2/* - (player2Test.sprite.Height / 2)*/),
            //    position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
            //    //Layer = 0.4f,
            //    //Health = 10,
            //};

            // Frederik
            //if (playerCount >= 1)
            //{
            //    playerObjects.Add(player1Test);
 
            //}

            //// Frederik
            //if (playerCount >= 2)
            //{
            //    playerObjects.Add(player2Test);
            //}

            //players = playerObjects.Where(c => c is Player1).Select(c => (Player1)c).ToList();


            // astar
            MouseState PrevMS = Mouse.GetState();

            grid = new Grid();

            // set up a white texture
            rectTexture = new Texture2D(GameWorld.graphics.GraphicsDevice, NodeSize, NodeSize);
            Color[] data = new Color[NodeSize * NodeSize];

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            rectTexture.SetData(data);

            //aStar = new Astar();

            //goal = grid.Node(1, 1);


            //enemyA = new EnemyAstar(new Rectangle(new Point(100, 100), new Point(NodeSize, NodeSize)));
            //enemyA.LoadContent(content);
            //
        }

        public override void Update(GameTime gameTime)
        {
            previousKeyState = currentKeyState;

            currentKeyState = Keyboard.GetState();
            // Frederik
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                ShutdownThreads();
                GameWorld.ChangeState(new MenuState());
            }
            if (currentKeyState.IsKeyDown(Keys.P) && !previousKeyState.IsKeyDown(Keys.P))
            {
                //SpawnEnemy();
                SpawnEnemies();


            }

            //inputHandler.Execute(player1Test);
            //inputHandler.Execute();


            // InputHandler.Instance.Execute();

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


            //inputHandler.Execute(player1Test);
            /////


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


            foreach (var sprite in gameObjects)
            {
                sprite.Update(gameTime);
            }

            foreach (var item in buttons)
            {
                item.Update();
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
                   enemy.Update(gameTime);
                }
            }


        }

        public override void PostUpdate(GameTime gameTime)
        {
            //// When sprites collide = attacks colliding with enemy (killing them) (unload game-specific content)

            //// If player is dead, show game over screen
            //// Frederik
            //if (players.All(c => c.IsDead))
            //{
            //    //highscores can also be added here (to be shown in the game over screen)

            //    _game.ChangeState(new GameOverState(_game, content));
            //}
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.Begin();

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Draw(gameTime, spriteBatch);
            //}


            //spriteBatch.Begin(transformMatrix: camera.Transform);

            // Frederik
            //float x = 10f;
            //foreach (var player in players)
            //{
            //    spriteBatch.DrawString(font, "Player: ", /*+ player name,*/ new Vector2(x, 10f), Color.White);
            //    spriteBatch.DrawString(font, "Health: ", /*+ health,*/ new Vector2(x, 30f), Color.White);
            //    spriteBatch.DrawString(font, "Score: ", /*+ score,*/ new Vector2(x, 50f), Color.White);

            //    x += 150;
            //}

            foreach (var item in buttons)
            {
                item.Draw(spriteBatch);
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
               // Frederik
            foreach (var sprite in menuObjects)
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }

            Vector2 gridPosition = new Vector2(0, 0);
            Vector2 pos = gridPosition;
            int margin = 0;

            for (int j = 0; j < grid.Height; j++)
            {
                pos.Y = j * (NodeSize + margin) + gridPosition.Y;
                for (int i = 0; i < grid.Width; i++)
                {
                   // grid.Node(i, j).rectangle(new Point(i * 100, j * 100));

                    pos.X = i * (NodeSize + margin) + gridPosition.X;
                    //grid.Node(i, j).rectangle((int)pos.X, (int)pos.Y, rectTexture.Width, rectTexture.Height);

                    if (grid.Node(i, j).Passable)
                    {
                        //if (goal.x == i && goal.y == j)
                        //{
                        //    //spriteBatch.Draw(rectTexture, pos, Color.Blue);
                        //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.Blue);

                        //}
                        //else if (grid.Node(i, j).Path)
                        //{
                        //    //spriteBatch.Draw(rectTexture, pos, Color.LightBlue);
                        //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.LightBlue);

                        //}
                        //else if (grid.Node(i, j).Open)
                        //{
                        //    //spriteBatch.Draw(rectTexture, pos, Color.LightCoral);
                        //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.LightCoral);

                        //}
                        //else if (grid.Node(i, j).Closed)
                        //{
                        //    //spriteBatch.Draw(rectTexture, pos, Color.RosyBrown);
                        //    spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.RosyBrown);

                        //}
                        //else
                        //{
                        //    //spriteBatch.Draw(rectTexture, pos, Color.White);
                        //spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.White);

                        //}
                    }
                    else
                    {

                       // spriteBatch.Draw(rectTexture, pos, Color.Gray);
                       //spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.Gray);


                    }
                    //if (grid.Node(i, j).nodeOccupiedByEnemy)
                    //{
                    //    grid.Node(i, j).Passable = false;
                    //}
                    //else if (true)
                    //{
                    //    grid.Node(i, j).Passable = true;

                    //}
                    
                }
            }



           spriteBatch.End();

            


        }


        //private void SpawnEnemy()
        //{
        //    running = true;
        //    Enemy enemy =  new Enemy();
        //    //enemy = new Enemy($"Enemy ");

        //    //enemy.Start();
        //    enemy.StartThread();
        //  // enemy.ClickSelect += Enemy_ClickSelect;
        //    enemies.Add(enemy);
        //    delEnemies.Add(enemy);
        //}
        private void SpawnEnemies()
        {
            //spawnTime += delta;
            //if (spawnTime >= cooldown)
            //{
            Random rnd = new Random(0);
                GameObject1 go = EnemyPool.Instance.GetObject();
                go.Transform.Position = new Vector2(rnd.Next(0, GameWorld.ScreenWidth), 0);
            GameState.running = true;


            AddGameObject(go);
               // spawnTime = 0;
            //}
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


        /// <summary>
        /// Adds an enemy when button is clicked, and also adds enemy to the other list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void SpawnEnemyBtn_Clicked(object sender, EventArgs e)
        //{
        //    running = true;

        //    enemy = new Enemy($"Enemy ");
        //    enemy.Start();
        //    enemy.ClickSelect += Enemy_ClickSelect;
        //    enemies.Add(enemy);
        //    delEnemies.Add(enemy);
        //}

        /// <summary>
        /// Enables clicking on the CPU, and sets enemy ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Enemy_ClickSelect(object sender, EventArgs e)
        //{
        //   // cpuBtn.Click += CPU_Clicked;

        //    enemy = (Enemy)sender;
        //    int ID = enemy.id;
        //    Debug.WriteLine(ID);
        //    enemyID = ID.ToString();
        //}

        ///// <summary>
        ///// Toggles bool on latest clicked enemy and removes click events on CPU
        ///// Enemy thread enters CPU 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void CPU_Clicked(object sender, EventArgs e)
        //{
        //    enemy.AttackingPlayer = true;

        //    cpuBtn.Click -= CPU_Clicked;
        //}

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


        //public void AddGameObject(GameObject go)
        //{
        //    go.Awake();
        //    go.Start();
        //    gameObjects.Add(go);
        //    Collider collider = (Collider)go.GetComponent("Collider");
        //    if (collider != null)
        //    {
        //        Colliders.Add(collider);
        //    }
        //}

        //public void RemoveGameObject(GameObject go)
        //{
        //    gameObjects.Remove(go);
        //}
        //protected void Animate(GameTime gametime)
        //{
            //if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
            //{
            //    //Giver tiden, der er gået, siden sidste update
            //    timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            //    //Beregner currentIndex
            //    currentIndex = (int)(timeElapsed * fps);
            //    sprite = upWalk[currentIndex];

            //    //Checks if animation needs to restart
            //    if (currentIndex >= upWalk.Length - 1)
            //    {
            //        //Resets animation
            //        timeElapsed = 0;
            //        currentIndex = 0;
            //    }
            //}
        //}
        #endregion
    }
}