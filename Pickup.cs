using SystemShutdown.ObserverPattern;

namespace SystemShutdown
{
    // Lead author: Lau
    class Pickup : Component, IGameListener
    {
        private Mods floormod;
        private static Pickup instance;


        public static Pickup Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Pickup();
                }
                return instance;
            }
        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            ((IGameListener)floormod).Notify(gameEvent, component);
        }

    }
}
