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

        public GameObject1 GetObject()
        {
            GameObject1 go;
            if (inactive.Count > 0)
            {
                go = inactive.Pop();
            }
            else
            {
                go = Create();
            }
            return go;
        }

        public void ReleaseObject(GameObject1 gameObject)
        {
            GameWorld.Instance.RemoveGameObject(gameObject);
            active.Remove(gameObject);
            inac
        }
    }
}
