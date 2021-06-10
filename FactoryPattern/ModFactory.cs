using Microsoft.Xna.Framework;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;

namespace SystemShutdown.FactoryPattern
{
    // Lead author: Ras
    // Contributor: Lau
    class ModFactory : Factory
    {
        #region Fields
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
        #endregion

        #region Methods

        /// <summary>
        /// Creates a gameobject and adds 3 component to it. A Mod, a SpriteRenderer and a Collider.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GameObject Create(Vector2 position, string type)
        {
            GameObject modGO = new GameObject();
            SpriteRenderer modSR = new SpriteRenderer("Textures/pick");
            modGO.AddComponent(modSR);
            mods = new Mods();
            modGO.AddComponent(new Collider(modSR, mods) { CheckCollisionEvents = true });
            modGO.Transform.Position = position;
            modGO.AddComponent(mods);
            return modGO;
        }
        #endregion
    }
}
