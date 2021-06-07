using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.Buttons;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.States
{
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

        private Song gameOverMusic;

        private bool scoreSaved = false;

        public bool hasChosenInitials = false;

        private bool isSetttingInitials = false;


        public bool IsSettingInitials
        {
            get { return isSetttingInitials; }
            set { isSetttingInitials = value; }
        }

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
            buttonFont = content.Load<SpriteFont>("Fonts/font");
            buttonTexture = content.Load<Texture2D>("Controls/button");
            gameOverSprite = content.Load<Texture2D>("Backgrounds/gameovermenu");
            saveText = content.Load<Texture2D>("Controls/savescore");
            initialText = content.Load<Texture2D>("Controls/initials");
            quitGameText = content.Load<Texture2D>("Controls/quitgame");
            savedScoreText = content.Load<Texture2D>("Controls/scoresaved");
            enterInitialText = content.Load<Texture2D>("Controls/enterinitials");

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
                }
            };
        }

        private void Button_Quit_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.Exit();
        }

        private void Button_SaveHighscore_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton2.Play();

            if (!scoreSaved)
            {
                GameWorld.Instance.repo.Open();

                GameWorld.Instance.repo.SaveScore(Highscores.PlayerNameInput.ToString(), GameWorld.Instance.gameState.playerBuilder.Player.kills, GameWorld.Instance.gameState.days);

                GameWorld.Instance.repo.Close();

                scoreSaved = true;
            }
            
        }

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


        //public void SetInitials()
        //{
        //    Window.TextInput += UserLogin.CreateUsernameInput;
        //}

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

            enterInitialPos = new Vector2((GameWorld.Instance.ScreenWidth / 2) - 150, 315);
            savedScorePos = new Vector2((GameWorld.Instance.ScreenWidth / 2) - 200, 150);
            

            foreach (var component in components)
            {
                component.Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {

                GameWorld.Instance.Exit();
            }
        }

        //public override void PostUpdate(GameTime gameTime)
        //{
        //    //(unload game-specific content)
        //}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(gameOverSprite, gameOverPosition, null, Color.White, 0, gameOverOrigin, 1f, SpriteEffects.None, 0.1f);
            
            //spriteBatch.DrawString(buttonFont, "Save highscore" , new Vector2 (GameWorld.Instance.ScreenWidth / 2, 380), Color.White);

            //spriteBatch.DrawString(buttonFont, "Set initials", new Vector2(GameWorld.Instance.ScreenWidth / 2, 500), Color.White);


            if (isSetttingInitials)
            {
                spriteBatch.Draw(enterInitialText, enterInitialPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                //spriteBatch.DrawString(buttonFont, "Enter your initials", new Vector2((GameWorld.Instance.ScreenWidth / 2) - 100, 550), Color.Black, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(buttonFont, Highscores.PlayerNameInput, new Vector2((GameWorld.Instance.ScreenWidth / 2) - 125, 425), Color.Black, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
            }

            if (scoreSaved)
            {
                spriteBatch.Draw(savedScoreText, savedScorePos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                //spriteBatch.DrawString(buttonFont, "Your score has been saved", new Vector2((GameWorld.Instance.ScreenWidth / 2) - 100, 100), Color.Black, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.Draw(saveText, savePosition, null, Color.White, 0, saveOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(initialText, initialPosition, null, Color.White, 0, initialOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(quitGameText, quitGamePosition, null, Color.White, 0, quitGameOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
        #endregion
    }
}