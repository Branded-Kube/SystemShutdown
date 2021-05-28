using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.GameObjects
{
    public class CyclebarNight
    {
        public Texture2D nightContainer, nightBar;
        public Vector2 nightBarPosition;
        public int fullBarNight;
        public float currentBarNight;
        public float nightMeter = 0.7f;
        public Color nightBarColor;

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

        public void Update()
        {
            NightColor();

            if (currentBarNight >= 0)
            {
                currentBarNight -= nightMeter;
            }
        }

        public void NightColor()
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
