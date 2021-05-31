using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.AStar;
using SystemShutdown.ComponentPattern;
//using SystemShutdown.ComponentPattern;

namespace SystemShutdown
{
    public abstract class Component
    {
        //public Texture2D sprite;
        //protected Texture2D[] sprites, upWalk;
        //protected float fps;
        //private float timeElapsed;
        //private int currentIndex;

        //protected Texture2D _texture;
        //public Vector2 _position;
        //public Vector2 _velocity = new Vector2(0f, 0f);
        //public Color _color = Color.White;
        //public float _speed;

        //public Vector2 position;
        //public Rectangle rectangle;
        public Vector2 previousPosition;
        public Vector2 currentDir;
        protected float rotation;
       // protected Vector2 velocity;
        
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

        public Node node;

        public Node Node
        {
            get { return node; }
            set { node = value; }
        }

        public bool IsEnabled { get; set; } = true;

        public GameObject1 GameObject { get; set; }

        //public Rectangle _rectangle
        //{
        //    get
        //    {
        //        return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        //    }
        //}

        public Component(/*Texture2D texture*/)
        {
            //_texture = texture;
        }

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update(GameTime gameTime/*, List<Component> colliders*/)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, _position, _color);
        }

        public virtual void Destroy()
        {
        }

        //#region Collision

        ////Is touching left
        //protected bool IsTouchingLeft(Component collider)
        //{
        //    return this._rectangle.Right + this._velocity.X > collider._rectangle.Left &&
        //                 this._rectangle.Left < collider._rectangle.Left &&
        //                 this._rectangle.Bottom > collider._rectangle.Top &&
        //                 this._rectangle.Top < collider._rectangle.Bottom;
        //}

        ////Is touching Right
        //protected bool IsTouchingRight(Component collider)
        //{
        //    return this._rectangle.Left + this._velocity.X < collider._rectangle.Right &&
        //                 this._rectangle.Right > collider._rectangle.Right &&
        //                 this._rectangle.Bottom > collider._rectangle.Top &&
        //                 this._rectangle.Top < collider._rectangle.Bottom;
        //}

        ////Is touching top
        //protected bool IsTouchingTop(Component collider)
        //{
        //    return this._rectangle.Bottom + this._velocity.Y > collider._rectangle.Top &&
        //                 this._rectangle.Top < collider._rectangle.Top &&
        //                 this._rectangle.Right > collider._rectangle.Left &&
        //                 this._rectangle.Left < collider._rectangle.Right;
        //}

        ////Is touching bottom
        //protected bool IsTouchingBottom(Component collider)
        //{
        //    return this._rectangle.Top + this._velocity.Y < collider._rectangle.Bottom &&
        //                 this._rectangle.Bottom > collider._rectangle.Bottom &&
        //                 this._rectangle.Right > collider._rectangle.Left &&
        //                 this._rectangle.Left < collider._rectangle.Right;
        //}

        //#endregion

        //protected void Animate(GameTime gametime)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
        //    {
        //        //Giver tiden, der er gået, siden sidste update
        //        timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

        //        //Beregner currentIndex
        //        currentIndex = (int)(timeElapsed * fps);
        //        sprite = upWalk[currentIndex];

        //        //Checks if animation needs to restart
        //        if (currentIndex >= upWalk.Length - 1)
        //        {
        //            //Resets animation
        //            timeElapsed = 0;
        //            currentIndex = 0;
        //        }
        //    }
        //}
    }
}
