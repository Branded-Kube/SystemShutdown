using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.BuildPattern;

namespace SystemShutdown.GameObjects
{
    // Hovedforfatter: Frederik
    public class CyclebarDay
    {
        public Texture2D sunSprite;
        public Texture2D dayContainer, dayBar;
        public Vector2 dayBarPosition;
        public int fullBarDay;
        public float currentBarDay;
        public float dayMeter = 0.1f;
        public Color dayBarColor;

        public CyclebarDay(ContentManager content)
        {
            LoadContent(content);

            fullBarDay = dayBar.Width;
            currentBarDay = fullBarDay;
        }

        private void LoadContent(ContentManager content)
        {
            sunSprite = content.Load<Texture2D>("Textures/sun");
            dayContainer = content.Load<Texture2D>("Textures/HealthbarEmpty");
            dayBar = content.Load<Texture2D>("Textures/Healthbar");
        }

        public void Update()
        {
            if (currentBarDay >= 0)
            {
                currentBarDay -= dayMeter;
            }
            DayColor();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sunSprite, new Vector2(GameWorld.Instance.GameState.PlayerBuilder.player.GameObject.Transform.Position.X + 525,
                GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.Y - 480), Color.White);

            //Draws bar
            spriteBatch.Draw(dayBar, new Vector2(GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.X + 635,
                GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.Y - 455), new Rectangle((int)dayBarPosition.X,
                (int)dayBarPosition.Y, (int)currentBarDay, dayBar.Height), dayBarColor);
            spriteBatch.Draw(dayContainer, new Vector2(GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.X + 635,
                GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.Y - 455), Color.White);
        }

        public void DayColor()
        {
            if (currentBarDay >= dayBar.Width * 0.70)
                dayBarColor = Color.Yellow;
            else if (currentBarDay >= dayBar.Width * 0.40)
                dayBarColor = Color.DarkOrange;
            else if (currentBarDay >= dayBar.Width * 0.25)
                dayBarColor = Color.MediumBlue;
            else
                dayBarColor = Color.DarkBlue;
        }
    }
}
