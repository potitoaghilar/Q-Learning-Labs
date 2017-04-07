using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Q_Learning
{
    // Experiment are in which everything is snapped to a unitary grid
    class Game : GameWindow
    {
        protected int generationN = 0;

        public Game(int width, int height) : base(width, height)
        {
            setGenerationInTitle(generationN);
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

            drawLabWalls(10, 10, 80, 80, Color.Black);


            this.SwapBuffers();

        }

        public void drawLabWalls(int x, int y, int width, int height, Color color) {

            GL.Color3(color);

            Vector2[] vertices = new Vector2[] {
                new Vector2(x, y),
                new Vector2(x + width, y),
                new Vector2(x + width, y + height),
                new Vector2(x, y + height)
            };
            drawLines(vertices);

            setCamera(100, 100);

        }

        protected void drawLines(Vector2[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(vertices[i].X, vertices[i].Y);
                int next = i + 1;
                if (next == vertices.Length) next = 0;
                GL.Vertex2(vertices[next].X, vertices[next].Y);
                GL.End();
            }
        }

        protected void setCamera(float x, float y)
        {
            GL.Ortho(0, x, 0, y, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }

        protected void setGenerationInTitle(int generationN) {
            this.Title = "Q-Learning Experiment Lab - Generation #" + generationN;
        }
    }
}
