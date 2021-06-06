using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.ObjectPool
{
    abstract class ObjectPool
    {
        protected List<GameObject1> active = new List<GameObject1>();
        protected Stack<GameObject1> inactive = new Stack<GameObject1>();

        public GameObject1 GetObject(Vector2 position, string type)
        {
            GameWorld.Instance.gameState.aliveEnemies++;

            GameObject1 go;

            if (inactive.Count > 0)
            {
                go = inactive.Pop();
            }
            else
            {
                go = Create(position, type);
            }
            return go;
        }
        public void RealeaseObject(GameObject1 gameObject)
        {
            GameWorld.Instance.gameState.RemoveGameObject(gameObject);
            active.Remove(gameObject);
            inactive.Push(gameObject);
        }
        protected abstract GameObject1 Create(Vector2 position, string type);
        protected abstract void Cleanup(GameObject1 gameObject);


    }
}
