namespace Rendor.Visual.Rendering.OpenGL;

public class GlProgram
{
    public uint ID { get; private set; }

    internal GlProgram(uint id)
    {
        ID = id;
    }

    public void Use()
    {
        GL.UseProgram(ID);
    }
}
