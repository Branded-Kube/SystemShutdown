﻿using Microsoft.Xna.Framework;
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
        protected Texture2D _texture;

        public Vector2 origin;

        protected float rotation;

        public Vector2 position;

        protected float timePassed;

        protected Vector2 velocity;
        protected float rotationVelocity = 3f;
        protected float friction = 0.1f;
        #endregion

        #region Properties

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
        public GameObject(Texture2D texture)
        {
            _texture = texture;

            // Default origin in the centre of the sprite
            origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            Colour = Color.White;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Frederik
            spriteBatch.Draw(_texture, position, null, Colour, rotation, origin, 1, SpriteEffects.None, 0);
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