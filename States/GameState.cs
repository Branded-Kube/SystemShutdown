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

namespace SystemShutdown.States
{
    public class GameState : State
    {
        #region Fields

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

        private List<Player1> players;

        //private List<GameObject> gameObjects;
        private List<MenuObject> menuObjects/* = new List<GameObject>()*/;
        private List<GameObject1> gameObjects = new List<GameObject1>();

        private List<Component> playerObjects;

        //public List<Collider> Colliders { get; set; } = new List<Collider>();

        public int playerCount = 1;

        private Player1 player1Test;

        private Player1 player2Test;

        private InputHandler inputHandler;


        // Astar 
        Texture2D rectTexture;

        double updateTimer = 0.0;

        bool Searching = false;


        public Grid grid;

        Stack<Node> path = new Stack<Node>();
        Node goal;

        MouseState PrevMS;

        Astar aStar;
        EnemyAstar enemyA;
        //
       int NodeSize = Grid.NodeSize;



        public Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        private SpriteRenderer spriteRenderer;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

        public Vector2 position;
        public Rectangle rectangle;
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
        protected Vector2 velocity;



        public Player1 Player1Test
        {
            get { return player1Test; }
            set { player1Test = value; }
        }

        public Player1 Player2Test
        {
            get { return player2Test; }
            set { player2Test = value; }
        }


        #endregion

        #region Methods

        #region Constructor
        public GameState(GameWorld game, ContentManager content) : base(game, content)
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
            Director director = new Director(new PlayerBuilder());
            gameObjects.Add(director.Contruct());

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }
        }
        #endregion

        public override void LoadContent()
        {
            //Frederik
            font = content.Load<SpriteFont>("Fonts/font");
            standardBtn = content.Load<Texture2D>("Controls/button");
            cpuTexture = content.Load<Texture2D>("Textures/box");
            shutdownThreadsBtn = new Button2(800, 840, "Shutdown Threads", standardBtn);
            activeThreadsBtn = new Button2(1000, 840, "Thread info", standardBtn);
            spawnEnemyBtn = new Button2(150, 10, "Spawn Enemy", standardBtn);
            cpuBtn = new Button2(700, 700, "CPU", cpuTexture);

            spawnEnemyBtn.Click += SpawnEnemyBtn_Clicked;
            shutdownThreadsBtn.Click += ShutdownBtn_Clicked;
            activeThreadsBtn.Click += ActiveThreadsBtn_Clicked;
            buttons.Add(spawnEnemyBtn);
            buttons.Add(shutdownThreadsBtn);
            buttons.Add(activeThreadsBtn);
            buttons.Add(cpuBtn);

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
                    sprite = content.Load<Texture2D>("Backgrounds/game"),
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
            rectTexture = new Texture2D(GameWorld.graphics.GraphicsDevice,  NodeSize, NodeSize);
            Color[] data = new Color[NodeSize * NodeSize];

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            rectTexture.SetData(data);

            aStar = new Astar();

            goal = grid.Node(1, 1);


            enemyA = new EnemyAstar(new Rectangle(new Point(100, 100), new Point(NodeSize, NodeSize)));
            enemyA.LoadContent(content);
            //
        }

        public override void Update(GameTime gameTime)
        {
            //Animate(gametime: gameTime);
            // Frederik
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                ShutdownThreads();
                _game.ChangeState(new MenuState(_game, content));
            }

            InputHandler.Instance.Execute();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
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
                    enemy.Update();
                }
            }


            // Astar
            MouseState ms = Mouse.GetState();
            // on left click set a new goal and restart search from current player position
            if (ms.LeftButton == ButtonState.Pressed && !Searching && PrevMS.LeftButton == ButtonState.Released)
            {
                int mx = ms.X;
                int my = ms.Y;

                // mouse coords to grid index
                int x = mx / NodeSize; 
                int y = my / NodeSize;


                goal = grid.Node((int)player1Test.position.X / 100, (int)player1Test.position.Y / 100);

                //goal = grid.Node(x, y);

                Node start = null;
                start = grid.Node(enemyA.position.X / NodeSize, enemyA.position.Y / NodeSize);

                // if clicked on non passable node, then march in direction of player till passable found
                while (!goal.Passable)
                {
                    int di = start.x - goal.x;
                    int dj = start.y - goal.y;

                    int di2 = di * di;
                    int dj2 = dj * dj;

                    int ni = (int)Math.Round(di / Math.Sqrt(di2 + dj2));
                    int nj = (int)Math.Round(dj / Math.Sqrt(di2 + dj2));

                    goal = grid.Node(goal.x + ni, goal.y + nj);
                }


                aStar.Start(start);

                Searching = true;

                while (path.Count > 0) path.Pop();
                grid.ResetState();
            }

            // use update timer to slow down animation
            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (updateTimer >= 0.1)
            {

                // begin the search to goal from player's position
                // search function pushs path onto the stack
                if (Searching)
                {
                    Node current = null;
                    current = grid.Node(enemyA.position.X / NodeSize, enemyA.position.Y / NodeSize);

                    aStar.Search(grid, current, goal, path);

                    Searching = false;
                }
                if (path.Count > 0)
                {
                    Node node = path.Pop();
                    int x = node.x * NodeSize;
                    int y = node.y * NodeSize;
                    enemyA.Move(x, y);
                }
                updateTimer = 0.0;
            }

            PrevMS = ms;

            //
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

            // astar

            //GraphicsDevice.Clear(Color.Black);

            Vector2 gridPosition = new Vector2(0, 0);
            Vector2 pos = gridPosition;
            int margin = 0;

            for (int j = 0; j < grid.Height; j++)
            {
                pos.Y = j * (NodeSize + margin) + gridPosition.Y;
                for (int i = 0; i < grid.Width; i++)
                {
                    grid.Node(i, j).rectangle(new Point(i*100, j*100));

                    pos.X = i * (NodeSize + margin) + gridPosition.X;
                     //grid.Node(i, j).rectangle((int)pos.X, (int)pos.Y, rectTexture.Width, rectTexture.Height);

                    if (grid.Node(i, j).Passable)
                    {
                        if (goal.x == i && goal.y == j)
                        {
                            //spriteBatch.Draw(rectTexture, pos, Color.Blue);
                            spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.Blue);

                        }
                        else if (grid.Node(i, j).Path)
                        {
                            //spriteBatch.Draw(rectTexture, pos, Color.LightBlue);
                            spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.LightBlue);

                        }
                        else if (grid.Node(i, j).Open)
                        {
                            //spriteBatch.Draw(rectTexture, pos, Color.LightCoral);
                            spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.LightCoral);

                        }
                        else if (grid.Node(i, j).Closed)
                        {
                            //spriteBatch.Draw(rectTexture, pos, Color.RosyBrown);
                            spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.RosyBrown);

                        }
                        else
                        {
                            //spriteBatch.Draw(rectTexture, pos, Color.White);
                           // spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.White);

                        }
                    }
                    else
                    {

                        //spriteBatch.Draw(rectTexture, pos, Color.Gray);
                        spriteBatch.Draw(rectTexture, grid.Node(i, j).collisionRectangle, Color.Gray);


                    }
                }
            }

            enemyA.Draw(spriteBatch);
            //

          
            

            spriteBatch.End();

            


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
        protected void Animate(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //Giver tiden, der er gået, siden sidste update
                timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                //Beregner currentIndex
                currentIndex = (int)(timeElapsed * fps);
                spriteRenderer.Sprite = upWalk[currentIndex];

                //Checks if animation needs to restart
                if (currentIndex >= upWalk.Length - 1)
                {
                    //Resets animation
                    timeElapsed = 0;
                    currentIndex = 0;
                }
            }
        }
        #endregion
    }
}