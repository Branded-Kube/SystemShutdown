using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;

namespace SystemShutdown.FactoryPattern
{
    // Hovedforfatter: Frederik
    public abstract class Factory
    {
        public abstract GameObject Create(Vector2 position, string type);

    }
}
