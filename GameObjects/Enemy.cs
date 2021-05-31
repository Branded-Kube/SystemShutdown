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
        float speed = 200;

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
        private Node goal;

        Astar aStar;
        GameObject1 go;
        //
        bool goalFound = false;


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
            dmg = 5;
        }


        public override void Destroy()
        {
            EnemyPool.Instance.RealeaseObject(GameObject);
            threadRunning = false;
        }
       
        public void FindGoal()
        {
            if (playerTarget)
            {
                goal = GameWorld.gameState.grid.Node((int)Math.Round(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
                speed = 200;
            }
            else if (!GameWorld.isDay)
            {
                goal = GameWorld.gameState.grid.Node((int)Math.Round(GameWorld.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
                speed = 200;
            }
            else
            {
                speed = 100;

                goal = null;

                var maxvalue = new Vector2(((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) + 5) , ((int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) + 5) );
                var minvalue = new Vector2(((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) - 5), ((int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) - 5) );

                var tmpvector = SetEnemyPosition(minvalue, maxvalue);

                goal = GameWorld.gameState.grid.Node((int)tmpvector.X / 100, (int)tmpvector.Y / 100);


            }
            goalFound = true;
        }
        

        private Node GetRandomPassableNode(Vector2 minLimit, Vector2 maxLimit)
        {
            Random rndd = new Random();
            Node tmppos = null;
            while (tmppos == null || tmppos.x < 0 && tmppos.x > GameWorld.gameState.grid.Width -2 && tmppos.y < 0 && tmppos.y > GameWorld.gameState.grid.Height - 2)
            {

                tmppos = GameWorld.gameState.grid.Node(rndd.Next((int)minLimit.X, (int)maxLimit.X), rndd.Next((int)minLimit.Y, (int)maxLimit.Y));

            }

            return tmppos;
        }


        public Vector2 SetEnemyPosition(Vector2 minLimit, Vector2 maxLimit)
        {

            Node enemypos = null;
            while (enemypos == null || !enemypos.Passable)
            {


                enemypos = GetRandomPassableNode(minLimit, maxLimit);

            }

            return new Vector2(enemypos.x *100 , enemypos.y *100);
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
                    goalFound = false;
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

            if (!goalFound)
            {
                FindGoal();
            }



            if (goal != null)
            {
            
                Node start = null;
                start = GameWorld.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.gameState.NodeSize, (int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.gameState.NodeSize);
              
                aStar.Start(start);


                while (path.Count > 0)
                {
                    path.Pop();

                }

                GameWorld.gameState.grid.ResetState();
                Searching = true;

               
                //updateTimerA += gameTime.ElapsedGameTime.TotalSeconds;

                //if (updateTimerA >= 0.8)
                //{

                // begin the search to goal from enemy's position
                // search function pushs path onto the stack
                if (Searching)
                {

                    current = GameWorld.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.gameState.NodeSize, (int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.gameState.NodeSize);
                    //current.alreadyOccupied = true;
                    //if (current.cameFrom != null)
                    //{
                    //    current.cameFrom.alreadyOccupied = false;

                    //}
                   
                    aStar.Search(GameWorld.gameState.grid, current, goal, path);
                    // Debug.WriteLine($"path {path.Count}");
                    //Debug.WriteLine($"path add {path.Count}");

                    Searching = false;
                    if (path.Count > 0)
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
                        //Debug.WriteLine($"pos{GameObject.Transform.Position}");
                        //Debug.WriteLine($"current node {current.x} {current.y}");
                        //Debug.WriteLine($"next node {node.x} {node.y}");

                        Move(nextpos);
                    }
                    else
                    {
                        goalFound = false;
                    }

                }
            }


        }

        //}


        //updateTimerA = 0.0;



        public void RotateEnemy()
        {
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
           // goal = GameWorld.gameState.grid.Node(1, 1);
        }

        public void Move(Vector2 nextpos)
        {

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
            
            GameObject.Transform.Position = SetEnemyPosition(new Vector2( 1, 1), new Vector2(GameWorld.gameState.grid.Width -2, GameWorld.gameState.grid.Height -2));

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
               //Debug.WriteLine($"{Health}");
              // Health -= GameWorld.gameState.playerBuilder.player.dmg;
               
            }
        }
    }
}
