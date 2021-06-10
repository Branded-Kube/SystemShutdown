using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SystemShutdown.Buttons;
using SystemShutdown.Components;
using SystemShutdown.States;

namespace SystemShutdown
{
    // Lead author: Frederik
    // Contributor: Søren
    public class MenuState : State
    {
        private List<StateComponent> components;

        private SpriteFont buttonFont;
        private Texture2D buttonSprite;
        private Texture2D sprite;
        private Texture2D newGameText;
        private Texture2D howToText;
        private Texture2D quitGameText;
        private Texture2D highscoreText;
        private Vector2 position;
        private Vector2 origin;
        private Vector2 newGamePosition;
        private Vector2 howToPosition;
        private Vector2 quitGamePosition;
        private Vector2 highscorePos;
        private Vector2 newGameOrigin;
        private Vector2 howToOrigin;
        private Vector2 quitGameOrigin;
        private Vector2 highscoreOrigin;

        private Song menuMusic;

        #region Methods

      

        public override void LoadContent()
        {
            // Frederik
            buttonFont = content.Load<SpriteFont>("Fonts/font");
            buttonSprite = content.Load<Texture2D>("Controls/button");
            sprite = content.Load<Texture2D>("Backgrounds/menu");
            newGameText = content.Load<Texture2D>("Controls/newgame");
            howToText = content.Load<Texture2D>("Controls/howtoplay");
            highscoreText = content.Load<Texture2D>("Controls/highscore");
            quitGameText = content.Load<Texture2D>("Controls/quitgame");

            menuMusic = content.Load<Song>("Sounds/song01");

            MediaPlayer.Play(menuMusic);
            MediaPlayer.IsRepeating = true;

            components = new List<StateComponent>()
            {
                new Button(buttonSprite, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 310),
                    Click = new EventHandler(Button_PlayGame_Clicked),
                },

                new Button(buttonSprite, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 440),
                    Click = new EventHandler(Button_HowToPlay_Clicked),
                },

                new Button (buttonSprite, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 570),
                    Click = new EventHandler(Button_CheckHighscore_Clicked),
                },

                new Button(buttonSprite, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 700),
                    Click = new EventHandler(Button_Quit_Clicked),
                }
            };
        }

        // Frederik
        private void Button_PlayGame_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton5.Play();
            GameState newGameState = new GameState();
            GameWorld.Instance.GameState = newGameState;
            GameWorld.ChangeState(GameWorld.Instance.GameState);
        }

        // Frederik
        public void Button_HowToPlay_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton3.Play();
            GameWorld.ChangeState(GameWorld.Instance.HowToState);
        }

        // Frederik
        private void Button_Quit_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.GameState.ShutdownThreads();
            GameWorld.Instance.Exit();
        }

        // Søren
        private void Button_CheckHighscore_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton.Play();
            GameWorld.ChangeState(GameWorld.Instance.HighscoreState);
        }

        public override void Update(GameTime gameTime)
        {
            // Frederik
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
            position = new Vector2(GameWorld.Instance.ScreenWidth / 2, GameWorld.Instance.ScreenHeight / 2);
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            newGamePosition = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 265);
            newGameOrigin = new Vector2(newGameText.Width / 2, newGameText.Height / 2);
            howToPosition = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 395);
            howToOrigin = new Vector2(howToText.Width / 2, howToText.Height / 2);
            highscorePos = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 530);
            highscoreOrigin = new Vector2(highscoreText.Width / 2, highscoreText.Height / 2);
            quitGamePosition = new Vector2(GameWorld.Instance.ScreenWidth / 4 + 50, 655);
            quitGameOrigin = new Vector2(quitGameText.Width / 2, quitGameText.Height / 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(newGameText, newGamePosition, null, Color.White, 0, newGameOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(howToText, howToPosition, null, Color.White, 0, howToOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(highscoreText, highscorePos, null, Color.White, 0, highscoreOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(quitGameText, quitGamePosition, null, Color.White, 0, quitGameOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
        #endregion
    }
}