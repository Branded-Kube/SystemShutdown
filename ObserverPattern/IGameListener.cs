using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.Components;

namespace SystemShutdown.ObserverPattern
{
    public interface IGameListener
    {
        void Notify(GameEvent gameEvent, Component component);
    }
}
