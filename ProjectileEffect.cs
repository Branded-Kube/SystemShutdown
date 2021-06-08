using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown
{
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
           // effectTexture = GameWorld.Instance.GameState.projektilEffectTexture;
            walk = new Texture2D[3];

            //Loop animaiton textures
            for (int g = 0; g<walk.Length; g++)
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
            //Giver tiden, der er gået, siden sidste update
            timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            //Beregner currentIndex
            currentIndex = (int)(timeElapsed * fps);

            effectTexture = walk[currentIndex];

            //Checks if animation needs to restart
            if (currentIndex >= walk.Length - 1)
            {
                //Resets animation
                timeElapsed = 0;
                currentIndex = 0;
            }
        }
    }
}
