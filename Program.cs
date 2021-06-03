using System;
using SystemShutdown.AStar;

namespace SystemShutdown
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game =  GameWorld.Instance)
                game.Run();
        }
    }
}
