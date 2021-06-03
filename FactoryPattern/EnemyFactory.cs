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

        public override GameObject1 Create(Vector2 position, string type)
        {

            GameObject1 enemyGO = new GameObject1();
            GameWorld.Instance.gameState.aliveEnemies++;

            switch (type)
            {
                case "Bug":
                    SpriteRenderer enemyBugSR = new SpriteRenderer("Textures/enemy");
                    enemyGO.Transform.Position = position;
                    enemyGO.AddComponent(enemyBugSR);
                    enemyBugSR.Origin = new Vector2(enemyBugSR.Sprite.Width / 2, (enemyBugSR.Sprite.Height) / 2);
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(enemyBugSR, enemy) { CheckCollisionEvents = true });
                    enemyGO.AddComponent(enemy);


                    break;
                case "Trojan":
                    SpriteRenderer enemyTrojanSR = new SpriteRenderer("Textures/trojan");
                    enemyGO.Transform.Position = position;
                    enemyGO.AddComponent(enemyTrojanSR);
                    enemyTrojanSR.Origin = new Vector2(enemyTrojanSR.Sprite.Width / 2, (enemyTrojanSR.Sprite.Height) / 2);
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(enemyTrojanSR, enemy) { CheckCollisionEvents = true });
                    enemyGO.AddComponent(enemy);
                    enemy.isTrojan = true;
                    break;
            }
            return enemyGO;
        }
    }
}

