using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemShutdown.GameObjects
{
    public class GameObject : Component, ICloneable
    {
        #region Fields
        //protected Texture2D _texture;
        public Texture2D sprite;


        public Vector2 origin;

        protected float rotation;

        public Vector2 position;

        protected float timePassed;

        protected Vector2 velocity;
        protected float rotationVelocity = 3f;
        protected float friction = 0.1f;
        #endregion

        #region Properties
        public Transform Transform { get; private set; }
        public int Health { get; set; }
        public Color Colour { get; set; }

        protected float _Layer { get; set; }

        // Frederik
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

        // Frederik
        public GameObject()
        {
            //_texture = texture;

            // Default origin in the centre of the sprite
            //origin = new Vector2(sprite.Width / 2, sprite.Height / 2);

            Colour = Color.White;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Draw(sprite, position, Colour);
        }

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
