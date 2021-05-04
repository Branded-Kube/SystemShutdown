using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.Sprites
{
    public class GameObject : Component, ICloneable
    {
        #region Fields
        protected Texture2D _texture;

        protected float _rotation;

        public Vector2 Origin;

        public Vector2 Position;

        public Vector2 Direction;

        public float RotationVelocity = 3f;

        public float LinearVelocity = 4f;

        public GameObject Parent;

        public float LifeSpan = 0f;

        public bool IsRemoved = false;

        public readonly Color[] TextureData;
        #endregion

        #region Properties

        //Bullets
        public List<GameObject> Children { get; set; }

        public Color Colour { get; set; }

        //public Rectangle Rectangle
        //{
        //    get
        //    {
        //        return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
        //    }
        //}

        //public Matrix Transform
        //{
        //    get
        //    {
        //        return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
        //          Matrix.CreateRotationZ(_rotation) *
        //          Matrix.CreateTranslation(new Vector3(Position, 0));
        //    }
        //}


        protected float _Layer { get; set; }

        public float Layer
        {
            get { return _Layer; }
            set
            {
                _Layer = value;
            }
        }
        #endregion

        #region Methods

        #region Constructor

        public GameObject(Texture2D texture)
        {
            _texture = texture;

            // Default origin in the centre of the sprite
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            Colour = Color.White;

            // Bullets
            Children = new List<GameObject>();

            //TextureData = new Color[_texture.Width * _texture.Height];
            //_texture.GetData(TextureData);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Colour, _rotation, Origin, 1, SpriteEffects.None, 0);
        }

        // GameObjects collision (Need to find a easier/better solution)

        //public bool Intersects(GameObject gameObject)
        //{
        //    // Calculate a matrix which transforms from A's local space into world space and then into B's local space
        //    var transformAToB = this.Transform * Matrix.Invert(gameObject.Transform);

        //    // When a point moves in A's local space, it moves in B's local space with a
        //    // fixed direction and distance proportional to the movement in A.
        //    // This algorithm steps through A one pixel at a time, along A's X and Y axes
        //    // Calculate the analogous steps in B:
        //    var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
        //    var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

        //    // Calculate the top left corner of A in B's local space
        //    // This variable will be reused to keep track of the start of each row
        //    var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

        //    for (int yA = 0; yA < this.Rectangle.Height; yA++)
        //    {
        //        // Start at the beginning of the row
        //        var posInB = yPosInB;

        //        for (int xA = 0; xA < this.Rectangle.Width; xA++)
        //        {
        //            // Round to the nearest pixel
        //            var xB = (int)Math.Round(posInB.X);
        //            var yB = (int)Math.Round(posInB.Y);

        //            if (0 <= xB && xB < gameObject.Rectangle.Width &&
        //                0 <= yB && yB < gameObject.Rectangle.Height)
        //            {
        //                // Get the colors of the overlapping pixels
        //                var colourA = this.TextureData[xA + yA * this.Rectangle.Width];
        //                var colourB = gameObject.TextureData[xB + yB * gameObject.Rectangle.Width];

        //                // If both pixel are not completely transparent
        //                if (colourA.A != 0 && colourB.A != 0)
        //                {
        //                    return true;
        //                }
        //            }

        //            // Move to the next pixel in the row
        //            posInB += stepX;
        //        }

        //        // Move to the next row
        //        yPosInB += stepY;
        //    }

        //    // No intersection found
        //    return false;
        //}

        public virtual void OnCollision(GameObject gameObject)
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
