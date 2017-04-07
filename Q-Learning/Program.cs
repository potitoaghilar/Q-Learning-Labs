using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace Q_Learning
{
    class Program : GameWindow
    {
        static void Main(string[] args)
        {
            Game game = new Game(800, 600);
            game.Run();

            //game.drawLabWalls(0, 0, 1, 1, Color.Black);
        }
    }
}
