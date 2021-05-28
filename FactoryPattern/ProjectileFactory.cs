﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.FactoryPattern;

namespace SystemShutdown.FactoryPattern
{
    class ProjectileFactory : Factory
    {
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
        private Projectile playerProjectile;
        private SpriteRenderer projectileSR;
        private ProjectileFactory()
        {


            CreatePrototype(ref projectileSR, ref playerProjectile, "laserBlue05", 500);
        }
        //laserBlue05
        private void CreatePrototype(ref SpriteRenderer spriteRenderer, ref Projectile projectile, string sprite, float speed)
        {
            projectile = new Projectile(speed);
            spriteRenderer = new SpriteRenderer(sprite);

        }
        public override GameObject1 Create(string type)
        {
            GameObject1 projectileGO = new GameObject1();
           

            switch (type)
            {
                case "Player":
                    Projectile projectileClone = playerProjectile.Clone();
                    projectileSR.Origin = new Vector2(projectileSR.Sprite.Width / 2, (projectileSR.Sprite.Height) / 2);

                    projectileGO.AddComponent(new Collider(projectileSR, projectileClone) { CheckCollisionEvents = true });
                    projectileGO.AddComponent(projectileClone);
                    projectileGO.AddComponent(projectileSR.Clone());
                    break;
            }
            return projectileGO;

        }
    }
}