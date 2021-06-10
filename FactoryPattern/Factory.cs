using Microsoft.Xna.Framework;
using System;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.FactoryPattern
{
    // Lead author: Frederik
    public abstract class Factory
    {
        public abstract GameObject Create(Vector2 position, string type);
    }
}
