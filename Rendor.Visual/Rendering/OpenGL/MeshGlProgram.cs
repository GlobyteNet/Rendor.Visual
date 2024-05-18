using Rendor.Visual.GUI;

namespace Rendor.Visual.Rendering.OpenGL;

/// <summary>
/// Wrapps the OpenGL program and provides a way to set its uniform variables.
/// </summary>
public class MeshGlProgram : IDisposable
{
    public MeshGlProgram()
    {
        var shaderSource = File.ReadAllText("Rendering/OpenGL/Shaders.glsl");
        var vertexShaderSource = GetShader(shaderSource, ShaderType.VertexShader);
        var fragmentShaderSource = GetShader(shaderSource, ShaderType.FragmentShader);

        var builder = new GLProgramBuilder();
        program = builder.AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
            .AddShaderFromString(ShaderType.FragmentShader, fragmentShaderSource)
            .Build();

        uniformResolutionLocation = GL.GetUniformLocation(program.Id, "u_Resolution");
    }

    public void Use()
    {
        program.Use();
    }

    public void Dispose()
    {
        program.Dispose();
    }

    public (float, float) U_Resolution
    {
        set
        {
            Use();
            GL.Uniform2f(uniformResolutionLocation, value.Item1, value.Item2);
        }
    }

    private static string GetShader(string source, ShaderType type)
    {
        int begin;
        int end;

        if (type == ShaderType.VertexShader)
        {
            begin = source.IndexOf("### VERTEX SHADER") + "### VERTEX SHADER".Length;
            end = source.IndexOf("### END VERTEX SHADER");
        }
        else
        {
            begin = source.IndexOf("### FRAGMENT SHADER") + "### FRAGMENT SHADER".Length;
            end = source.IndexOf("### END FRAGMENT SHADER");
        }

        return source.Substring(begin, end - begin);
    }

    private int uniformResolutionLocation;

    private GLProgram program;
}
