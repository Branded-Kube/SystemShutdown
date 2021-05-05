﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Buttons;
using SystemShutdown.Sprites;

namespace SystemShutdown.States
{
    public class GameOverState : State
    {
        private List<Component> components;

        private SpriteFont font;

        public GameOverState(GameWorld game, ContentManager content)
            : base(game, content)
        {
        }

        public override void LoadContent()
        {
            font = _content.Load<SpriteFont>("Fonts/font");

            var buttonTexture = _content.Load<Texture2D>("Controls/button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/font");

            components = new List<Component>()
            {
                new GameObject(_content.Load<Texture2D>("Backgrounds/gameover"))
                {
                    Layer = 0f,
                    Position = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2),
                },

                new Button(buttonTexture, buttonFont)
                {
                    Text = "Main Menu",
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 500),
                    Click = new EventHandler(Button_MainMenu_Clicked),
                    Layer = 0.1f
                },
            };
        }

        private void Button_MainMenu_Clicked(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _content));
        }

        public override void Update(GameTime gameTime)
        {
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
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
