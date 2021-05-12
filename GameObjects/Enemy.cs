using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using SystemShutdown.States;

namespace SystemShutdown.GameObjects
{
    class Enemy /*: GameObject*/
    {
        Thread internalThread;
        string data;
        private bool harvesting = false;
        public bool Harvesting
        {
            get { return harvesting; }
            set { harvesting = value; }
        }
        public int id { get; set; }
        private Color hoverColor = Color.Gray;
        private Color currentColor = Color.White;
        private MouseState mouseCurrent;
        private MouseState mouseLast;
        private Rectangle mouseRectangle;
        private Texture2D workersprite;
        private string name;
        private Rectangle WorkerRectangle;
        public event EventHandler ClickSelect;
        private Random randomNumber;
        private int positionX;
        private int positionY;
        private Point x;
        private Point y;

        private bool threadRunning = true;

        public bool ThreadRunning
        {
            get { return threadRunning; }
            set { threadRunning = value; }
        }
        public Enemy(string data)
        {
            internalThread = new Thread(ThreadMethod);
            randomNumber = new Random();
            positionX = 300 + randomNumber.Next(0, 150);
            positionY = 700 + randomNumber.Next(0, 150);
            x = new Point(positionX, positionY);
            y = new Point(24, 48);
            this.WorkerRectangle = new Rectangle(x, y);
            this.name = data;

            LoadContent(GameWorld.content);
        }
        private void LoadContent(ContentManager content)
        {
            workersprite = content.Load<Texture2D>("Textures/worker");
        }

        public void Update()
        {
            mouseLast = mouseCurrent;
            mouseCurrent = Mouse.GetState();
            mouseRectangle = new Rectangle(mouseCurrent.X, mouseCurrent.Y, 1, 1);
            if (mouseRectangle.Intersects(WorkerRectangle))
            {
                this.currentColor = hoverColor;
                if (mouseLast.LeftButton == ButtonState.Pressed && mouseCurrent.LeftButton == ButtonState.Released)
                {
                    ClickSelect?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                this.currentColor = Color.White;
            }
        }

        /// <summary>
        /// Ras - Draws button and its description text in middle of button
        /// </summary>
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(workersprite, WorkerRectangle, currentColor);
            var x = (WorkerRectangle.X + (WorkerRectangle.Width / 2)) - (GameState.font.MeasureString(name).X / 2);
            var y = (WorkerRectangle.Y + (WorkerRectangle.Height / 2)) - (GameState.font.MeasureString(name).Y / 2);
            _spriteBatch.DrawString(GameState.font, name, new Vector2(x, y), Color.Black);
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
                if (harvesting == true)
                {
                    Debug.WriteLine($"{data}{id} is Running;");
                    Thread.Sleep(2000);

                    Debug.WriteLine($"{data}{id} Trying to enter CPU");

                    CPU.Enter(internalThread);

                    harvesting = false;
                    //delivering = true;

                    Debug.WriteLine(string.Format($"{data}{id} shutdown"));

                }
            }
        }
        public void Start()
        {
            internalThread.IsBackground = true;
            internalThread.Start();
        }
    }
}
