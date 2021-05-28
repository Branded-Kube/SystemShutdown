using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.GameObjects
{
    public class CyclebarDay
    {
        public Texture2D dayContainer, dayBar;
        public Vector2 dayBarPosition;
        public int fullBarDay;
        public float currentBarDay;
        public float dayMeter = 0.07f;
        public Color dayBarColor;

        public CyclebarDay(ContentManager content)
        {
            LoadContent(content);

            fullBarDay = dayBar.Width;
            currentBarDay = fullBarDay;
        }

        private void LoadContent(ContentManager content)
        {
            dayContainer = content.Load<Texture2D>("Textures/HealthbarEmpty");
            dayBar = content.Load<Texture2D>("Textures/Healthbar");
        }

        public void Update()
        {
            DayColor();

            if (currentBarDay >= 0)
            {
                currentBarDay -= dayMeter;
            }
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

        public void ResetBar()
        {
            fullBarDay = dayBar.Width;
            currentBarDay = fullBarDay;
        }
    }
}
