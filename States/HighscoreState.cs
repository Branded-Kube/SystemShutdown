using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Texture2D sprite;
        private Texture2D backText;
        private Vector2 backPosition;
        private Vector2 backOrigin;
        private Vector2 position;
        private Vector2 origin;

      //  private int i = 1;

        private bool highscoresDrawed = false;

        public HighscoreState()
        {

            //GameWorld.Instance.Repo.Open();

            //GameWorld.Instance.Repo.ScoreHandler();



            //GameWorld.Instance.Repo.Close();

        }

        public override void LoadContent()
        {
            buttonFont = content.Load<SpriteFont>("Fonts/font");
            buttonSprite = content.Load<Texture2D>("Controls/button");
            sprite = content.Load<Texture2D>("Backgrounds/scoremenu");
            backText = content.Load<Texture2D>("Controls/back");

            components = new List<StateComponent>()
            {
                new Button(buttonSprite, buttonFont)
                {
                    Position = new Vector2(125, 970),
                    Click = new EventHandler(Button_Close_Clicked),
                },
            };
        }

        private void Button_Close_Clicked(object sender, EventArgs e)
        {
            GameWorld.Instance.clickButton2.Play();
            GameWorld.ChangeState(GameWorld.Instance.MenuState);
        }



        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
            position = new Vector2(GameWorld.Instance.ScreenWidth / 2, GameWorld.Instance.ScreenHeight / 2);
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            backPosition = new Vector2(125, 930);
            backOrigin = new Vector2(backText.Width / 2, backText.Height / 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, 1f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(backText, backPosition, null, Color.White, 0, backOrigin, 1f, SpriteEffects.None, 0f);



            GameWorld.Instance.Repo.Open();

            GameWorld.Instance.Repo.ScoreHandler();

            //while (GameWorld.Instance.Repo.Reader.Read() /*&& GameWorld.Instance.Repo. <= 10*/)
            //{
            //    spriteBatch.DrawString(buttonFont, ("PlayerName: " + GameWorld.Instance.Repo.Reader["PlayerName"]/* + "Kills: " + GameWorld.Instance.Repo.Reader["Kills"] + "DaysSurvived: " + GameWorld.Instance.Repo.Reader["DaysSurvived"]*/), new Vector2(GameWorld.Instance.ScreenWidth / 2, 200 + (i)), Color.Red);



            //    //for (int i = 0; i < GameWorld.Instance.Repo.Reader.FieldCount; i++)
            //    //{
            //    //    spriteBatch.DrawString(buttonFont, ("PlayerName: " + GameWorld.Instance.Repo.Reader["PlayerName"]/* + "Kills: " + GameWorld.Instance.Repo.Reader["Kills"] + "DaysSurvived: " + GameWorld.Instance.Repo.Reader["DaysSurvived"]*/), new Vector2(GameWorld.Instance.ScreenWidth / 2, 200 + (i* 50)), Color.Red);
            //    //    //Debug.WriteLine(("PlayerName: " + GameWorld.Instance.Repo.Reader["PlayerName"] + "Kills: " + GameWorld.Instance.Repo.Reader["Kills"] + "DaysSurvived: " + GameWorld.Instance.Repo.Reader["DaysSurvived"]));
            //    //}
            //}

            int i = 0;
            while (GameWorld.Instance.Repo.Reader.Read())
            {

                    Debug.WriteLine("PlayerName: " + GameWorld.Instance.Repo.Reader["PlayerName"] + "Kills: " + GameWorld.Instance.Repo.Reader["Kills"] + "DaysSurvived: " + GameWorld.Instance.Repo.Reader["DaysSurvived"]);
                    GameWorld.Instance.Repo.Score = ("PlayerName: " + GameWorld.Instance.Repo.Reader["PlayerName"] + "Kills: " + GameWorld.Instance.Repo.Reader["Kills"] + "DaysSurvived: " + GameWorld.Instance.Repo.Reader["DaysSurvived"]);

                    spriteBatch.DrawString(buttonFont, (GameWorld.Instance.Repo.Score), new Vector2(GameWorld.Instance.ScreenWidth / 2, 200 + (i * 50)), Color.Red);

                    i++;
      
            }


            GameWorld.Instance.Repo.Close();


            //   spriteBatch.DrawString(buttonFont, "something to test", new Vector2 (500,500), Color.White);

            //     spriteBatch.DrawString(buttonFont, ($"{}"), new Vector2(GameWorld.ScreenWidth / 2, GameWorld.ScreenHeight / 2), Color.White);


            spriteBatch.End();
        }

    }
}
