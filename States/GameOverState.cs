using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Buttons;
using SystemShutdown.Components;

namespace SystemShutdown.States
{
    // Lead author: Frederik
    // Contributor: Søren
    public class GameOverState : State
    {
        #region Fields
        private List<StateComponent> components;

        private SpriteFont buttonFont;
        private Texture2D buttonTexture;
        private Texture2D gameOverSprite;
        private Texture2D saveText;
        private Texture2D initialText;
        private Texture2D quitGameText;
        private Texture2D savedScoreText;
        private Texture2D enterInitialText;
        private Texture2D returnToMenuText;
        private Vector2 gameOverPosition;
        private Vector2 gameOverOrigin;
        private Vector2 savePosition;
        private Vector2 saveOrigin;
        private Vector2 initialPosition;
        private Vector2 initialOrigin;
        private Vector2 quitGamePosition;
        private Vector2 quitGameOrigin;
        private Vector2 savedScorePos;
        private Vector2 enterInitialPos;
        private Vector2 returnToMenuOrigin;
        private Vector2 returnToMenuPosition;
        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;

        private Song gameOverMusic;

        private bool scoreSaved = false;

        public bool hasChosenInitials = false;

        private bool isSetttingInitials = false;

        #endregion

        #region Methods

        #region Constructor

        #endregion

        public override void LoadContent()
        {
            //Frederik
            buttonFont = content.Load<SpriteFont>("Fonts/font");
            buttonTexture = content.Load<Texture2D>("Controls/button");
            gameOverSprite = content.Load<Texture2D>("Backgrounds/gameovermenu");
            saveText = content.Load<Texture2D>("Controls/savescore");
            initialText = content.Load<Texture2D>("Controls/initials");
            quitGameText = content.Load<Texture2D>("Controls/quitgame");
            savedScoreText = content.Load<Texture2D>("Controls/scoresaved");
            enterInitialText = content.Load<Texture2D>("Controls/enterinitials");
            returnToMenuText = content.Load<Texture2D>("Controls/mainmenu");

            gameOverMusic = content.Load<Song>("Sounds/song03");

            MediaPlayer.Play(gameOverMusic);
            MediaPlayer.IsRepeating = true;

            components = new List<StateComponent>()
            {
                new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 2 - buttonTexture.Width - 40, 1000),
                    Click = new EventHandler(Button_SaveHighscore_Clicked),
                },

                new Button (buttonTexture, buttonFont)
                {
                    Position = new Vector2 (GameWorld.Instance.ScreenWidth /2, 1000),
                    Click = new EventHandler(CreateInitialsButton_Clicked),
                },

                new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 2 + buttonTexture.Width + 40, 1000),
                    Click = new EventHandler(Button_Quit_Clicked),
                },
                 new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(GameWorld.Instance.ScreenWidth / 2 + buttonTexture.Width - 1000, 1000),
                    Click = new EventHandler(Button_ToMenu_Clicked),
                }
            };
        }

        private void Button_ToMenu_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.StopSettingInitials();
            Highscores.PlayerNameInput = new StringBuilder("UserName");

            Highscores.user = true;
            GameWorld.ChangeState(new MenuState());
        }
        private void Button_Quit_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.Exit();
        }

        //Søren
        private void Button_SaveHighscore_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton2.Play();

            if (!scoreSaved)
            {
                GameWorld.Instance.Repo.Open();

                GameWorld.Instance.Repo.SaveScore(Highscores.PlayerNameInput.ToString(), GameWorld.Instance.GameState.PlayerBuilder.Player.Kills, GameWorld.Instance.GameState.Days);

                GameWorld.Instance.Repo.Close();

                scoreSaved = true;
            }

        }

        //Søren
        private void CreateInitialsButton_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton.Play();

            if (!hasChosenInitials)
            {
                GameWorld.Instance.SetInitials();
                hasChosenInitials = true;
            }

            if (!isSetttingInitials)
            {
                isSetttingInitials = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Frederik
            gameOverPosition = new Vector2(GameWorld.Instance.ScreenWidth / 2, GameWorld.Instance.ScreenHeight / 2);
            gameOverOrigin = new Vector2(gameOverSprite.Width / 2, gameOverSprite.Height / 2);
            savePosition = new Vector2(GameWorld.Instance.ScreenWidth / 2 - buttonTexture.Width - 40, 955);
            saveOrigin = new Vector2(saveText.Width / 2, saveText.Height / 2);
            initialPosition = new Vector2(GameWorld.Instance.ScreenWidth / 2, 955);
            initialOrigin = new Vector2(initialText.Width / 2, initialText.Height / 2);
            quitGamePosition = new Vector2(GameWorld.Instance.ScreenWidth / 2 + buttonTexture.Width + 40, 955);
            quitGameOrigin = new Vector2(quitGameText.Width / 2, quitGameText.Height / 2);
            returnToMenuPosition = new Vector2(GameWorld.Instance.ScreenWidth / 2 + buttonTexture.Width - 1000, 955);
            returnToMenuOrigin = new Vector2(returnToMenuText.Width / 2, returnToMenuText.Height / 2);

            enterInitialPos = new Vector2((GameWorld.Instance.ScreenWidth / 2) - 175, 315);
            savedScorePos = new Vector2((GameWorld.Instance.ScreenWidth / 2) - 250, 150);
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            if (currentKeyState.IsKeyDown(Keys.Escape) && !previousKeyState.IsKeyDown(Keys.Escape))
            {
                GameWorld.ChangeState(new MenuState());
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            spriteBatch.Draw(gameOverSprite, gameOverPosition, null, Color.White, 0, gameOverOrigin, 1f, SpriteEffects.None, 0.1f);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(gameOverSprite, gameOverPosition, null, Color.White, 0, gameOverOrigin, 1f, SpriteEffects.None, 0.1f);

            if (isSetttingInitials)
            {
                spriteBatch.Draw(enterInitialText, enterInitialPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(buttonFont, Highscores.PlayerNameInput, new Vector2((GameWorld.Instance.ScreenWidth / 2) - 125, 425), Color.Black, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
            }

            if (scoreSaved)
            {
                spriteBatch.Draw(savedScoreText, savedScorePos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(initialText, initialPosition, null, Color.White, 0, initialOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(quitGameText, quitGamePosition, null, Color.Red, 0, quitGameOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(returnToMenuText, returnToMenuPosition, null, Color.Green, 0, quitGameOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(saveText, savePosition, null, Color.White, 0, saveOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
        #endregion
    }
}