using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
        private Projectile playerLaser;
        //private SpriteRenderer playerRenderer;
        //private SpriteRenderer enemyRenderer;
        private LaserFactory()
        {
            CreatePrototype(ref playerRenderer, ref playerLaser, "Laser", 500, new Vector2(0, -1));
        }
        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile laser, string sprite, float speed, Vector2 velocity)
        {
            laser = new Projectile(speed, velocity);
            //spriteRenderer = new SpriteRenderer(sprite);

        }
        public override GameObject Create(string type)
        {
            GameObject go = new GameObject();
            switch (type)
            {
                case "Player":
                    Projectile laserClone = playerLaser.Clone();
                    go.AddComponent(new Collider(playerRenderer, laserClone) { CheckCollisionEvents = true });
                    go.AddComponent(laserClone);
                    go.AddComponent(playerRenderer.Clone());
                    break;
            }
            return go;

        }
    }
}
