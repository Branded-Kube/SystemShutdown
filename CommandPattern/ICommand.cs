using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;

namespace SystemShutdown
{
    interface ICommand
    {
        void Execute(Player player);
    }
}
