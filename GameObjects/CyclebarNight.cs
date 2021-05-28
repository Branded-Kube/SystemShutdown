using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.BuildPattern;

namespace SystemShutdown.GameObjects
{
    public class CyclebarNight
    {
        public Texture2D nightContainer, nightBar;
        public Vector2 nightBarPosition;
        public int fullBarNight;
        public float currentBarNight;
        public float nightMeter = 1f;
        public Color nightBarColor;
        private PlayerBuilder playerBuilder;

        public CyclebarNight(ContentManager content)
        {
            LoadContent(content);

            fullBarNight = nightBar.Width;
            currentBarNight = fullBarNight;
        }

        private void LoadContent(ContentManager content)
        {
            nightContainer = content.Load<Texture2D>("Textures/HealthbarEmpty");
            nightBar = content.Load<Texture2D>("Textures/Healthbar");
        }

        public void Update(/*GameTime gameTime*/)
        {
            

            if (currentBarNight >= 0)
            {
                currentBarNight -= nightMeter;
            }
            NightColor();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (isNight == true)
            //{
            spriteBatch.Draw(nightBar, new Vector2(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.X + 635,
                GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.Y - 455), new Rectangle((int)nightBarPosition.X,
                (int)nightBarPosition.Y, (int)currentBarNight, nightBar.Height), nightBarColor);
            spriteBatch.Draw(nightContainer, new Vector2(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.X + 635,
                GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.Y - 455), Color.White);
            //}

            //if (cyclebarNight.currentBarNight <= 0)
            //{
            //    isNight = false;
            //    isDay = true;
            //    cyclebarDay.currentBarDay = cyclebarDay.fullBarDay;
            //    cycle = Cycle.DAY;
            //}
        }

        public void NightColor(/*GameTime gameTime*/)
        {
            if (currentBarNight >= nightBar.Width * 0.70)
                nightBarColor = Color.DarkBlue;
            else if (currentBarNight >= nightBar.Width * 0.40)
                nightBarColor = Color.MediumBlue;
            else if (currentBarNight >= nightBar.Width * 0.25)
                nightBarColor = Color.DarkOrange;
            else
                nightBarColor = Color.Yellow;
        }
    }
}
