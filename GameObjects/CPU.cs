using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
   public class CPU : Component, IGameListener
    {
        public delegate void DamageEventHandler(object source,Enemy enemy, EventArgs e);
        public static event DamageEventHandler DamageCPU;
        static Semaphore MySemaphore = new Semaphore(0, 3);
        /// <summary>
        /// Releases Semaphore (how many that may enter at a time)
        /// 3 Threads can enter
        /// </summary>
        public CPU()
        {
            Debug.WriteLine("CPU semaphore releases (3)");
            MySemaphore.Release(3);

            Health = 1000;

        }

        

        /// <summary>
        /// Tells worker to wait for empty space using semaphore, and then start harvesting from the palmtree - Soeren
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
            DamageCPU(null, enemy, EventArgs.Empty);
          //  Debug.WriteLine("Enemy " + tmp + " is leaving (CPU)");
            MySemaphore.Release();
        }



        public override void Awake()
        {
            GameObject.Tag = "CPU";

            GameObject.Transform.Position = new Vector2(1700, 1700);
          //  spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }

        public override string ToString()
        {
            return "CPU";
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy")
            {
                Enemy tmpEnemy = (Enemy)component.GameObject.GetComponent("Enemy");

                if (tmpEnemy.IsTrojan)
                {
                    GameWorld.Instance.horseEffect2.Play();

                    GameWorld.Instance.gameState.SpawnBugEnemies(tmpEnemy.GameObject.Transform.Position);
                    GameWorld.Instance.gameState.SpawnBugEnemies(tmpEnemy.GameObject.Transform.Position);
                    GameWorld.Instance.gameState.SpawnBugEnemies(tmpEnemy.GameObject.Transform.Position);

                    tmpEnemy.GameObject.Destroy();
                }
                else
                {

                    tmpEnemy.AttackingCPU = true;
                }
            }
        }
    }
}
