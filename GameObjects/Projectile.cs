using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.GameObjects
{
    class Projectile : GameObject
    {
        public Rectangle laserRectangle;
        private float speed;

        public Projectile()
        {
            this.speed = 100;
        }
        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Textures/Laser");
            laserRectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(sprite.Width - 10, sprite.Height - 10));

        }

    }
}
