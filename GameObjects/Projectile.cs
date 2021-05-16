using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.GameObjects
{
    class Projectile : MenuObject
    {
        //public Rectangle laserRectangle;
        //private float speed;

        //public Projectile(float speed, Vector2 velocity)
        //{
        //    this.speed = 100;
        //    //this.currentDir = velocity;
        //}

        ////public override void Awake()
        ////{
        ////    GameObject1.Tag = "Laser";
        ////}

        //public void LoadContent(ContentManager content)
        //{
        //    sprite = content.Load<Texture2D>("Textures/Laser");
        //    //laserRectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(sprite.Width - 10, sprite.Height - 10));

        //}

        ////public override void Update(GameTime gameTime)
        ////{
        ////    Move();
        ////}

        ////private void Move()
        ////{
        ////    //GameObject.Transform.Translate(velocity * speed);
        ////}
    }
}
