using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Threading;
using SystemShutdown.ComponentPattern;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
    // Lead author: Søren
    public class CPU : Component, IGameListener
    {
        #region Fields
        public delegate void DamageEventHandler(object source, Enemy enemy, EventArgs e);
        public static event DamageEventHandler TakeDamageCPU;
        static Semaphore MySemaphore;
        private float animateTimer = 5f;
        private float countDown = 0.05f;
        private bool AnimateTimer { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Releases Semaphore (how many that may enter at a time)
        /// 10 Threads can enter
        /// </summary>
        public CPU()
        {
            // Closes old semaphore and creates a new one (New gamestate bug, return to menu and resume
            if (MySemaphore != null)
            {
                MySemaphore.Close();
                MySemaphore = null;
            }
            Debug.WriteLine("CPU semaphore releases (10)");
            MySemaphore = new Semaphore(0, 10);
            MySemaphore.Release(10);

            Health = 1000;
            TakeDamageCPU += CPU_DamageCPU;

            GameWorld.Instance.GameState.CpuBuilder.fps = 5;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Cpu health minus enemy damage
        /// </summary>
        /// <param name="source"></param>
        /// <param name="enemy"></param>
        /// <param name="e"></param>
        private void CPU_DamageCPU(object source, Enemy enemy, EventArgs e)
        {
            Health -= enemy.Dmg;
        }

        /// <summary>
        /// Tells Enemy thread to wait for empty space using semaphore, and then start damaging CPU Health
        /// </summary>
        /// <param name="id"></param>
        public static void Enter(Object id, Enemy enemy)
        {
            int tmp = Thread.CurrentThread.ManagedThreadId;

            // Debug.WriteLine($"Enemy {tmp} Waiting to enter (CPU)");
            MySemaphore.WaitOne();
            // Debug.WriteLine("Enemy " + tmp + " Starts harvesting power (CPU)");
            Random randomNumber = new Random();
            Thread.Sleep(50 * randomNumber.Next(0, 15));
            TakeDamageCPU(null, enemy, EventArgs.Empty);
            //  Debug.WriteLine("Enemy " + tmp + " is leaving (CPU)");
            MySemaphore.Release();
        }

        public override void Awake()
        {
            GameObject.Tag = "CPU";
            GameObject.Transform.Position = new Vector2(1700, 1700);
        }

        public override void Update(GameTime gameTime)
        {
            if (AnimateTimer == true)
            {
                GameWorld.Instance.GameState.CpuBuilder.Animate(gameTime);
                animateTimer -= countDown;

                if (animateTimer <= 0)
                {
                    AnimateTimer = false;
                }
            }
            else if (AnimateTimer == false)
            {
                animateTimer = 5f;
                GameWorld.Instance.GameState.CpuBuilder.sr.Sprite = GameWorld.Instance.GameState.CpuBuilder.colors[0];
            }
        }
        public override string ToString()
        {
            return "CPU";
        }

        /// <summary>
        /// Collision with Enemy Bug:
        /// Enables enemy damage bool. 
        /// Collision with Enemy Trojan:
        /// Enemy destroys itself and 3 Bug enemies are spawned in its position 
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy")
            {
                Enemy tmpEnemy = (Enemy)component.GameObject.GetComponent("Enemy");

                AnimateTimer = true;

                if (tmpEnemy.IsTrojan)
                {
                    GameWorld.Instance.horseEffect2.Play();

                    GameWorld.Instance.GameState.SpawnBugEnemies(tmpEnemy.GameObject.Transform.Position);
                    GameWorld.Instance.GameState.SpawnBugEnemies(tmpEnemy.GameObject.Transform.Position);
                    GameWorld.Instance.GameState.SpawnBugEnemies(tmpEnemy.GameObject.Transform.Position);

                    tmpEnemy.GameObject.Destroy();
                }
                else
                {

                    tmpEnemy.AttackingCPU = true;
                }
            }
        }
        #endregion
    }
}
