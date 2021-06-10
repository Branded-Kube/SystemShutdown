using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SystemShutdown.GameObjects
{
    // Hovedforfatter: Frederik
    public class CyclebarNight
    {
        public Texture2D moonSprite;
        public Texture2D nightContainer, nightBar;
        public Vector2 nightBarPosition;
        public int fullBarNight;
        public float currentBarNight;
        public float nightMeter = 0.15f;
        public Color nightBarColor;

        public CyclebarNight(ContentManager content)
        {
            LoadContent(content);

            fullBarNight = nightBar.Width;
            currentBarNight = fullBarNight;
        }

        private void LoadContent(ContentManager content)
        {
            moonSprite = content.Load<Texture2D>("Textures/moon");
            nightContainer = content.Load<Texture2D>("Textures/HealthbarEmpty");
            nightBar = content.Load<Texture2D>("Textures/Healthbar");
        }

        public void Update()
        {
            if (currentBarNight >= 0)
            {
                currentBarNight -= nightMeter;
            }
            NightColor();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(moonSprite, new Vector2(GameWorld.Instance.GameState.PlayerBuilder.player.GameObject.Transform.Position.X + 525,
                GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.Y - 475), Color.White);

            //Draws bar
            spriteBatch.Draw(nightBar, new Vector2(GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.X + 635,
                GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.Y - 455), new Rectangle((int)nightBarPosition.X,
                (int)nightBarPosition.Y, (int)currentBarNight, nightBar.Height), nightBarColor);
            spriteBatch.Draw(nightContainer, new Vector2(GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.X + 635,
                GameWorld.Instance.GameState.PlayerBuilder.Player.GameObject.Transform.Position.Y - 455), Color.White);
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
