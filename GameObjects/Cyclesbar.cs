using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.BuildPattern;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.GameObjects
{
    public class Cyclesbar
    {
        public Texture2D healthContainer, healthBar;
        public Vector2 healthPosition;
        public int fullHealth;
        public float currentHealth;
        public float healthMeter = 0.7f;
        public Color barColor;

        public Cyclesbar(ContentManager content)
        {
            LoadContent(content);

            fullHealth = healthBar.Width;
            currentHealth = fullHealth;
        }

        private void LoadContent(ContentManager content)
        {
            healthContainer = content.Load<Texture2D>("Textures/HealthbarEmpty");
            healthBar = content.Load<Texture2D>("Textures/Healthbar");
        }

        public void Update()
        {
            HealthColor();

            if (currentHealth >= 0)
                currentHealth -= healthMeter;
        }

        public void HealthColor()
        {
            if (currentHealth >= healthBar.Width * 0.70)
                barColor = Color.Yellow;
            else if (currentHealth >= healthBar.Width * 0.40)
                barColor = Color.DarkOrange;
            else if (currentHealth >= healthBar.Width * 0.25)
                barColor = Color.MediumBlue;
            else
                barColor = Color.DarkBlue;
        }
    }
}
