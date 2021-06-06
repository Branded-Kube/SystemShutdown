using Microsoft.Xna.Framework;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;

namespace SystemShutdown.FactoryPattern
{
   // Ras
    public class EnemyFactory : Factory
    {
        private static EnemyFactory instance;
        private Enemy enemy;
        // EnemyFactoy Singleton
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

        /// <summary>
        /// Creates a gameobject and adds 3 component to it. A Enemy, a SpriteRenderer and a Collider.
        /// Adds a number to aliveEnemies
        /// 2 Types of enemy´s can be created. a Bug or a Trojan. IsTrojan bool is default false and is sat to true if a Trojan is created
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GameObject1 Create(Vector2 position, string type)
        {

            GameObject1 enemyGO = new GameObject1();
            GameWorld.Instance.gameState.aliveEnemies++;

            switch (type)
            {
                case "Bug":
                    SpriteRenderer enemyBugSR = new SpriteRenderer("1enemy");
                    enemyGO.Transform.Position = position;
                    enemyGO.AddComponent(enemyBugSR);
                    enemyBugSR.Origin = new Vector2(enemyBugSR.Sprite.Width / 2, (enemyBugSR.Sprite.Height) / 2);
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(enemyBugSR, enemy) { CheckCollisionEvents = false });
                    enemyGO.AddComponent(enemy);

                    break;
                case "Trojan":
                    SpriteRenderer enemyTrojanSR = new SpriteRenderer("Textures/trojan");
                    enemyGO.Transform.Position = position;
                    enemyGO.AddComponent(enemyTrojanSR);
                    enemyTrojanSR.Origin = new Vector2(enemyTrojanSR.Sprite.Width / 2, (enemyTrojanSR.Sprite.Height) / 2);
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(enemyTrojanSR, enemy) { CheckCollisionEvents = false });
                    enemyGO.AddComponent(enemy);
                    enemy.IsTrojan = true;
                    break;
            }
            return enemyGO;
        }

        
    }
}

