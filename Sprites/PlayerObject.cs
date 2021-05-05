using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.Sprites
{
    public class PlayerObject : GameObject
    {
        public float speed;

        public int Health { get; set; }

        public PlayerObject(Texture2D texture)
            : base(texture)
        {
        }

        protected void Shoot(float speed)
        {
            //bullet stuff
        }

        public virtual void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}
