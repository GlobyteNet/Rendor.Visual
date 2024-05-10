using CorePlayground.Main.GUI.Rendering.OpenGL;

namespace CorePlayground.Main.OpenGL
{
    /// <summary>
    /// Thin wrapper around OpenGL buffer objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class GLBuffer<T> : GLBuffer
        where T : unmanaged
    {
        public GLBuffer()
        {
            Id = GL.GenBuffer();
        }

        public void BufferData(T[] data, BufferTarget target, BufferUsage usageHint)
        {
            GL.BindBuffer(target, Id);

            GL.BufferData(target, data, usageHint);
        }
    }

    internal class GLBuffer : IDisposable, IGLObject
    {
        public uint Id { get; protected init; }

        public IGLObjectType Type => IGLObjectType.Buffer;

        public void Bind(BufferTarget target)
        {
            GL.BindBuffer(target, Id);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }

    internal interface IGLObject
    {
        public uint Id { get; }

        public IGLObjectType Type { get; }
    }

    internal enum IGLObjectType
    {
        Buffer,
        VertexArray,
        Shader,
        Program
    }
}
