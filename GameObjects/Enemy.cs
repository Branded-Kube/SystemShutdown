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
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;
using SystemShutdown.ObjectPool;
using SystemShutdown.ObserverPattern;
using SystemShutdown.States;

namespace SystemShutdown.GameObjects
{
    //Ras
    public class Enemy : Component, IGameListener
    {
        Thread internalThread;
        string data;
        private bool attackingPlayer = false;
        private bool attackingCPU = false;
        private bool enableAstar = true;

        private Vector2 distance;

        double updateTimer = 0.0;
        private SpriteRenderer spriteRenderer;
        Node node = null;
                    Node current = null;

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

        public int dmg { get; set; }
        public int id { get; set; }
        private string name = "Enemy";
        public event EventHandler ClickSelect;
        private Random randomNumber;
        private float vision;

        private Texture2D sprite;
        private Rectangle rectangle;

        bool playerTarget = false;

        public Vector2 nextpos;
        // Astar 
        Vector2 velocity;
        private double updateTimerA = 0.0;
        private double updateTimerB = 0.0;


        private bool Searching = false;

        Stack<Node> path = new Stack<Node>();
        public Node goal;

        Astar aStar;
        GameObject1 go;
        //
        


        private bool threadRunning = true;

        public bool ThreadRunning
        {
            get { return threadRunning; }
            set { threadRunning = value; }
        }

        public Enemy()
        {
            this.vision = 500;
            internalThread = new Thread(ThreadMethod);
            LoadContent(GameWorld.content);
           //Health = 100;
            dmg = 5;
            //    randomNumber = new Random();
            //    positionX = 300 + randomNumber.Next(0, 150);
            //    positionY = 700 + randomNumber.Next(0, 150);
            //    x = new Point(positionX, positionY);
            //    y = new Point(24, 48);
            //    this.rectangle = new Rectangle(x, y);
            //go = new GameObject1();
            //go.AddComponent(goal);
        }


        public override void Destroy()
        {
            EnemyPool.Instance.RealeaseObject(GameObject);
            threadRunning = false;
        }
       

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                Random rnd = new Random();

                var moddrop = rnd.Next(1, 3);
                if (moddrop == 2)
                {
                    ModFactory.Instance.Create(GameObject.Transform.Position);
                }
                GameObject.Destroy();
            }

            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (updateTimer >= 1.0)
            {
                if (IsPlayerInRange(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position))
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





            //if (cycle == day)
            //{

            //}
            if (playerTarget)
            {
                goal = GameWorld.gameState.grid.Node((int)Math.Round(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int) Math.Round(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);

            }
            else
            {
                goal = GameWorld.gameState.grid.Node((int)Math.Round(GameWorld.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
            }




            Node start = null;
            start = GameWorld.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.gameState.NodeSize, (int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.gameState.NodeSize);

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


            while (path.Count > 0)
            {
                path.Pop();

            }

            GameWorld.gameState.grid.ResetState();
            Searching = true;

            //  }
            // updateTimerB = 0.0;
            //}

            // }
            // use update timer to slow down animation
            //updateTimerA += gameTime.ElapsedGameTime.TotalSeconds;

            //if (updateTimerA >= 0.8)
            //{

                // begin the search to goal from enemy's position
                // search function pushs path onto the stack
                if (Searching )
                {

                    current = GameWorld.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.gameState.NodeSize, (int) Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.gameState.NodeSize);
                //current.alreadyOccupied = true;(76d / 100d, 0) * 100
                //if (current.cameFrom != null)
                //{
                //    current.cameFrom.alreadyOccupied = false;

                //}


                aStar.Search(GameWorld.gameState.grid, current, goal, path);
                // Debug.WriteLine($"path {path.Count}");
                //Debug.WriteLine($"path add {path.Count}");

                Searching = false;
                if (path.Count >0)
                {
                    //if ( node == null || current.x == node.x && current.y == node.y )
                    //{
                        //Debug.WriteLine($"Path Pop");

                        node = path.Pop();
                        int x = node.x * GameWorld.gameState.NodeSize;
                        int y = node.y * GameWorld.gameState.NodeSize;
                        //  node.alreadyOccupied = true;
                        // node.cameFrom.alreadyOccupied = false;
                        nextpos = new Vector2(x, y);
                      //  Debug.WriteLine($"pos{GameObject.Transform.Position}");
                    //     Debug.WriteLine($"current node {current.x} {current.y}");
                    //Debug.WriteLine($"next node {node.x} {node.y}");


                    //Debug.WriteLine($"path {path.Count}");
                    //}
                    //else
                    //{
                        Move(nextpos);

                   // }

                    //if (Math.Round(GameObject.Transform.Position.X) != nextpos.X && Math.Round(GameObject.Transform.Position.Y) != nextpos.Y || nextpos != Vector2.Zero)
                    //{

                    //}
                }
              
            }

        }

        //}


        //updateTimerA = 0.0;



        public void RotateEnemy()
        {
           // distance = GameObject.Transform.Position - new Vector2(goal.x*100, goal.y *100);
            distance = GameObject.Transform.Position - nextpos;

            var tmpSR = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            tmpSR.Rotation = (float)Math.Atan2(distance.Y, distance.X);
        }

        public bool IsPlayerInRange(Vector2 target)
        {
            return vision >= Vector2.Distance(GameObject.Transform.Position, target);
        }

        public void LoadContent(ContentManager content)
        {
            aStar = new Astar();
            goal = GameWorld.gameState.grid.Node(1, 1);
        }

        public void Move(Vector2 nextpos)
        {
            var speed = 200;

            velocity = nextpos - GameObject.Transform.Position;

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            //Debug.WriteLine($"pos{GameObject.Transform.Position}");
            //Debug.WriteLine($"path {velocity * GameWorld.DeltaTime}");
            
                GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);


            RotateEnemy();
           


            // GameObject.Transform.Position = nextpos;


        }




        /// <summary>
        /// Sets Thread id
        /// If any of the 3 bools are true, worker enters corresponding building (Volcano/PalmTree/MainBuilding)
        /// </summary>
        private void ThreadMethod()
        {
            this.id = Thread.CurrentThread.ManagedThreadId;

            while (GameWorld.gameState.running == true)
            {
                if (attackingPlayer && threadRunning)
                {
                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(500);

                    Debug.WriteLine($"{data}{id} Trying to enter Player");

                    GameWorld.gameState.playerBuilder.Player.Enter(internalThread);

                    attackingPlayer = false;
                    attackingCPU = false;
                    //delivering = true;

                    GameWorld.gameState.playerBuilder.Player.hp -= dmg / 2;

                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));

                }
                else if (attackingCPU && threadRunning)
                {
                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(1000);

                    Debug.WriteLine($"{data}{id} Trying to enter CPU");

                    CPU.Enter(internalThread);

                    attackingPlayer = false;
                    attackingCPU = false;
                    //delivering = true;

                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));

                    //    CPU.CPUTakingDamage(internalThread);

                    GameWorld.gameState.cpuBuilder.Cpu.Health -= dmg;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void StartThread()
        {
            internalThread.IsBackground = true;
            if (!internalThread.IsAlive)
            {
                internalThread.Start();

            }
            threadRunning = true;
        }
        public override void Awake()
        {
            GameObject.Tag = "Enemy";
            Health = 100;
            //GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
            // this.position = GameObject.Transform.Position;
            //spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            StartThread();
        }
        public override string ToString()
        {
            return "Enemy";
        }
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {
               //attackingPlayer = true;
            }

            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "CPU")
            {
               // attackingCPU = true;
            }
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Projectile")
            {
                Debug.WriteLine($"{Health}");
                Health -= GameWorld.gameState.playerBuilder.player.dmg;
               
            }
        }
    }
}
