using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SystemShutdown.Buttons;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class MenuState : State
    {
        private List<ButtonComponent> components;

        #region Methods

        #region Constructor
        public MenuState(GameWorld game, ContentManager content)
          : base(game, content)
        {
        }
        #endregion

        public override void LoadContent()
        {
            // Frederik
            var buttonTexture = content.Load<Texture2D>("Controls/button");
            var buttonFont = content.Load<SpriteFont>("Fonts/font");

            components = new List<ButtonComponent>()
            {
                new GameObject()
                {
                    sprite = content.Load<Texture2D>("Backgrounds/mainmenu"),
                    Layer = 0f,
                    position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
                },

                new Button(buttonTexture, buttonFont)
                {
                    Text = "1 Player",
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 300),
                    Click = new EventHandler(Button_1Player_Clicked),
                    Layer = 0.1f
                },

                new Button(buttonTexture, buttonFont)
                {
                    Text = "2 Players",
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 360),
                    Click = new EventHandler(Button_2Player_Clicked),
                    Layer = 0.1f
                },

                new Button(buttonTexture, buttonFont)
                {
                    Text = "Quit Game",
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 420),
                    Click = new EventHandler(Button_Quit_Clicked),
                    Layer = 0.1f
                },
            };
        }

        // Frederik
        private void Button_1Player_Clicked(object sender, EventArgs e)
        {
            GameWorld.gameState = new GameState(_game, content);
            _game.ChangeState(GameWorld.gameState);
        }

        // Frederik
        private void Button_2Player_Clicked(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, content)
            {
                playerCount = 2,
            });
        }

        // Frederik
        private void Button_Quit_Clicked(object sender, EventArgs e)
        {
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            // Frederik
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //(unload game - specific content)
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
        #endregion
    }
}