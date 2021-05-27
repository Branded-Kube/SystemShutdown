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
    public class Cyclesbar /*: Component, IGameListener*/
    {
        public Texture2D healthContainer, healthBar;
        public Vector2 healthPosition;
        public int fullHealth;
        public int currentHealth;
        public int healthMeter = 1;
        public Color barColor;
        //private SpriteRenderer sr;
        private PlayerBuilder playerBuilder;

        public Cyclesbar(ContentManager content)
        {
            //healthPosition = new Vector2(playerBuilder.Player.GameObject.Transform.Position.X - 50, playerBuilder.Player.GameObject.Transform.Position.Y - 50);

            LoadContent(content);
            //LoadContent(GameWorld.content);

            fullHealth = healthBar.Width;
            currentHealth = fullHealth;
        }

        //public override void Awake()
        //{
        //    GameObject.Tag = "Cycle";
        //    GameObject.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height);
        //    spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        //}


        private void LoadContent(ContentManager content)
        {
            healthContainer = GameWorld.content.Load<Texture2D>("Textures/HealthbarEmpty");
            healthBar = GameWorld.content.Load<Texture2D>("Textures/Healthbar");
        }

        public void Update()
        {
            HealthColor();

            if (currentHealth >= 0)
                currentHealth -= healthMeter;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(healthBar, new Vector2(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.X - 50, playerBuilder.Player.GameObject.Transform.Position.Y - 50), new Rectangle((int)healthPosition.X, (int)healthPosition.Y, currentHealth, healthBar.Height), barColor);
            //spriteBatch.Draw(healthContainer, new Vector2(GameWorld.gameState.playerBuilder.Player.GameObject.Transform.Position.X - 50, playerBuilder.Player.GameObject.Transform.Position.Y - 50), Color.White);
        }

        public void HealthColor()
        {
            if (currentHealth >= healthBar.Width * 0.60)
                barColor = Color.Green;
            else if (currentHealth >= healthBar.Width * 0.40)
                barColor = Color.DarkOrange;
            else if (currentHealth >= healthBar.Width * 0.20)
                barColor = Color.OrangeRed;
            else
                barColor = Color.Red;
        }
    }
}
