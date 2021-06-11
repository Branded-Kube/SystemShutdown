using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown.FactoryPattern
{
    // Lead author: Frederik
    // Contributor: Ras
    class Projectile : Component, IGameListener
    {
        #region Fields
        private float speed;
        private Vector2 velocity;
        private bool alreadyCollided = false;
        public Texture2D[] projectiles;
        public float fps;
        public float timeElapsed;
        public int currentIndex;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        #endregion

        #region Constructor
        public Projectile(float speed)
        {
            this.speed = speed;
            fps = 5;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return "Projectile";
        }

        public override void Awake()
        {
            GameObject.Tag = "Projectile";
            alreadyCollided = false;

            //Load sprite sheet - Frederik
            projectiles = new Texture2D[7];
            //Loop animaiton textures
            for (int g = 0; g < projectiles.Length; g++)
            {
                projectiles[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "bit");
            }
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            Animate(gameTime);
        }
        private void Move()
        {
            GameObject.Transform.Translate(velocity * speed * GameWorld.Instance.DeltaTime);
        }

        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
        }

        /// <summary>
        /// Collision with Node:
        /// Destroys itself on impact with a Node object (wall) or enemy
        /// Collision with Enemy:
        /// Damages enemys health with players dmg. Bool AlreadyCollided causes projektile to only hit 1 enemy at a time. 
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Node")
            {
                GameWorld.Instance.GameState.Effects.Add(new ProjectileEffect(new Vector2(GameObject.Transform.Position.X - 50, GameObject.Transform.Position.Y - 50)));
                GameObject.Destroy();
            }
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy" && !alreadyCollided)
            {
                GameWorld.Instance.GameState.Effects.Add(new ProjectileEffect(new Vector2(GameObject.Transform.Position.X - 50, GameObject.Transform.Position.Y - 50)));
                GameObject.Destroy();
                component.GameObject.GetComponent("Enemy").Health -= GameWorld.Instance.GameState.PlayerBuilder.Player.dmg;
                alreadyCollided = true;
            }
        }

        /// <summary>
        /// Animate projectile - Frederik
        /// </summary>
        /// <param name="gametime"></param>
        public void Animate(GameTime gametime)
        {
            // Gives time that has passed since last update
            timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            // Calculates currentIndex
            currentIndex = (int)(timeElapsed * fps);
            var tmpSpriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            tmpSpriteRenderer.Sprite = projectiles[currentIndex];

            // Checks if animation needs to restart
            if (currentIndex >= projectiles.Length - 1)
            {
                // Resets animation
                timeElapsed = 0;
                currentIndex = 0;
            }
        }
        #endregion
    }
}
