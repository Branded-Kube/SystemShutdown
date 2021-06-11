using System;
using System.Collections.Generic;

namespace SystemShutdown.ObserverPattern
{
    // Lead author: Frederik
    public class GameEvent
    {
        private List<IGameListener> listeners = new List<IGameListener>();

        public string Title { get; private set; }

        public GameEvent(string title)
        {
            this.Title = title;
        }
        public void Attach(IGameListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Nofity other objects - Used for collision detection
        /// </summary>
        /// <param name="other"></param>
        public void Notify(Component other)
        {
            foreach (IGameListener listener in listeners)
            {
                listener.Notify(this, other);
            }
        }
    }
}
