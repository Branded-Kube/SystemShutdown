using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown
{
    public class Transform
    {
        public Vector2 Position { get; set; }

        /// <summary>
        /// This will move the transform in the direction and distance of the translation.
        /// IsNaN checks if X & Y position already exists (is instantiated). To make sure we don't get an exception.
        /// </summary>
        /// <param name="translation"></param>
        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }
    }
}
