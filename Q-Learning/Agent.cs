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
        protected Matrix Q, A;
        protected Game game;
        protected LabSpace lab;
        protected Thread brain;
        private Random random = new Random();
        protected bool fastLearning;

        public Agent(int position_x, int position_y, double learning_rate, LabSpace lab, Game game, bool fastLearning = false, double radius = .25) {
            this.position_x = position_x;
            this.position_y = position_y;
            this.radius = radius;

            // Set learning rate
            if (learning_rate < 0) learning_rate = 0;
            if (learning_rate > .99) learning_rate = .99;
            this.learning_rate = learning_rate;

            // Set fast learning
            this.fastLearning = fastLearning;

            // Set game instance
            this.game = game;

            // Set lab in which agent have to learn
            this.lab = lab;

            // Start Q and A matrix as zero matrix
            Q = new Matrix(game.getR().getLength());
            A = new Matrix(game.getR().getLength());

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

        public double getLearningRate()
        {
            return learning_rate;
        }

        public bool getFastLearning()
        {
            return fastLearning;
        }

        protected void brainWorker() {

            bool brainActive = true;
            while (brainActive) {

                // Get current pos
                int currY = lab.height - (int)position_y - 1, currX = (int)position_x - lab.x - 1;

                // Calculate matrix row to get right coord of Q matrix
                int row = 0;
                for (int i = 0; i < currY; i++)
                    row += lab.width - 1;
                row += currX;
                
                // Select better action to reach the goal
                int selectedAction;
                Array actionsArray = Enum.GetValues(typeof(actions));

                int[] moreConvenientActions = Q.getMaxValueArrayFromRow(row);
                if (moreConvenientActions.Length == 0)
                    selectedAction = random.Next(actionsArray.Length);
                else
                    selectedAction = (int)A.getValue(row, moreConvenientActions[random.Next(0, moreConvenientActions.Length)]);
                actions action = (actions)actionsArray.GetValue(selectedAction);
                GetType().GetMethod(action.ToString()).Invoke(this, null);

                // Get next pos
                int nextY = (lab.height - (int)position_y - 1), nextX = (int)position_x - lab.x - 1;

                // Calculate matrix column to get right coord of Q matrix
                int column = 0;
                for (int i = 0; i < nextY; i++)
                    column += lab.width - 1;
                column += nextX;

                // Angent changed position -> update Q
                if (currY != nextY || currX != nextX) {
                    
                    // Update Q
                    Q.setValue(row, column, game.getR().getValue(row, column) + learning_rate * Q.getMaxFromRow(column));

                    // Update A
                    A.setValue(row, column, selectedAction);

                    if (fastLearning && game.getGeneration() <= 250)
                        Thread.Sleep(3);
                    else
                        Thread.Sleep(100);
                }

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

        public void setA(Matrix A)
        {
            this.A = A;
        }
        public Matrix getA()
        {
            return A;
        }

    }
}
