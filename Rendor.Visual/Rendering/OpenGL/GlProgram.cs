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

    public int GetUniformLocation(string name)
    {
        return GL.GetUniformLocation(Id, name);
    }

    public uint GetUniformBlockIndex(string name)
    {
        return GL.GetUniformBlockIndex(Id, name);
    }

    public void BindUniformBlock(string name, uint binding)
    {
        var index = GetUniformBlockIndex(name);
        GL.UniformBlockBinding(Id, index, binding);
    }

    public GLObjectType Type => GLObjectType.Program;
}
