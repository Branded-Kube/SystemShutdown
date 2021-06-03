using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Buttons;
using SystemShutdown.Components;

namespace SystemShutdown.States
{
   public class HighscoreState : State
    {
        private List<StateComponent> components;

        private SpriteFont buttonFont;
        private Texture2D buttonSprite;
        private Texture2D backText;
        private Vector2 backPosition;
        private Vector2 backOrigin;

   

        public HighscoreState()
        {
        }

        public override void LoadContent()
        {
            buttonFont = content.Load<SpriteFont>("Fonts/font");
            buttonSprite = content.Load<Texture2D>("Controls/button");
            backText = content.Load<Texture2D>("Controls/back");

            components = new List<StateComponent>()
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
            GameWorld.ChangeState(GameWorld.Instance.menuState);
        }



        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
            backPosition = new Vector2(GameWorld.Instance.ScreenWidth / 4, 935);
            backOrigin = new Vector2(backText.Width / 2, backText.Height / 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(backText, backPosition, null, Color.White, 0, backOrigin, 1f, SpriteEffects.None, 0f);

   

         //   spriteBatch.DrawString(buttonFont, "something to test", new Vector2 (500,500), Color.White);

       //     spriteBatch.DrawString(buttonFont, ($"{}"), new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2), Color.White);


            spriteBatch.End();
        }

    }
}
