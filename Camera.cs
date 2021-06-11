using Microsoft.Xna.Framework;
using SystemShutdown.BuildPattern;

namespace SystemShutdown
{
    // Lead author: Søren
    public class Camera
    {
        #region Fields
        public Matrix offset;
        public Matrix position;
        public Matrix Transform { get; private set; }
        #endregion
        #region Methods
        

        /// <summary>
        /// Will follow the player.
        /// </summary>
        /// <param name="target"></param>
        public void Follow(PlayerBuilder target)
        {
            position = Matrix.CreateTranslation(-target.Player.GameObject.Transform.Position.X - (target.Sr.Sprite.Width / 2),
                -target.Player.GameObject.Transform.Position.Y - (target.Sr.Sprite.Height / 2),
                0);

            offset = Matrix.CreateTranslation(GameWorld.Instance.ScreenWidth / 2,
                GameWorld.Instance.ScreenHeight / 2,
                0);
            Transform = position * offset;
        }
        #endregion
    }
}
