using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.FactoryPattern
{
    public class EnemyFactory : Factory
    {

        private static Random rnd = new Random();
        private static EnemyFactory instance;
        private Enemy enemy;
        public SpriteRenderer sr;
        private Vector2 distance;

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

        public Enemy Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }

        public override GameObject1 Create(Vector2 position)
        {

            GameObject1 enemyGO = new GameObject1();
                SpriteRenderer enemySR = new SpriteRenderer("Textures/enemy");
                enemyGO.AddComponent(enemySR);
            enemyGO.Transform.Position = position;
            enemySR.Origin = new Vector2(enemySR.Sprite.Width / 2, (enemySR.Sprite.Height) / 2);

            enemy = new Enemy();
            enemyGO.AddComponent(new Collider(enemySR, enemy) { CheckCollisionEvents = true });
            enemyGO.AddComponent(enemy);


            //switch (type)
            //{
            //    case "Blue":
            //enemy = new Enemy();
            //enemyGO.AddComponent(new Collider(sr, enemy) { CheckCollisionEvents = true });
            //enemyGO.AddComponent(enemy);

            //distance.X = enemy.goal.x;
            //distance.Y = enemy.goal.y;

            //sr.Rotation = (float)Math.Atan2(distance.Y, distance.X);
            //        break;
            //    case "Black":
            //enemy = new Enemy();
            //        enemyGO.AddComponent(new Collider(sr, enemy) { CheckCollisionEvents = true });
            //        enemyGO.AddComponent(enemy);
            //        break;
            //}
            return enemyGO;
        }
    }
}

