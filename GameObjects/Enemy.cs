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
        public bool isTrojan = false;
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
        Node tmppos = null;

        private bool threadRunning = true;

        public bool ThreadRunning
        {
            get { return threadRunning; }
            set { threadRunning = value; }
        }

        public Enemy()
        {
            this.vision = 500;
          //internalThread = new Thread(ThreadMethod);
            LoadContent(GameWorld.Instance.content);
            dmg = 5;
        }


        public override void Destroy()
        {
            //EnemyPool.Instance.RealeaseObject(GameObject);
            GameWorld.Instance.gameState.aliveEnemies--;
            GameWorld.Instance.gameState.playerBuilder.player.kills++;
            GameWorld.Instance.gameState.RemoveGameObject(GameObject);
            threadRunning = false;
        }
       
        public void FindGoal()
        {
            if (playerTarget)
            {
                goal = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameWorld.Instance.gameState.playerBuilder.Player.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.Instance.gameState.playerBuilder.Player.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
                speed = 200;
            }
            else if (!GameWorld.Instance.isDay)
            {
                goal = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameWorld.Instance.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.Instance.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
                speed = 200;
            }
            else
            {
                speed = 100;

                goal = null;

                var maxvalue = new Vector2(((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) + 5) , ((int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) + 5) );
                var minvalue = new Vector2(((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) - 5), ((int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) - 5) );

                var tmpvector = SetRandomEnemyGoal(minvalue, maxvalue);

               // Debug.WriteLine($"{GameObject.Transform.Position.X}  ,{GameObject.Transform.Position.Y}");

                goal = GameWorld.Instance.gameState.grid.Node((int)tmpvector.X / 100, (int)tmpvector.Y / 100);
               // Debug.WriteLine($"{goal.x}  ,{goal.y}");


            }
            goalFound = true;
        }
        

        public Vector2 SetRandomEnemyGoal(Vector2 minLimit, Vector2 maxLimit)
        {
            Random rndd = new Random();

            Node enemypos = null;
            while (enemypos == null || !enemypos.Passable )
            {

                enemypos = GameWorld.Instance.gameState.grid.Node(rndd.Next((int)minLimit.X, (int)maxLimit.X), rndd.Next((int)minLimit.Y, (int)maxLimit.Y));
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
                    //ModFactory.Instance.Create(GameObject.Transform.Position, "default");
                }
                GameObject.Destroy();
            }

            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (updateTimer >= 1.0)
            {
                if (!isTrojan)
                {
                    if (IsPlayerInRange(GameWorld.Instance.gameState.playerBuilder.Player.GameObject.Transform.Position))
                    {
                        playerTarget = true;
                        goalFound = false;

                    }
                    else
                    {
                        playerTarget = false;
                    }
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
                start = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.Instance.gameState.NodeSize, (int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.Instance.gameState.NodeSize);
              
                aStar.Start(start);


                while (path.Count > 0)
                {
                    path.Pop();

                }

                GameWorld.Instance.gameState.grid.ResetState();
                Searching = true;

               
                //updateTimerA += gameTime.ElapsedGameTime.TotalSeconds;

                //if (updateTimerA >= 0.8)
                //{

                // begin the search to goal from enemy's position
                // search function pushs path onto the stack
                if (Searching)
                {

                    current = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.Instance.gameState.NodeSize, (int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.Instance.gameState.NodeSize);
                   
                    aStar.Search(GameWorld.Instance.gameState.grid, current, goal, path);
                    Searching = false;
                    if (path.Count > 0)
                    {
                  

                        node = path.Pop();
                        int x = node.x * GameWorld.Instance.gameState.NodeSize;
                        int y = node.y * GameWorld.Instance.gameState.NodeSize;
                        nextpos = new Vector2(x, y);
                      

                        Move(nextpos);
                    }
                    else
                    {
                        goalFound = false;
                    }

                }
            }
        }

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
        }

        public void Move(Vector2 nextpos)
        {

            velocity = nextpos - GameObject.Transform.Position;

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
                GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
            RotateEnemy();
          
        }

        /// <summary>
        /// Sets Thread id
        /// If any of the 3 bools are true, worker enters corresponding building (Volcano/PalmTree/MainBuilding)
        /// </summary>
        private void ThreadMethod(object callback)
        {
            this.id = Thread.CurrentThread.ManagedThreadId;

            while (threadRunning == true)
            {
                if (attackingPlayer)
                {

                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(100);

                    Debug.WriteLine($"{data}{id} Trying to enter Player");

                    GameWorld.Instance.gameState.playerBuilder.Player.Enter(internalThread);

                    attackingPlayer = false;
                    attackingCPU = false;

                    //GameWorld.gameState.playerBuilder.Player.Health -= dmg / 2;

                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));

                }
                else if (attackingCPU)
                {
                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(1000);

                    Debug.WriteLine($"{data}{id} Trying to enter CPU");


                    attackingPlayer = false;
                    attackingCPU = false;


                    Random rnd = new Random();
                    if (rnd.Next(1, 3) == 1 && GameWorld.Instance.gameState.playerBuilder.player.playersMods.Count > 0)
                    {
                       // GameWorld.gameState.playerBuilder.player.playersMods.Pop();
                        //GameWorld.gameState.playerBuilder.player.ApplyAllMods();
                    }
                    else
                    {
                        CPU.Enter(internalThread);
                    }
                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));

                }
                else
                {
                    Thread.Sleep(1000);
                }
               
            }
        }


        public void StartThread()
        {
            
            if (!internalThread.IsAlive)
            {
                internalThread.Start();
                internalThread.IsBackground = true;

            }
            threadRunning = true;
        }
        public override void Awake()
        {
            GameObject.Tag = "Enemy";
            goalFound = false;
            playerTarget = false;
            threadRunning = true;
            if (isTrojan)
            {
                Health = 300;
            }
            else
            {
                Health = 100;
            }
            ThreadPool.QueueUserWorkItem(ThreadMethod);

            // StartThread();
        }
        public override string ToString()
        {
            return "Enemy";
        }
        public void Notify(GameEvent gameEvent, Component component)
        {

            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {

                attackingPlayer = true;
            }

            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "CPU")
            {
                if (isTrojan)
                {
                    GameWorld.Instance.gameState.SpawnBugEnemies(GameObject.Transform.Position);
                    GameWorld.Instance.gameState.SpawnBugEnemies(GameObject.Transform.Position);
                    GameWorld.Instance.gameState.SpawnBugEnemies(GameObject.Transform.Position);

                    GameObject.Destroy();
                }
                else
                {

                    attackingCPU = true;
                }
            }
            //if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Projectile")
            //{
            //   //Debug.WriteLine($"{Health}");
            //  // Health -= GameWorld.gameState.playerBuilder.player.dmg;
               
            //}
        }
    }
}
