using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Buttons;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.States
{
    public class HowToState : State
    {
        private List<ComponentMenu> components;

        private SpriteFont buttonFont;
        private Texture2D buttonSprite;
        private Texture2D howToSprite;
        private Texture2D backText;
        private Vector2 howToPosition;
        private Vector2 howToOrigin;
        private Vector2 backPosition;
        private Vector2 backOrigin;

        public HowToState()
        {
        }

        public override void LoadContent()
        {
            buttonFont = content.Load<SpriteFont>("Fonts/font");
            buttonSprite = content.Load<Texture2D>("Controls/button");
            howToSprite = content.Load<Texture2D>("Backgrounds/mainmenu");
            backText = content.Load<Texture2D>("Controls/back");

            components = new List<ComponentMenu>()
            {
                new Button(buttonSprite, buttonFont)
                {
                    Position = new Vector2(480, 980),
                    Click = new EventHandler(Button_Close_Clicked),
                },
            };
        }

        private void Button_Close_Clicked(object sender, EventArgs e)
        {
            GameWorld.ChangeState(GameWorld.menuState);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
            howToPosition = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2);
            howToOrigin = new Vector2(howToSprite.Width / 2, howToSprite.Height / 2);
            backPosition = new Vector2(GameWorld.ScreenWidth / 4, 935);
            backOrigin = new Vector2(backText.Width / 2, backText.Height / 2);
        }

        //public override void PostUpdate(GameTime gameTime)
        //{
        //    throw new NotImplementedException();
        //}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(howToSprite, howToPosition, null, Color.White, 0, howToOrigin, 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(backText, backPosition, null, Color.White, 0, backOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
