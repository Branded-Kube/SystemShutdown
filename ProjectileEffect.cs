using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SystemShutdown
{
    // Lead author: Ras
    public class ProjectileEffect
    {
        private Texture2D effectTexture;
        private Vector2 effectVector;
        private Double timer = 0.0;

        public Texture2D[] walk;
        public float fps;
        public float timeElapsed;
        public int currentIndex;

        public ProjectileEffect(Vector2 effectVector)
        {
            fps = 6f;
            this.effectVector = effectVector;
            walk = new Texture2D[3];

            //Loop animaiton textures
            for (int g = 0; g < walk.Length; g++)
            {
                walk[g] = GameWorld.Instance.Content.Load<Texture2D>($"projectileEffect{g + 1}");
            }
            effectTexture = walk[0];
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(effectTexture, effectVector, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (timer >= 0.3)
            {
                GameWorld.Instance.GameState.Effects.RemoveAt(0);
            }
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            Animate(gameTime);
        }
        public void Animate(GameTime gametime)
        {
            // Gives time that has passed since last update
            timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            // Calculates currentIndex
            currentIndex = (int)(timeElapsed * fps);

            effectTexture = walk[currentIndex];

            // Checks if animation needs to restart
            if (currentIndex >= walk.Length - 1)
            {
                // Resets animation
                timeElapsed = 0;
                currentIndex = 0;
            }
        }
    }
}
