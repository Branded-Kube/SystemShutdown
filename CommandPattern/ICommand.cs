using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    interface ICommand
    {
        void Execute(Player player);
        void Execute(GameState player);
    }
}
