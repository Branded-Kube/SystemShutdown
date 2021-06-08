using Microsoft.Xna.Framework;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;

namespace SystemShutdown.FactoryPattern
{
    // Ras
    class ModFactory : Factory
    {
        private static ModFactory instance;
        private Mods mods;
        // Modfactory Singleton
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
        /// <summary>
        /// Creates a gameobject and adds 3 component to it. A Mod, a SpriteRenderer and a Collider.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GameObject1 Create(Vector2 position, string type)
        {
            GameObject1 modGO = new GameObject1();
            SpriteRenderer modSR = new SpriteRenderer("Textures/pick");
            modGO.AddComponent(modSR);
            mods = new Mods();
            modGO.AddComponent(new Collider(modSR, mods) { CheckCollisionEvents = true });
            modGO.Transform.Position = position;
            modGO.AddComponent(mods);
            return modGO;
        }
    }
}
