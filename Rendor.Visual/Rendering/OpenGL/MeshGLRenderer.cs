using Rendor.Visual.Drawing;
using Rendor.Visual.Drawing.Commands;

namespace Rendor.Visual.Rendering.OpenGL
{
    internal class MeshGLRenderer
    {
        public MeshGLRenderer(uint uboBinding)
        {
            vertexBuffer = new GLBuffer<ColorPoint>();
            vertexArray = new VertexArray();
            program = new MeshGLProgram(uboBinding);

            vertexArray.AddPoint3f(vertexBuffer, 0, 28, 0); // position
            vertexArray.AddPoint4f(vertexBuffer, 1, 28, 12); // color
            vertexArray.Build();
        }

        public void Render(DrawMeshCommand command)
        {
            vertexBuffer.BufferData(command.Meshes.ToArray(), BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);

            program.Use();
            vertexArray.Bind();

            GL.DrawArrays(DrawMode.Triangles, 0, command.Meshes.Count);

            vertexArray.Unbind();
        }

        private GLBuffer<ColorPoint> vertexBuffer;
        private VertexArray vertexArray;
        private MeshGLProgram program;
    }
}
