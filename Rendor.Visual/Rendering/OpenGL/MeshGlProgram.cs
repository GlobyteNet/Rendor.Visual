using Rendor.Visual.GUI;

namespace Rendor.Visual.Rendering.OpenGL;

/// <summary>
/// Wrapps the OpenGL program and provides a way to set its uniform variables.
/// </summary>
public class MeshGLProgram : IDisposable
{
    public MeshGLProgram()
    {
        var builder = new GLProgramBuilder();
        program = builder.AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
            .AddShaderFromString(ShaderType.FragmentShader, fragmentShaderSource)
            .Build();

        uniformResolutionLocation = program.GetUniformLocation("u_Resolution");
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

    private readonly int uniformResolutionLocation;

    private GLProgram program;

    private const string vertexShaderSource = """"
        #version 330 core

        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec4 aColor;

        out vec4 ourColor;

        uniform vec2 u_Resolution;

        void main()
        {
        	gl_Position = vec4(aPos.x / u_Resolution.x * 2.0 - 1.0, 1.0 - aPos.y / u_Resolution.y * 2.0, aPos.z, 1.0);
        	ourColor = aColor;
        }
        """";

    private const string fragmentShaderSource = """
        #version 330 core

        in vec4 ourColor;

        out vec4 FragColor;

        void main()
        {
        	FragColor = ourColor;
        }
        """;
}
