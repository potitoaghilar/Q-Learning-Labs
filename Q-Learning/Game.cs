using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;

namespace Q_Learning
{
    // Experiment are in which everything is snapped to a unitary grid
    class Game : GameWindow
    {
        protected int generationN = 1;
        private double aspectRatio;
        protected LabSpace lab;
        protected Agent agent;
        protected Matrix R;
        protected Random random = new Random();

        public Game(int width, int height) : base(width, height, new GraphicsMode(32, 24, 0, 4))
        {
            setGenerationInTitle(generationN);
            this.aspectRatio = (double)width / height;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.White);

            if (lab != null) drawLabWallsHelper(lab.x, lab.y, lab.width, lab.height, lab.color);
            if (agent != null) drawAgent();

            drawReward();
            
            this.SwapBuffers();

        }

        // PUBLIC FUNCTIONS
        public void drawLabWalls(int x, int y, int width, int height, Color color) {
            lab = new LabSpace(x, y, width, height, color);

            // Start R matrix - direct rewards matrix (top-right screen)
            R = new Matrix((width - 1) * (height - 1));
            R.setValue(width - 3, width - 2, 1);
            R.setValue((width - 1) * 2 - 1, width - 2, 1);
        }

        public void createAgent(int position_x, int position_y, double learningRate = .4, bool fastLearning = false) {
            agent = new Agent(position_x, position_y, learningRate, lab, this, fastLearning);
        }

        public void newGeneration() {
            Matrix Q = agent.getQ(), A = agent.getA();
            createAgent(random.Next(lab.x + 1, lab.width - 1), random.Next(lab.y + 1, lab.height - 1), agent.getLearningRate(), agent.getFastLearning());
            agent.setQ(Q);
            agent.setA(A);

            setGenerationInTitle(++generationN);
        }

        public Matrix getR() {
            return R;
        }

        public int getGeneration()
        {
            return generationN;
        }
        // ---
        
        // PROTECTED FUNCTIONS
        protected void drawLabWallsHelper(int x, int y, int width, int height, Color color) {
            GL.Color3(color);

            Vector2[] vertices = new Vector2[] {
                new Vector2(x, y),
                new Vector2(x + width, y),
                new Vector2(x + width, y + height),
                new Vector2(x, y + height)
            };
            drawLines(vertices);

            setCamera(x, y, x + width, y + height);
        }

        protected void drawLines(Vector2[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                GL.LineWidth(2);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(vertices[i].X, vertices[i].Y);
                int next = i + 1;
                if (next == vertices.Length) next = 0;
                GL.Vertex2(vertices[next].X, vertices[next].Y);
                GL.End();
            }
        }

        protected void setCamera(double left_margin_x, double left_margin_y, double right_margin_x, double right_margin_y)
        {
            int margin = 2;
            if (right_margin_x - left_margin_x >= right_margin_y - left_margin_y)
            {
                double margin_height = ((right_margin_x - left_margin_x + margin * 2) / aspectRatio - (right_margin_y - left_margin_y)) / 2;
                GL.Ortho(left_margin_x - margin, right_margin_x + margin, left_margin_y - margin_height, right_margin_y + margin_height, -1, 1);
            }
            else
            {
                double margin_width = ((right_margin_y - left_margin_y + margin * 2) * aspectRatio - (right_margin_x - left_margin_x)) / 2;
                GL.Ortho(left_margin_x - margin_width, right_margin_x + margin_width, left_margin_y - margin, right_margin_y + margin, -1, 1);
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }

        protected void drawAgent() {
            GL.Color3(Color.Blue);
            GL.Begin(PrimitiveType.Polygon);
            for(int i = 0; i < 360; i++)
                GL.Vertex2(agent.getPositionX() + agent.getRadius() * Math.Cos(i * MyMath.DEG2RAD), agent.getPositionY() + agent.getRadius() * Math.Sin(i * MyMath.DEG2RAD));
            GL.End();
        }

        protected void drawReward() {
            GL.Color3(Color.Yellow);
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < 360; i++)
                GL.Vertex2(lab.x + lab.width - 1 + .6 * Math.Cos(i * MyMath.DEG2RAD), lab.y + lab.height - 1 + .6 * Math.Sin(i * MyMath.DEG2RAD));
            GL.End();
        }

        protected void setGenerationInTitle(int generationN) {
            Title = "Q-Learning Experiment Lab - Generation #" + generationN;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            agent.stopBrain();
        }
    }
}
