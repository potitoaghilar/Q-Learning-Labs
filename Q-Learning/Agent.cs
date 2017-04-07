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
        protected LabSpace lab;
        protected Thread brain;

        public Agent(int position_x, int position_y, double learning_rate, LabSpace lab, double radius = .25) {
            this.position_x = position_x;
            this.position_y = position_y;
            this.radius = radius;

            // Set learning rate
            if (learning_rate < 0) learning_rate = 0;
            if (learning_rate > .99) learning_rate = .99;
            this.learning_rate = learning_rate;

            // Set lab in which agent have to learn
            this.lab = lab;

            // Start Q matrix as zero matrix - QI = 1x1 matrix
            Q = new Matrix(1);

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

                // TODO
                Array actionsArray = Enum.GetValues(typeof(actions));
                actions action = (actions)actionsArray.GetValue((new Random()).Next(actionsArray.Length));
                GetType().GetMethod(action.ToString()).Invoke(this, null);

                Thread.Sleep(100);

            }

        }

        // Agent possible actions enum
        protected enum actions {
            moveUp,
            moveDown,
            moveLeft,
            moveRight,
            interact
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
        public void interact()
        {
            // TODO
        }

        public void stopBrain() {
            brain.Abort();
        }

        //Q.increaseMatrix();

    }
}
