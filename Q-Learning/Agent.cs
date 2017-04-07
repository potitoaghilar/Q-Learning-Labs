using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q_Learning
{
    class Agent
    {
        protected double radius, position_x, position_y, learning_rate;
        protected Matrix Q;
        protected Game game;
        protected LabSpace lab;
        protected Thread brain;
        private Random random = new Random();

        public Agent(int position_x, int position_y, double learning_rate, LabSpace lab, Game game, double radius = .25) {
            this.position_x = position_x;
            this.position_y = position_y;
            this.radius = radius;

            // Set learning rate
            if (learning_rate < 0) learning_rate = 0;
            if (learning_rate > .99) learning_rate = .99;
            this.learning_rate = learning_rate;

            // Set game instance
            this.game = game;

            // Set lab in which agent have to learn
            this.lab = lab;

            // Start Q matrix as zero matrix
            Q = new Matrix(game.getR().getLength());

            // Start async brain operations
            brain = new Thread(new ThreadStart(brainWorker));
            brain.Start();
        }

        public void setRadius(double radius) {
            this.radius = radius;
        }
        public double getRadius() {
            return radius;
        }

        public void setPositionX(int position_x)
        {
            this.position_x = position_x;
        }
        public double getPositionX()
        {
            return position_x;
        }

        public void setPositionY(int position_y)
        {
            this.position_y = position_y;
        }
        public double getPositionY()
        {
            return position_y;
        }

        protected void brainWorker() {

            bool brainActive = true;
            while (brainActive) {

                // Get current pos
                int currY = lab.height - (int)position_y - 1, currX = (int)position_x - lab.x - 1;

                // TODO
                int selectedAction = random.Next(4);
                Array actionsArray = Enum.GetValues(typeof(actions));
                actions action = (actions)actionsArray.GetValue(selectedAction);
                GetType().GetMethod(action.ToString()).Invoke(this, null);

                // Get next pos
                int nextY = (lab.height - (int)position_y - 1), nextX = (int)position_x - lab.x - 1;

                // Angent changed position -> update Q
                if (currY != nextY || currX != nextX) {

                    // Calculate matrix row
                    int row = 0;
                    for (int i = 0; i < currY; i++)
                        row += lab.width - 1;
                    row += currX;
                    // Calculate matrix column
                    int column = 0;
                    for (int i = 0; i < nextY; i++)
                        column += lab.width - 1;
                    column += nextX;
                    // Update Q
                    Q.setValue(row, column, game.getR().getValue(row, column) + learning_rate * Q.getMaxFromRow(column));

                }

                Thread.Sleep(100);

                // If agent reaches the goal stop brain
                if (position_x == lab.x + lab.width - 1 && position_y == lab.y + lab.height - 1)
                {
                    game.newGeneration();
                    brainActive = false;
                }
            }

        }

        // Agent possible actions enum
        protected enum actions {
            moveUp,
            moveDown,
            moveLeft,
            moveRight
        }
        // Agent possible actions methods
        public void moveUp()
        {
            if (position_y < lab.y + lab.height - 1)
                position_y++;
        }
        public void moveDown()
        {
            if (position_y > lab.y + 1)
                position_y--;
        }
        public void moveLeft()
        {
            if (position_x > lab.x + 1)
                position_x--;
        }
        public void moveRight()
        {
            if (position_x < lab.x + lab.width - 1)
                position_x++;
        }

        public void stopBrain() {
            brain.Abort();
        }

        public void setQ(Matrix Q)
        {
            this.Q = Q;
        }
        public Matrix getQ()
        {
            return Q;
        }

    }
}
