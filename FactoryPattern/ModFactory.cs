using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;

namespace SystemShutdown.FactoryPattern
{
    // Ras
    class ModFactory : Factory
    {

        public int Id { get; set; }

        public string Name { get; set; }
        private static ModFactory instance;
        private Mods mods;
        public static ModFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModFactory();
                }
                return instance;
            }
        }
        public override GameObject1 Create(Vector2 position, string type)
        {
            GameObject1 go = new GameObject1();
            SpriteRenderer sr = new SpriteRenderer("laserBlue05");
            go.AddComponent(sr);


            //sr.SetSprite("1GuyUp");
            mods = new Mods();
            go.AddComponent(new Collider(sr, mods) { CheckCollisionEvents = true });
            go.Transform.Position = position;
            go.AddComponent(mods);
            GameWorld.Instance.gameState.AddGameObject(go);
            return go;
        }
    }
}
