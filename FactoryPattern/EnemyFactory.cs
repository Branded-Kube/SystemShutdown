using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public SpriteRenderer enemyBug = new SpriteRenderer("1enemy");
        private Vector2 distance;

        //public Texture2D[] walk;
        //public float fps;
        //public float timeElapsed;
        //public int currentIndex;
        //public bool isMoving = false;

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
                    SpriteRenderer enemyBugSR = new SpriteRenderer("1enemy");
                    enemyBug = enemyBugSR;
                    enemyGO.Transform.Position = position;
                    enemyGO.AddComponent(enemyBugSR);
                    enemyBugSR.Origin = new Vector2(enemyBugSR.Sprite.Width / 2, (enemyBugSR.Sprite.Height) / 2);
                    enemy = new Enemy();
                    enemyGO.AddComponent(new Collider(enemyBugSR, enemy) { CheckCollisionEvents = false });
                    enemyGO.AddComponent(enemy);

                    ////Load sprite sheet
                    //walk = new Texture2D[3];

                    ////Loop animaiton
                    //for (int g = 0; g < walk.Length; g++)
                    //{
                    //    walk[g] = GameWorld.Instance.content.Load<Texture2D>(g + 1 + "enemy");
                    //}
                    ////When loop is finished return to first sprite/Sets default sprite
                    //enemyBugSR.Sprite = enemyBugSR.Sprite/*upWalk[0]*/;


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

