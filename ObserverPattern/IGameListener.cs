using System;

namespace SystemShutdown.ObserverPattern
{
    // Lead author: Frederik
    public interface IGameListener
    {
        void Notify(GameEvent gameEvent, Component component);
    }
}
