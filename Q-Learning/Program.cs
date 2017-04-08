using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using System.Threading;

namespace Q_Learning
{
    class Program : GameWindow
    {

        static Game game;
        private static Random random = new Random();

        static void Main(string[] args)
        {
            game = new Game(800, 600);

            Thread asyncWorker = new Thread(new ThreadStart(asyncOperations));
            asyncWorker.Start();
            
            game.Run();
        }

        public static void asyncOperations() {

            // Rectangle of the workspace
            int x = 0, y = 0, width = 20, height = 15;

            // Draw lab
            game.drawLabWalls(x, y, width, height, Color.Black);
            // Spawn agent in random position inside Lab
            game.createAgent(random.Next(x + 1, width - 1), random.Next(y + 1, height - 1), .4, true);

        }
    }
}
