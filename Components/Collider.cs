using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.Components
{
    // Lead author: Frederik 
    // Contributor: Ras
    public class Collider : Component
    {
        #region Fields
        public bool CheckCollisionEvents { get; set; }

        private GameEvent onCollisionEvent = new GameEvent("Collision");

        private Vector2 size;
        private Vector2 origin;
        public Texture2D _texture;
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
        #endregion

        #region Constructor
        public Collider(SpriteRenderer spriteRenderer, IGameListener gameListener)
        {
            onCollisionEvent.Attach(gameListener);
            this.origin = spriteRenderer.Origin;
            this.size = new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
            _texture = GameWorld.Instance.Content.Load<Texture2D>("Textures/CollisionBox");
        }
        #endregion

        #region Methods

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
            GameWorld.Instance.GameState.Colliders.Remove(this);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.Draw(_texture, CollisionBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
#endif
        }

        public override string ToString()
        {
            return "Collider";
        }


        //Is touching left
        public bool IsTouchingLeft(Collider collider)
        {
            return this.CollisionBox.Right + GameWorld.Instance.GameState.PlayerBuilder.Player.velocity.X > collider.CollisionBox.Left &&
                         this.CollisionBox.Left < collider.CollisionBox.Left &&
                         this.CollisionBox.Bottom > collider.CollisionBox.Top &&
                         this.CollisionBox.Top < collider.CollisionBox.Bottom;
        }

        //Is touching Right
        public bool IsTouchingRight(Collider collider)
        {
            return this.CollisionBox.Left + GameWorld.Instance.GameState.PlayerBuilder.Player.velocity.X < collider.CollisionBox.Right &&
                         this.CollisionBox.Right > collider.CollisionBox.Right &&
                         this.CollisionBox.Bottom > collider.CollisionBox.Top &&
                         this.CollisionBox.Top < collider.CollisionBox.Bottom;
        }

        //Is touching top
        public bool IsTouchingTop(Collider collider)
        {
            return this.CollisionBox.Bottom + GameWorld.Instance.GameState.PlayerBuilder.Player.velocity.Y > collider.CollisionBox.Top &&
                         this.CollisionBox.Top < collider.CollisionBox.Top &&
                         this.CollisionBox.Right > collider.CollisionBox.Left &&
                         this.CollisionBox.Left < collider.CollisionBox.Right;
        }

        //Is touching bottom
        public bool IsTouchingBottom(Collider collider)
        {
            return this.CollisionBox.Top + GameWorld.Instance.GameState.PlayerBuilder.Player.velocity.Y < collider.CollisionBox.Bottom &&
                         this.CollisionBox.Bottom > collider.CollisionBox.Bottom &&
                         this.CollisionBox.Right > collider.CollisionBox.Left &&
                         this.CollisionBox.Left < collider.CollisionBox.Right;
        }
        #endregion
    }
}
