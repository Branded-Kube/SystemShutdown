using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Buttons;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.States
{
    public class GameOverState : State
    {
        #region Fields
        private List<ComponentMenu> components;

        private SpriteFont font;

        #endregion

        #region Methods

        #region Constructor
        public GameOverState()
        {
        }
        #endregion

        public override void LoadContent()
        {
            //Frederik
            font = content.Load<SpriteFont>("Fonts/font");

            var buttonTexture = content.Load<Texture2D>("Controls/button");
            var buttonFont = content.Load<SpriteFont>("Fonts/font");

            components = new List<ComponentMenu>()
            {
                //new GameObject()
                //{
                //    sprite = content.Load<Texture2D>("Backgrounds/gameover"),
                //    Layer = 0f,
                //    position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
                //},

                new Button(buttonTexture, buttonFont)
                {
                    Text = "Main Menu",
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 500),
                    Click = new EventHandler(Button_MainMenu_Clicked),
                    Layer = 0.1f
                },
            };
        }

        // Frederik
        private void Button_MainMenu_Clicked(object sender, EventArgs e)
        {
            GameWorld. ChangeState(new MenuState());
        }

        public override void Update(GameTime gameTime)
        {
            // Frederik
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Button_MainMenu_Clicked(this, new EventArgs());
            }

            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //(unload game-specific content)
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