using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Learning
{
    class Agent
    {
        protected double radius, position_x, position_y;

        public Agent(int position_x, int position_y, double radius = .25) {
            this.position_x = position_x;
            this.position_y = position_y;
            this.radius = radius;
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

    }
}
