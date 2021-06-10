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


        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile projectile, string sprite, float speed)
        {
            projectile = new Projectile(speed);
            spriteRenderer = new SpriteRenderer(sprite);

        }
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
