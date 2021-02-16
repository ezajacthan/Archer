using System;

namespace Archer
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ArcherGame())
                game.Run();
        }
    }
}
