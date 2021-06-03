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
        private List<StateComponent> components;

        private SpriteFont buttonFont;
        private Texture2D buttonTexture;
        private Texture2D gameOverSprite;
        private Texture2D quitGameText;
        private Vector2 gameOverPosition;
        private Vector2 gameOverOrigin;
        private Vector2 quitGamePosition;
        private Vector2 quitGameOrigin;

        private bool hasChosenName;

        private string tmpName = "Player";

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
            gameOverSprite = content.Load<Texture2D>("Backgrounds/gameover");
            quitGameText = content.Load<Texture2D>("Controls/quitgame");

            components = new List<StateComponent>()
            {
                new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 1000),
                    Click = new EventHandler(Button_Quit_Clicked),
                },

                new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(GameWorld.ScreenWidth / 2, 390),
                    Click = new EventHandler(Button_SaveHighscore_Clicked),
                }
            };
        }

        private void Button_Quit_Clicked(object sender, EventArgs e)
        {
            GameWorld.thisGameWorld.Exit();
        }

        private void Button_SaveHighscore_Clicked(object sender, EventArgs e)
        {
          
            //if (hasChosenName!)
            //{
            //    //menyMsg = "Write your name and Press enter to confirm";


            //    if (inMenu == false)
            //    {
            //        GameWorld.Instance.AddCreateUserLogin();
            //        inMenu = true;
            //    }
            //    if (isCreatingUser == false)
            //    {
            //        isCreatingUser = true;
            //    }


                GameWorld.repo.Open();

                GameWorld.repo.SaveScore(tmpName ,GameWorld.gameState.playerBuilder.Player.kills, GameWorld.gameState.days);

                GameWorld.repo.Close();
            //}


                
            

       
            
            
        }


        //public void SetInitials()
        //{
        //    Window.TextInput += UserLogin.CreateUsernameInput;
        //}

        public override void Update(GameTime gameTime)
        {
            // Frederik
            gameOverPosition = new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2);
            gameOverOrigin = new Vector2(gameOverSprite.Width / 2, gameOverSprite.Height / 2);
            quitGamePosition = new Vector2(GameWorld.ScreenWidth / 2, 855);
            quitGameOrigin = new Vector2(quitGameText.Width / 2, quitGameText.Height / 2);
            

            foreach (var component in components)
            {
                component.Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                
                GameWorld.thisGameWorld.Exit();
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
            spriteBatch.Draw(quitGameText, quitGamePosition, null, Color.White, 0, quitGameOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(buttonFont, "Save highscore" , new Vector2 (), Color.White);

            spriteBatch.End();
        }
        #endregion
    }
}