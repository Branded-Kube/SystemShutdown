using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;

namespace SystemShutdown.FactoryPattern
{
    public abstract class Factory
    {
        public abstract GameObject Create(string type);

    }
}
