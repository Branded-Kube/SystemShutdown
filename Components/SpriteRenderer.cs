using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SystemShutdown.Components
{
    // Lead author: Frederik
    public class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

        public SpriteRenderer(string spriteName)
        {
            SetSprite(spriteName);
        }
        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color.White, Rotation, Origin, 1, SpriteEffects.None, 0);
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
