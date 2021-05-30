using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.Components
{
    public class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

       // public Rectangle rectangle { get; set; }


        //public SpriteRenderer()
        //{
        //}

        public SpriteRenderer(string spriteName)
        {
            SetSprite(spriteName);
        }

        //public SpriteRenderer()
        //{
        //}

        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color.White, Rotation, Origin, 1, SpriteEffects.None, 0);
            //spriteBatch.Draw(Sprite, GameObject.Transform.Position, rectangle, Color.White, 0, Origin, 1, SpriteEffects.None, 0);
            
        }

        
        public override string ToString()
        {
            return "SpriteRenderer";
        }

        public SpriteRenderer Clone()
        {
            return (SpriteRenderer)this.MemberwiseClone();
        }
    }
}
