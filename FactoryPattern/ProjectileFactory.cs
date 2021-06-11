using Microsoft.Xna.Framework;
using System;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;

namespace SystemShutdown.FactoryPattern
{
    // Lead author: Frederik
    // Contributor: Lau
    // Contributor: Ras

    class ProjectileFactory : Factory
    {
        #region Fields
        private Projectile playerProjectile;
        private SpriteRenderer projectileSR;
        private static ProjectileFactory instance;
        public static ProjectileFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectileFactory();
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        private ProjectileFactory()
        {
            CreatePrototype(ref projectileSR, ref playerProjectile, "Textures/4bit", 600);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Ref to projectile speed and spriterenderer to create prototype of projectile
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="projectile"></param>
        /// <param name="sprite"></param>
        /// <param name="speed"></param>
        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile projectile, string sprite, float speed)
        {
            projectile = new Projectile(speed);
            spriteRenderer = new SpriteRenderer(sprite);

        }

        /// <summary>
        /// Creates protoype of projectile
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GameObject Create(Vector2 position, string type)
        {
            GameObject projectileGO = new GameObject();
            projectileGO.Transform.Position = position;

            Projectile projectileClone = playerProjectile.Clone();
            projectileSR.Origin = new Vector2(projectileSR.Sprite.Width / 2, (projectileSR.Sprite.Height) / 2);

            projectileGO.AddComponent(new Collider(projectileSR, projectileClone) { CheckCollisionEvents = true });
            projectileGO.AddComponent(projectileClone);
            projectileGO.AddComponent(projectileSR.Clone());
            return projectileGO;

        }
        #endregion
    }
}
