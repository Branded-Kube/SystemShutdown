using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown
{
    // Hovedforfatter: Frederik
    public class Transform
    {
        public Vector2 Position { get; set; }

        public Vector2 Rotation { get; set; }

        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }
    }
}
