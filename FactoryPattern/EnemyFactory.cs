using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.FactoryPattern
{
    class EnemyFactory : Factory
    {
 
            private static Random rnd = new Random();
            private static EnemyFactory instance;
            private Enemy enemy;

        public static EnemyFactory Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new EnemyFactory();
                    }
                    return instance;
                }
            }
            public override GameObject1 Create(string type)
            {

            GameObject1 enemyGO = new GameObject1();
                SpriteRenderer sr = new SpriteRenderer("1GuyUp");
                enemyGO.AddComponent(sr);
            //go.Transform.Position = new Vector2(rnd.Next(0, GameWorld.Instance.GraphicsDevice.Viewport.Width), 0);
            //enemyGO.Transform.Position = new Vector2(GameWorld.graphics.GraphicsDevice.Viewport.Width / 2, GameWorld.graphics.GraphicsDevice.Viewport.Height / 2);



            switch (type)
                {
                    case "Blue":
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(sr, enemy) { CheckCollisionEvents = true });
                    enemyGO.AddComponent(enemy);
                    break;
                    case "Black":
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(sr, enemy) { CheckCollisionEvents = true });
                    enemyGO.AddComponent(enemy);
                    break;
                }
                return enemyGO;
            }
        }
    }

