using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    class Camera

    {
        public Matrix Transform { get; private set; }

        public void Follow(GameState target)
        {
            var position = Matrix.CreateTranslation(-target.Player1Test.position.X - (target.Player1Test.sprite.Width / 2),
                -target.Player1Test.position.Y - (target.Player1Test.sprite.Height / 2),
                0);

            var offset = Matrix.CreateTranslation(GameWorld.ScreenWidth / 2,
                GameWorld.ScreenHeight / 2,
                0);

            //var position = Matrix.CreateTranslation(-target.GameObject.Transform.Position.X ,
            //    -target.GameObject.Transform.Position.Y,
            //    0);

            //var offset = Matrix.CreateTranslation(Game1.Screensize.X / 2,
            //    Game1.Screensize.Y / 2,
            //    0);

            Transform = position * offset;


        }
    }


}
