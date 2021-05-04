using System;

namespace SystemShutdown
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameWorld())
                game.Run();
        }
    }
}
