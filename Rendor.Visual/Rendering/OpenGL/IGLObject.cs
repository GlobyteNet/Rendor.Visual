namespace Rendor.Visual.Rendering.OpenGL;

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
