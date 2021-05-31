using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.Components
{
    public class Collider : Component
    {
        public bool CheckCollisionEvents { get; set; }

        private GameEvent onCollisionEvent = new GameEvent("Collision");

        private Vector2 size;
        private Vector2 origin;
        //public Vector2 velocity = new Vector2(0f, 0f);
        public Input input;
        public float speed;

        private Player player;

        //private Texture2D texture;
        public Texture2D _texture;
        private SpriteRenderer sr;
        private Mods floormod;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                    (
                        (int)(GameObject.Transform.Position.X - origin.X),
                        (int)(GameObject.Transform.Position.Y - origin.Y),
                        (int)size.X,
                        (int)size.Y
                    );
            }
        }

        public Rectangle CollisionRec
        {
            get
            {
                return new Rectangle((int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Collider(SpriteRenderer spriteRenderer/*, Texture2D texture*/, IGameListener gameListener)
        {
            onCollisionEvent.Attach(gameListener);
            this.origin = spriteRenderer.Origin;
            this.size = new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
            _texture = GameWorld.content.Load<Texture2D>("Textures/CollisionBox");
            //_texture = texture;
        }

        //public Collider(Texture2D texture)
        //{
        //    _texture = texture;
        //}

        public void OnCollisionEnter(Collider other)
        {
            if (CheckCollisionEvents)
            {
                if (other != this)
                {
                    if (CollisionBox.Intersects(other.CollisionBox))
                    {
                        onCollisionEvent.Notify(other);
                    }
                }
            }
        }

        public override void Destroy()
        {
           // base.Destroy();
           GameWorld.gameState.Colliders.Remove(this);
        }

        //public virtual void Update(GameTime gameTime, List<Collider> colliders)
        //{
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, CollisionBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public override string ToString()
        {
            return "Collider";
        }

        #region Collision

        //Is touching left
        public bool IsTouchingLeft(Collider collider)
        {
            return this.CollisionRec.Right + player.velocity.X > collider.CollisionRec.Left &&
                         this.CollisionRec.Left < collider.CollisionRec.Left &&
                         this.CollisionRec.Bottom > collider.CollisionRec.Top &&
                         this.CollisionRec.Top < collider.CollisionRec.Bottom;
        }

        //Is touching Right
        public bool IsTouchingRight(Collider collider)
        {
            return this.CollisionRec.Left + player.velocity.X < collider.CollisionRec.Right &&
                         this.CollisionRec.Right > collider.CollisionRec.Right &&
                         this.CollisionRec.Bottom > collider.CollisionRec.Top &&
                         this.CollisionRec.Top < collider.CollisionRec.Bottom;
        }

        //Is touching top
        public bool IsTouchingTop(Collider collider)
        {
            return this.CollisionRec.Bottom + player.velocity.Y > collider.CollisionRec.Top &&
                         this.CollisionRec.Top < collider.CollisionRec.Top &&
                         this.CollisionRec.Right > collider.CollisionRec.Left &&
                         this.CollisionRec.Left < collider.CollisionRec.Right;
        }

        //Is touching bottom
        public bool IsTouchingBottom(Collider collider)
        {
            return this.CollisionRec.Top + player.velocity.Y < collider.CollisionRec.Bottom &&
                         this.CollisionRec.Bottom > collider.CollisionRec.Bottom &&
                         this.CollisionRec.Right > collider.CollisionRec.Left &&
                         this.CollisionRec.Left < collider.CollisionRec.Right;
        }

        #endregion
    }
}
