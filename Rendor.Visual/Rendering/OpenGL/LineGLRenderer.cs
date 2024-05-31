using Rendor.Visual.Drawing;
using Rendor.Visual.Drawing.Commands;

namespace Rendor.Visual.Rendering.OpenGL
{
    internal class LineGLRenderer
    {
        public LineGLRenderer(uint uboBinding)
        {
            program = new LineGLProgram(uboBinding);
            vertexBuffer = new GLBuffer<Line>();
            lineInstanceBuffer = new GLBuffer<Vector2f>();
            vertexArray = new VertexArray();

            vertexArray.AddPoint2f(lineInstanceBuffer, 0, 8, 0); // instance data
            vertexArray.AddPoint2f(vertexBuffer, 1, 36, 0, 1); // position
            vertexArray.AddPoint2f(vertexBuffer, 2, 36, 8, 1); // position
            vertexArray.AddPoint4f(vertexBuffer, 3, 36, 16, 1); // color
            vertexArray.AddPoint1f(vertexBuffer, 4, 36, 32, 1); // width
            vertexArray.Build();

            lineInstanceBuffer.BufferData(LineInstance, BufferTarget.ArrayBuffer, BufferUsage.StaticDraw);
        }

        public void Render(DrawLineCommand command)
        {
            vertexBuffer.BufferData(command.Lines.ToArray(), BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);

            program.Use();
            vertexArray.Bind();

            GL.DrawArraysInstanced(DrawMode.Triangles, 0, 6, command.Lines.Count);

            vertexArray.Unbind();
        }

        private GLBuffer<Line> vertexBuffer;
        private GLBuffer<Vector2f> lineInstanceBuffer;
        private VertexArray vertexArray;
        private LineGLProgram program;

        private static Vector2f[] LineInstance =
        [
            new Vector2f(0.0f, -0.5f),
            new Vector2f(1.0f, -0.5f),
            new Vector2f(1.0f, 0.5f),
            new Vector2f(0.0f, -0.5f),
            new Vector2f(1.0f, 0.5f),
            new Vector2f(0.0f, 0.5f)
        ];
    }
}
