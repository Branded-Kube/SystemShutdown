using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.AStar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D rectTexture;

        int ScreenWidth = 800;
        int ScreenHeight = 800;

        double updateTimer = 0.0;

        bool Searching = false;

        int NodeSize = 20;

        Grid Grid;

        Stack<Node> path = new Stack<Node>();
        Node goal;

        KeyboardState PrevKS;
        MouseState PrevMS;

        AStar AStar;
        Enemy enemy;

        public GameWorld1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            KeyboardState PrevKS = Keyboard.GetState();
            MouseState PrevMS = Mouse.GetState();

            

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Grid = new Grid();

            // set up a white texture
            rectTexture = new Texture2D(graphics.GraphicsDevice, NodeSize, NodeSize);
            Color[] data = new Color[NodeSize * NodeSize];

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            rectTexture.SetData(data);

            AStar = new AStar();

            goal = Grid.Node(0, 0);

            
            enemy = new Enemy(new Rectangle(Point.Zero, new Point(NodeSize,NodeSize)));
            enemy.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            if (ks.IsKeyDown(Keys.Space))
            {
                Searching = true;// !Searching;
            }

            // on left click set a new goal and restart search from current player position
            if (ms.LeftButton == ButtonState.Pressed && !Searching && PrevMS.LeftButton == ButtonState.Released)
            {
                int mx = ms.X;
                int my = ms.Y;

                // mouse coords to grid index
                int i = mx / NodeSize; ;
                int j = my / NodeSize;

                goal = Grid.Node(i, j);

                Node start = null;
                start = Grid.Node(enemy.position.X / NodeSize, enemy.position.Y / NodeSize);

                // if clicked on non passable node, then march in direction of player till passable found
                while (!goal.Passable)
                {
                    int di = start.i - goal.i;
                    int dj = start.j - goal.j;

                    int di2 = di * di;
                    int dj2 = dj * dj;

                    int ni = (int)Math.Round(di / Math.Sqrt(di2 + dj2));
                    int nj = (int)Math.Round(dj / Math.Sqrt(di2 + dj2));

                    goal = Grid.Node(goal.i + ni, goal.j + nj);
                }


                AStar.Start(start);

                Searching = true;

                while (path.Count > 0) path.Pop();
                Grid.ResetState();
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
                    current = Grid.Node(enemy.position.X / NodeSize, enemy.position.Y / NodeSize);

                    AStar.Search(Grid, current, goal, path);

                    Searching = false;
                }
                if (path.Count > 0)
                {
                    Node node = path.Pop();
                    int x = node.i * NodeSize;
                    int y = node.j * NodeSize;
                    enemy.Move(x, y);
                }
                updateTimer = 0.0;
            }


            PrevKS = ks;
            PrevMS = ms;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Vector2 gridPosition = new Vector2(0, 0);
            Vector2 pos = gridPosition;
            int margin = 0;

            spriteBatch.Begin();
            for (int j = 0; j < Grid.Height; j++)
            {
                pos.Y = j * (NodeSize + margin) + gridPosition.Y;
                for (int i = 0; i < Grid.Width; i++)
                {
                    pos.X = i * (NodeSize + margin) + gridPosition.X;
                    if (Grid.Node(i, j).Passable)
                    {
                        if (goal.i == i && goal.j == j)
                        {
                            spriteBatch.Draw(rectTexture, pos, Color.Blue);
                        }
                        else if (Grid.Node(i, j).Path)
                        {
                            spriteBatch.Draw(rectTexture, pos, Color.LightBlue);
                        }
                        else if (Grid.Node(i, j).Open)
                        {
                            spriteBatch.Draw(rectTexture, pos, Color.LightCoral);
                        }
                        else if (Grid.Node(i, j).Closed)
                        {
                            spriteBatch.Draw(rectTexture, pos, Color.RosyBrown);
                        }
                        else
                        {
                            spriteBatch.Draw(rectTexture, pos, Color.White);
                        }
                    }
                }
            }

            enemy.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}