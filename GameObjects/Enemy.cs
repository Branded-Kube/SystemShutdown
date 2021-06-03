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
        private bool attackingPlayer = false;
        private bool attackingCPU = false;
        private bool playerTarget = false;
        private bool isGoalFound = false;
        private bool threadRunning = true;
        private bool isTrojan = false;

        private int dmg;
        private int Id;
        private float vision;
        private float speed;
        private double updateTimer = 0.0;

        private Vector2 direction;
        private Vector2 nextpos;
        private Vector2 velocity;

        private Thread internalThread;
        private Stack<Node> path = new Stack<Node>();
        private Node goal;
        private Astar aStar;
        public int Dmg
        {
            get { return dmg; }
            set { dmg = value; }
        }
        public bool IsTrojan
        {
            get { return isTrojan; }
            set { isTrojan = value; }
        }
        public bool AttackingPlayer
        {
            get { return attackingPlayer; }
            set { attackingPlayer = value; }
        }
        public bool AttackingCPU
        {
            get { return attackingCPU; }
            set { attackingCPU = value; }
        }

        /// <summary>
        /// Removes enemy gameobject from list of gameobjects, -1 to alive enemies and +1 to player kills. Stops threads while loop with bool threadRunning
        /// </summary>
        public override void Destroy()
        {
            //EnemyPool.Instance.RealeaseObject(GameObject);
            GameWorld.Instance.gameState.aliveEnemies--;
            GameWorld.Instance.gameState.playerBuilder.player.kills++;
            GameWorld.Instance.gameState.RemoveGameObject(GameObject);
            threadRunning = false;
        }

        /// <summary>
        /// Sets enemy goal for Astar pathfinder. 
        /// If player is in vision area, else if night, else random pos + or - in each direction from current position.
        /// Also sets enemy movement speed to 100 if random target (iddle behavior)
        /// </summary>
        private void FindGoal()
        {
            if (playerTarget)
            {
                speed = 200;
                goal = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameWorld.Instance.gameState.playerBuilder.Player.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.Instance.gameState.playerBuilder.Player.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
            }
            else if (!GameWorld.Instance.isDay)
            {
                speed = 200;
                goal = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameWorld.Instance.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.X / 100d, 0) * 100 / 100, (int)Math.Round(GameWorld.Instance.gameState.cpuBuilder.Cpu.GameObject.Transform.Position.Y / 100d, 0) * 100 / 100);
            }
            else
            {
                speed = 100;
                var maxvalue = new Vector2(((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) + 5), ((int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) + 5));
                var minvalue = new Vector2(((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) - 5), ((int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) - 5));
                var tmpvector = SetRandomEnemyGoal(minvalue, maxvalue);
                goal = GameWorld.Instance.gameState.grid.Node((int)tmpvector.X / 100, (int)tmpvector.Y / 100);
            }
            isGoalFound = true;
        }
        /// <summary>
        /// Sets a random goal for enemy. - or + 5 nodes from current position in each direction
        /// </summary>
        /// <param name="minLimit"></param>
        /// <param name="maxLimit"></param>
        /// <returns></returns>
        private Vector2 SetRandomEnemyGoal(Vector2 minLimit, Vector2 maxLimit)
        {
            Random rndd = new Random();
            Node enemypos = null;
            while (enemypos == null || !enemypos.Passable)
            {
                enemypos = GameWorld.Instance.gameState.grid.Node(rndd.Next((int)minLimit.X, (int)maxLimit.X), rndd.Next((int)minLimit.Y, (int)maxLimit.Y));
            }
            return new Vector2(enemypos.x * 100, enemypos.y * 100);
        }
        /// <summary>
        /// Destroys a enemys gameobject of health is below 0. 
        /// Also have 50 % to drop a mod on current position on death
        /// </summary>
        /// <param name="health"></param>
        private void IsAlive(int health)
        {
            if (health <= 0)
            {
                Random rnd = new Random();
                var moddrop = rnd.Next(1, 3);
                if (moddrop == 2)
                {
                    ModFactory.Instance.Create(GameObject.Transform.Position, "default");
                }
                GameObject.Destroy();
            }
        }
        /// <summary>
        /// Checks each second if player is inside of vision. If true, sets bool playerTarget to true, else to false
        /// </summary>
        /// <param name="gameTime"></param>
        private void ScanForPlayer(GameTime gameTime)
        {
            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (updateTimer >= 1.0)
            {
                if (!isTrojan)
                {
                    if (IsPlayerInRange(GameWorld.Instance.gameState.playerBuilder.Player.GameObject.Transform.Position))
                    {
                        playerTarget = true;
                        isGoalFound = false;

                    }
                    else
                    {
                        playerTarget = false;
                    }
                }
                updateTimer = 0.0;
            }
        }
        /// <summary>
        /// First resets earlier values in grid and Astar, and pops all remaining nodes in path stack.
        /// Calls Astar.Search() which populates stack with a path of nodes from current position to goal position
        /// If path stack is containing nodes, pops a node and sets it to enemy`s nextpos (node to move to next)
        /// If path stack is empty, enemy is at goal and goalFound bool is sat to false
        /// </summary>
        private void AstarSeachForPath()
        {
            while (path.Count > 0)
            {
                path.Pop();
            }
            GameWorld.Instance.gameState.grid.ResetState();
            aStar.Start();
            Node currentPositionAsNode = GameWorld.Instance.gameState.grid.Node((int)Math.Round(GameObject.Transform.Position.X / 100d, 0) * 100 / GameWorld.Instance.gameState.NodeSize, (int)Math.Round(GameObject.Transform.Position.Y / 100d, 0) * 100 / GameWorld.Instance.gameState.NodeSize);
            aStar.Search(currentPositionAsNode, goal, path);
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
                isGoalFound = false;
            }
        }
        public override void Update(GameTime gameTime)
        {
            IsAlive(Health);
            ScanForPlayer(gameTime);
            if (!isGoalFound)
            {
                FindGoal();
            }
            AstarSeachForPath();
        }
        /// <summary>
        /// Rotates enemy texture in direction of nextpos 
        /// </summary>
        /// <param name="nextpos"></param>
        public void RotateEnemy(Vector2 nextpos)
        {
            direction = GameObject.Transform.Position - nextpos;
            var tmpSR = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            tmpSR.Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
        /// <summary>
        /// Returns a bool depending on if player is inside of vision range 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsPlayerInRange(Vector2 target)
        {
            return vision >= Vector2.Distance(GameObject.Transform.Position, target);
        }
        /// <summary>
        /// Moves enemy in direction of Vector2 nextpos 
        /// Normalizes velocity and runs RotateEnemy after move direction is decided. 
        /// </summary>
        /// <param name="nextpos"></param>
        public void Move(Vector2 nextpos)
        {

            velocity = nextpos - GameObject.Transform.Position;

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
            RotateEnemy(nextpos);
        }
        /// <summary>
        /// Sets Thread id
        /// If any of the 2 bools are true, enemy enters either player or CPU 
        /// Else sets thread to sleep(1000)
        /// </summary>
        private void ThreadMethod(object callback)
        {
            this.Id = Thread.CurrentThread.ManagedThreadId;
            while (threadRunning == true)
            {
                if (attackingPlayer)
                {
                    Thread.Sleep(100);
                    GameWorld.Instance.gameState.playerBuilder.Player.Enter(internalThread, this);
                    attackingPlayer = false;
                    attackingCPU = false;
                }
                else if (attackingCPU)
                {
                    Thread.Sleep(1000);
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
                        CPU.Enter(internalThread, this);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
        /// <summary>
        /// Starts enemy thread if thread is not already alive. 
        /// Sets thread to Isbackground (to close down thread when game is closing)
        /// </summary>
        public void StartThread()
        {
            if (!internalThread.IsAlive)
            {
                internalThread.Start();
                internalThread.IsBackground = true;
            }
            threadRunning = true;
        }
        /// <summary>
        /// Sets / resets enemy value on creation
        /// </summary>
        public override void Awake()
        {
            this.vision = 500;
            aStar = new Astar();
            dmg = 5;
            GameObject.Tag = "Enemy";
            isGoalFound = false;
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
            internalThread = new Thread(ThreadMethod);
            StartThread();
        }
        public override string ToString()
        {
            return "Enemy";
        }
        public void Notify(GameEvent gameEvent, Component component)
        {
        }
    }
}
