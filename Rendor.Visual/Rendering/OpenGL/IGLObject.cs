namespace Rendor.Visual.Rendering.OpenGL;

internal interface IGLObject : IDisposable
{
    public uint Id { get; }

    public GLObjectType Type { get; }
}

internal enum GLObjectType
{
    Buffer,
    VertexArray,
    Shader,
    Program
}
