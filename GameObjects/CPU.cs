using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace SystemShutdown.GameObjects
{
    class CPU
    {
        static Semaphore MySemaphore = new Semaphore(0, 3);
        /// <summary>
        /// Releases Semaphore (how many that may enter at a time)
        /// 3 Threads can enter
        /// </summary>
        public CPU()
        {
            Debug.WriteLine("Main Thread calls releases (3)");
            MySemaphore.Release(3);
        }

        //tells worker to wait for empty space using semaphore, and then start harvesting from the palmtree
        public static void Enter(Object id)
        {
            int tmp = Thread.CurrentThread.ManagedThreadId;

            Debug.WriteLine($"Enemy {tmp} Waiting to enter (CPU)");
            MySemaphore.WaitOne();
            Debug.WriteLine("Enemy " + tmp + " Starts harvesting power (CPU)");
            Random randomNumber = new Random();
            Thread.Sleep(100 * randomNumber.Next(0, 150));
            Debug.WriteLine("Enemy " + tmp + " is leaving (CPU)");
            MySemaphore.Release();

        }
    }
}
