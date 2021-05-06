using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.GameObjects
{
    public class PlayerObject : GameObject
    {

        public int Health { get; set; }

        public PlayerObject(Texture2D texture)
            : base(texture)
        {
        }

        public virtual void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}
