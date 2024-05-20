namespace Rendor.Visual.Rendering.OpenGL;

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

    public unsafe void BufferData(T[] data, BufferTarget target, BufferUsage usageHint)
    {
        GL.BindBuffer(target, Id);

        var newSize = data.Length * sizeof(T);

        if (size >= newSize)
        {
            GL.BufferSubData(target, 0, data);
            return;
        }
        else
        {
            size = newSize;
            GL.BufferData(target, data, usageHint);
        }
    }

    private int size = 0;
}

internal class GLBuffer : IGLObject
{
    public uint Id { get; protected init; }

    public GLObjectType Type => GLObjectType.Buffer;

    public void Bind(BufferTarget target)
    {
        GL.BindBuffer(target, Id);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(Id);
    }
}
