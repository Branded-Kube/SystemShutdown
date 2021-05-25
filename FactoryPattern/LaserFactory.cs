using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private Player1 player;
        private Vector2 vector = Vector2.Zero;
        //private SpriteRenderer enemyRenderer;

        private LaserFactory(/*Player1 player*/)
        {
            //ShootUp(player);
            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //{
            CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(0, -1));
            //CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(0, 1));
            //CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(-1, 0));
            //CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(1, 0));
            //}
            //if (playerProjectile.currentDir.Y == -1)
            //{
            //    CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(0, -1));
            //}
            //else if (player.currentDir.Y == 1)
            //{
            //    CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(0, 1));
            //}

        }

        //private void ShootUp(Player1 player)
        //{
        //    if (player.currentDir.Y == -1)
        //    {
        //        CreatePrototype(ref playerRenderer, ref playerProjectile, "laserBlue05", 100, new Vector2(0, -1));
        //    }
        //}

        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile1 laser, string sprite, float speed, Vector2 velocity)
        {
            laser = new Projectile1(speed, velocity);
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
