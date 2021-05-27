using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;

namespace SystemShutdown.FactoryPattern
{
    class LaserFactory2 : Factory
    {
        private static LaserFactory2 instance;
        public static LaserFactory2 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LaserFactory2();
                }
                return instance;
            }
        }
        private Projectile2 playerLaser;
        private SpriteRenderer playerRenderer;
        private SpriteRenderer enemyRenderer;
        private LaserFactory2()
        {


            CreatePrototype(ref playerRenderer, ref playerLaser, "laserBlue05", 500);
        }
        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile2 laser, string sprite, float speed)
        {
            laser = new Projectile2(speed);
            spriteRenderer = new SpriteRenderer(sprite);

            laser.Height = spriteRenderer.Sprite.Height;
        }
        public override GameObject1 Create(string type)
        {
            GameObject1 go = new GameObject1();
           

            switch (type)
            {
                case "Player":
                    Projectile2 laserClone = playerLaser.Clone();
                    go.AddComponent(new Collider(playerRenderer, laserClone) { CheckCollisionEvents = true });
                    go.AddComponent(laserClone);
                    go.AddComponent(playerRenderer.Clone());
                    break;
            }
            return go;

        }
    }
}
