using Microsoft.Xna.Framework;

namespace SystemShutdown
{
    // Lead author: Frederik
    public class Transform
    {
        public Vector2 Position { get; set; }

        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }
    }
}
