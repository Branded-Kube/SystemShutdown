using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using SystemShutdown.AStar;
using SystemShutdown.Components;
using SystemShutdown.ObjectPool;
using SystemShutdown.ObserverPattern;
using SystemShutdown.States;

namespace SystemShutdown.GameObjects
{
   public class Enemy : Component, IGameListener
    {
        Thread internalThread;
        string data;
        private bool attackingPlayer = false;
        private bool attackingCPU = false;
        double updateTimer = 0.0;
        private SpriteRenderer spriteRenderer;

        public bool AttackingCPU
        {
            get { return attackingCPU; }
            set { attackingCPU = value; }
        }
        public bool AttackingPlayer
        {
            get { return attackingPlayer; }
            set { attackingPlayer = value; }
        }
        public int id { get; set; }
        private string name = "Enemy";
        public event EventHandler ClickSelect;
        private Random randomNumber;
        private float vision;

        private Texture2D sprite;
        private Rectangle rectangle;

        bool playerTarget = false;


        // Astar 

        private double updateTimerA = 0.0;

        private bool Searching = false;

        Stack<Node> path = new Stack<Node>();
        Node goal;

        Astar aStar;

        //




        private bool threadRunning = true;

        public bool ThreadRunning
        {
            get { return threadRunning; }
            set { threadRunning = value; }
        }

        public Enemy(Rectangle Rectangle)
        {
            this.vision = 500;
            this.rectangle = Rectangle;
            internalThread = new Thread(ThreadMethod);
            LoadContent(GameWorld.content);




            //    randomNumber = new Random();
            //    positionX = 300 + randomNumber.Next(0, 150);
            //    positionY = 700 + randomNumber.Next(0, 150);
            //    x = new Point(positionX, positionY);
            //    y = new Point(24, 48);
            //    this.rectangle = new Rectangle(x, y);

        }
        public Enemy( )
        {
           // this.speed = speed;
            //this.velocity = velocity;
        }
        public override void Destroy()
        {
            EnemyPool.Instance.RealeaseObject(GameObject);
        }
        public override void Update(GameTime gameTime)
        {
            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (updateTimer >= 1.0)
            {
                if (IsPlayerInRange(GameWorld.gameState.playerBuilder.player.position))
                {

                    //        //  better handling of walls is needed

                    //        //foreach (var item in GameWorld.gameState.grid.nodes)
                    //        //{
                    //        //    if (!item.Passable)
                    //        //    {
                    //        //        if (item.position.X < GameWorld.gameState.Player1Test.position.X && item.position.X > rectangle.X)
                    //        //        {

                   // Debug.WriteLine("Enemy can see player!");
                    playerTarget = true;
                //        //        }
                //        //    }
                //        //}

                }
                    else
                    {
                    // Debug.WriteLine("Enemy can not see player!");
                    playerTarget = false;

                }
                updateTimer = 0.0;
            }


            // Astar

            if (playerTarget)
          {

                
                Searching = true;

                goal = GameWorld.gameState.grid.Node((int)GameWorld.gameState.playerBuilder.player.position.X / 100, (int)GameWorld.gameState.playerBuilder.player.position.Y / 100);


                Node start = null;
                start = GameWorld.gameState.grid.Node(Rectangle.X / GameWorld.gameState.NodeSize, Rectangle.Y / GameWorld.gameState.NodeSize);

                // if clicked on non passable node, then march in direction of player till passable found
                //while (!goal.Passable)
                //{
                //    int di = start.x - goal.x;
                //    int dj = start.y - goal.y;

                //    int di2 = di * di;
                //    int dj2 = dj * dj;

                //    int ni = (int)Math.Round(di / Math.Sqrt(di2 + dj2));
                //    int nj = (int)Math.Round(dj / Math.Sqrt(di2 + dj2));

                //    goal = aStar.Node(goal.x + ni, goal.y + nj);
                //}


                aStar.Start(start);


                while (path.Count > 0) path.Pop();
                GameWorld.gameState.grid.ResetState();
            }

            // use update timer to slow down animation
            updateTimerA += gameTime.ElapsedGameTime.TotalSeconds;
            if (updateTimerA >= 0.1)
            {

                // begin the search to goal from enemy's position
                // search function pushs path onto the stack
                if (Searching)
                {
                    Node current = null;
                    current = GameWorld.gameState.grid.Node(Rectangle.X / GameWorld.gameState.NodeSize, Rectangle.Y / GameWorld.gameState.NodeSize);
                    //current.alreadyOccupied = true;
                    //if (current.cameFrom != null)
                    //{
                    //    current.cameFrom.alreadyOccupied = false;

                    //}
                    aStar.Search(GameWorld.gameState.grid, current, goal, path);

                    Searching = false;
                }
                if (path.Count > 0)
                {
                    Node node = path.Pop();
                    int x = node.x * GameWorld.gameState.NodeSize;
                    int y = node.y * GameWorld.gameState.NodeSize;
                    //  node.alreadyOccupied = true;
                    // node.cameFrom.alreadyOccupied = false;

                    Move(x, y);
                }

                updateTimerA = 0.0;
            }

        }


        public bool IsPlayerInRange(Vector2 target)
        {
            Vector2 thisPos = new Vector2(rectangle.X, rectangle.Y);
            return vision >= Vector2.Distance(thisPos, target);
        }



        public Rectangle Rectangle
        {
            get { return rectangle; }

        }
        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Textures/pl1");

            // astar
            MouseState PrevMS = Mouse.GetState();


            aStar = new Astar();

            goal = GameWorld.gameState.grid.Node(1, 1);


        }

        public void Move(int x, int y)
        {
            rectangle.X = x;
            rectangle.Y = y;
            Vector2 position = new Vector2(rectangle.X,rectangle.Y);
            if (position == goal.position)
            {
                //Debug.Write("!");
                attackingPlayer = true;

            }
        }
      
        public void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(sprite, rectangle, Color.Red);
            //var x = (rectangle.X + (rectangle.Width / 2)) - (GameState.font.MeasureString(name).X / 2);
            //var y = (rectangle.Y + (rectangle.Height / 2)) - (GameState.font.MeasureString(name).Y / 2);
            //spritebatch.DrawString(GameState.font, name, new Vector2(x, y), Color.Black);
            // astar

            //GraphicsDevice.Clear(Color.Black);




        }


        /// <summary>
        /// Sets Thread id
        /// If any of the 3 bools are true, worker enters corresponding building (Volcano/PalmTree/MainBuilding)
        /// </summary>
        private void ThreadMethod()
        {
            this.id = Thread.CurrentThread.ManagedThreadId;

            while (GameState.running == true)
            {
                if (attackingPlayer == true)
                {
                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(2000);

                    Debug.WriteLine($"{data}{id} Trying to enter CPU");

                    GameWorld.gameState.playerBuilder.player.Enter(internalThread);

                    attackingPlayer = false;
                    //delivering = true;

                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));

                }
                else if (attackingCPU == true)
                {
                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(2000);

                    Debug.WriteLine($"{data}{id} Trying to enter CPU");

                    CPU.Enter(internalThread);

                    attackingPlayer = false;
                    //delivering = true;

                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }


        //public void Start()
        //{
        //    internalThread.IsBackground = true;
        //    internalThread.Start();
        //}
        public override void Awake()
        {
            GameObject.Tag = "Enemy";

            GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
            this.position = GameObject.Transform.Position;
            //spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }
        public override string ToString()
        {
            return "Enemy";
        }
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {
                throw new NotImplementedException();
            }
        }
    }
}
