using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SystemShutdown.Sprites;

namespace SystemShutdown.States
{
    public class GameState : State
    {
        private SpriteFont font;

        private List<Player> players;

        private List<GameObject> gameObjects;

        public int playerCount;

        public GameState(GameWorld game, ContentManager content)
          : base(game, content)
        {
        }

        public override void LoadContent()
        {
            var playerTexture = _content.Load<Texture2D>("Textures/pl1");
            //var bulletTexture = _content.Load<Texture2D>("");

            font = _content.Load<SpriteFont>("Fonts/font");

            gameObjects = new List<GameObject>()
            {
                new GameObject(_content.Load<Texture2D>("Backgrounds/game"))
                {
                    Layer = 0.0f,
                    Position = new Vector2(GameWorld.renderTarget.Width / 2, GameWorld.renderTarget.Height / 2),
                    //Position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
                }
            };

            if (playerCount >= 1)
            {
                gameObjects.Add(new Player(playerTexture)
                {
                    Colour = Color.Blue,
                    Position = new Vector2(GameWorld.renderTarget.Width / 2 - (playerTexture.Width / 2 + 200), GameWorld.renderTarget.Height / 2 - (playerTexture.Height / 2)),
                    //Position = new Vector2(450, 600),
                    Layer = 0.3f,
                    //Bullet 
                    Input = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Shoot = Keys.Space,
                    },
                    Health = 10,
                });
            }

            if (playerCount >= 2)
            {
                gameObjects.Add(new Player(playerTexture)
                {
                    Colour = Color.Green,
                    Position = new Vector2(GameWorld.renderTarget.Width / 2 - (playerTexture.Width / 2 - 200), GameWorld.renderTarget.Height / 2 - (playerTexture.Height / 2)),
                    //Position = new Vector2(100, 600),
                    Layer = 0.4f,
                    //Bullet 
                    Input = new Input()
                    {
                        Up = Keys.Up,
                        Down = Keys.Down,
                        Left = Keys.Left,
                        Right = Keys.Right,
                        Shoot = Keys.Enter,
                    },
                    Health = 10,
                });
            }

            players = gameObjects.Where(c => c is Player).Select(c => (Player)c).ToList();

        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new MenuState(_game, _content));
            }

            foreach (var sprite in gameObjects)
            {
                sprite.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // When sprites collide = bullet colliding with enemy/player (unload game-specific content)

            // If player is dead, show game over screen
            if (players.All(c => c.IsDead))
            {
                //highscores can also be added here (to be shown in the game over screen)

                _game.ChangeState(new GameOverState(_game, _content));
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var sprite in gameObjects)
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin();

            float x = 10f;
            foreach (var player in players)
            {
                spriteBatch.DrawString(font, "Player: ", /*+ player name,*/ new Vector2(x, 10f), Color.White);
                spriteBatch.DrawString(font, "Health: ", /*+ health,*/ new Vector2(x, 30f), Color.White);
                spriteBatch.DrawString(font, "Score: ", /*+ score,*/ new Vector2(x, 50f), Color.White);

                x += 150;
            }
            spriteBatch.End();
        }
    }
}