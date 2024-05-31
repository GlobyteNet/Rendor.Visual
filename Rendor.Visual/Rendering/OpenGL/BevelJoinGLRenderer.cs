using Rendor.Visual.Drawing;
using Rendor.Visual.Drawing.Commands;

namespace Rendor.Visual.Rendering.OpenGL
{
    internal class BevelJoinGLRenderer
    {
        public BevelJoinGLRenderer(uint uboBinding)
        {
            program = new BevelJoinGLProgram(uboBinding);
            vertexBuffer = new GLBuffer<LineSegment>();
            bevelInstance = new GLBuffer<Vector2f>();
            vertexArray = new VertexArray();

            vertexArray.AddPoint2f(bevelInstance, 0, 8, 0); // instance data
            vertexArray.AddPoint2f(vertexBuffer, 1, 44, 0, 1); // position start
            vertexArray.AddPoint2f(vertexBuffer, 2, 44, 8, 1); // position middle
            vertexArray.AddPoint2f(vertexBuffer, 3, 44, 16, 1); // position end
            vertexArray.AddPoint4f(vertexBuffer, 4, 44, 24, 1); // color
            vertexArray.AddPoint1f(vertexBuffer, 5, 44, 40, 1); // width
            vertexArray.Build();

            bevelInstance.BufferData(BevelInstance, BufferTarget.ArrayBuffer, BufferUsage.StaticDraw);
        }

        public void Render(DrawBevelJoinCommand command)
        {
            vertexBuffer.BufferData(command.LineSegments.ToArray(), BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);

            program.Use();
            vertexArray.Bind();

            GL.DrawArraysInstanced(DrawMode.Triangles, 0, 6, command.LineSegments.Count);

            vertexArray.Unbind();
        }

        private GLBuffer<Vector2f> bevelInstance;
        private GLBuffer<LineSegment> vertexBuffer;
        private VertexArray vertexArray;
        private BevelJoinGLProgram program;

        private static Vector2f[] BevelInstance = [
            new Vector2f(0.0f, 0.0f),
            new Vector2f(1.0f, 0.0f),
            new Vector2f(0.0f, 1.0f),
        ];
    }
}
