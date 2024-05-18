namespace Rendor.Visual.Rendering.OpenGL;

/// <summary>
/// Represents an OpenGL built program. Use <see cref="GLProgramBuilder"/> to create a new instance.
/// </summary>
internal class GLProgram : IGLObject
{
    public uint Id { get; private set; }

    internal GLProgram(uint id)
    {
        Id = id;
    }

    public void Use()
    {
        GL.UseProgram(Id);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Id);
    }

    public GLObjectType Type => GLObjectType.Program;
}
