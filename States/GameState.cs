﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SystemShutdown.GameObjects;

namespace SystemShutdown.States
{
    public class GameState : State
    {
        #region Fields
        private SpriteFont font;

        private List<Player> players;

        private List<GameObject> gameObjects;

        public int playerCount;

        private Player player1Test;

        private Player player2Test;

        public Player Player1Test
        {
            get { return player1Test; }
            set { player1Test = value; }
        }

        public Player Player2Test
        {
            get { return player2Test; }
            set { player2Test = value; }
        }


        #endregion

        #region Methods

        #region Constructor
        public GameState(GameWorld game, ContentManager content) : base(game, content)
        {

        }
        #endregion

        public override void LoadContent()
        {
            // Frederik
            var playerTexture = _content.Load<Texture2D>("Textures/pl1");

            font = _content.Load<SpriteFont>("Fonts/font");

            gameObjects = new List<GameObject>()
            {
                new GameObject(_content.Load<Texture2D>("Backgrounds/game"))
                {
                    Layer = 0.0f,
                    position = new Vector2(GameWorld.renderTarget.Width / 2, GameWorld.renderTarget.Height / 2),
                }
            };

            player1Test = new Player(playerTexture)
            {
                Colour = Color.Blue,
                position = new Vector2(GameWorld.renderTarget.Width / 2 - (playerTexture.Width / 2 + 200), GameWorld.renderTarget.Height / 2 - (playerTexture.Height / 2)),
                Layer = 0.3f,
                Health = 10,
            };

            player2Test = new Player(playerTexture)
            {
                Colour = Color.Green,
                position = new Vector2(GameWorld.renderTarget.Width / 2 - (playerTexture.Width / 2 - 200), GameWorld.renderTarget.Height / 2 - (playerTexture.Height / 2)),
                Layer = 0.4f,
                Health = 10,
            };


            // Frederik
            if (playerCount >= 1)
            {
                gameObjects.Add(player1Test);
 
            }

            // Frederik
            if (playerCount >= 2)
            {
                gameObjects.Add(player2Test);
            }

            players = gameObjects.Where(c => c is Player).Select(c => (Player)c).ToList();

        }

        public override void Update(GameTime gameTime)
        {
            // Frederik
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
            // When sprites collide = attacks colliding with enemy (killing them) (unload game-specific content)

            // If player is dead, show game over screen
            // Frederik
            if (players.All(c => c.IsDead))
            {
                //highscores can also be added here (to be shown in the game over screen)

                _game.ChangeState(new GameOverState(_game, _content));
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            // Frederik
            foreach (var sprite in gameObjects)
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin();

            // Frederik
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
        #endregion
    }
}