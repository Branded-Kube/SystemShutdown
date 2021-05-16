using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.FactoryPattern
{
    class LaserFactory : Factory
    {
        private static LaserFactory instance;
        public static LaserFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LaserFactory();
                }
                return instance;
            }
        }
        private Projectile1 playerProjectile;
        private SpriteRenderer playerRenderer;
        //private SpriteRenderer enemyRenderer;

        private LaserFactory()
        {
            CreatePrototype(ref playerRenderer, ref playerProjectile, "Laser", 500, new Vector2(0, -1));
        }
        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile1 laser, string sprite, float speed, Vector2 velocity)
        {
            //laser = new Projectile1(speed, velocity);
            spriteRenderer = new SpriteRenderer(sprite);

        }
        public override GameObject1 Create(string type)
        {
            GameObject1 go = new GameObject1();
            switch (type)
            {
                case "Player":
                    Projectile1 laserClone = playerProjectile.Clone();
                    go.AddComponent(new Collider(playerRenderer, laserClone) { CheckCollisionEvents = true });
                    go.AddComponent(laserClone);
                    go.AddComponent(playerRenderer.Clone());
                    break;
            }
            return go;

        }
    }
}
