using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_Learning
{
    class Matrix
    {

        protected double[][] matrix;

        public Matrix() {
            matrix = new double[][] { };
        }
        public Matrix(int size) {
            matrix = new double[size][];

            for (int i = 0; i < matrix.Length; i++) {
                matrix[i] = new double[size];
            }
        }

        public void increaseMatrix() {
            // Initialize new row
            double[] newRow = new double[matrix.Length + 1];

            // Add new row and column to matrix
            double[][] tempMatrix = new double[matrix.Length + 1][];
            for (int i = 0; i < tempMatrix.Length; i++)
            {
                if (i < matrix.Length)
                {
                    tempMatrix[i] = new double[matrix[i].Length + 1];
                    for (int o = 0; o < tempMatrix[i].Length; o++)
                    {
                        if (o < matrix[i].Length)
                            tempMatrix[i][o] = matrix[i][o];
                        else
                            tempMatrix[i][o] = 0;
                    }
                }
                else {
                    // Here adds last new row
                    tempMatrix[i] = newRow;
                }
            }
            
            // Assign new array to "matrix"
            matrix = tempMatrix;
        }

        public void setValue(int x, int y, double value)
        {
            matrix[x][y] = value;
        }
        public double getValue(int x, int y)
        {
            return matrix[x][y];
        }

        public int getLength()
        {
            return matrix.Length;
        }

        public double getMaxFromRow(int i)
        {
            return matrix[i].Max();
        }

        public double getSumOfRow(int i)
        {
            return matrix[i].Sum();
        }

    }
}
