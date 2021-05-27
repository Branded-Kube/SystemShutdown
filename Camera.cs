using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.BuildPattern;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
   public class Camera

    {
        public Matrix offset;

        public Matrix position;

        public Matrix Transform { get; private set; }

        public void Follow(PlayerBuilder target)
        {
            position = Matrix.CreateTranslation(-target.Player.GameObject.Transform.Position.X - (target.Sr.Sprite.Width / 2),
                -target.Player.GameObject.Transform.Position.Y - (target.Sr.Sprite.Height / 2),
                0);

            offset = Matrix.CreateTranslation(GameWorld.ScreenWidth / 2,
                GameWorld.ScreenHeight / 2,
                0);
            Transform = position * offset;

        }

    }


}
