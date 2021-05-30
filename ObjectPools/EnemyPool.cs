using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.AStar;
using SystemShutdown.ComponentPattern;
using SystemShutdown.FactoryPattern;

namespace SystemShutdown.ObjectPool
{
    class EnemyPool : ObjectPool
    {
        Node enemypos;
        private static EnemyPool instance;
        public static EnemyPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyPool();
                }
                return instance;
            }
        }
        private Node GetRandomPassableNode()
        {
            Random rndd = new Random();
            var tmppos = GameWorld.gameState.grid.nodes[rndd.Next(1, GameWorld.gameState.grid.Width), rndd.Next(1, GameWorld.gameState.grid.Height)];

            return tmppos;
        }


        public void SetEnemyPosition()
        {
          
            enemypos = GetRandomPassableNode();

            while (!enemypos.Passable || enemypos == null)
            {
                enemypos = GetRandomPassableNode();
            }

        }
        protected override void Cleanup(GameObject1 gameObject)
        {
            throw new NotImplementedException();
        }

        protected override GameObject1 Create()
        {
            SetEnemyPosition();
            return EnemyFactory.Instance.Create(new Vector2(enemypos.x * 100, enemypos.y * 100));

        }
    }
}
