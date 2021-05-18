//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SystemShutdown.AStar
//{
//    public class EnemyAstar
//    {
//        private Texture2D sprite;
//        private Rectangle rectangle;

//        public EnemyAstar(Rectangle Rectangle)
//        {
//            this.rectangle = Rectangle;
//        }

//        public Rectangle position
//        {
//            get { return rectangle; }

//        }

//        public void Move(int x, int y)
//        {
//            rectangle.X = x;
//            rectangle.Y = y;
//        }
//        public void LoadContent(ContentManager content)
//        {
//            sprite = content.Load<Texture2D>("Textures/pl1");
//        }
//        public void Draw(SpriteBatch spritebatch)
//        {
//            spritebatch.Draw(sprite, rectangle, Color.Red);
//        }
//    }

//}
