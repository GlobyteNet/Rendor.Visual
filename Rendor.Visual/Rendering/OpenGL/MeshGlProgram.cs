using Rendor.Visual.GUI;

namespace Rendor.Visual.Rendering.OpenGL;

/// <summary>
/// Wrapps the OpenGL program and provides a way to set its uniform variables.
/// </summary>
public class MeshGLProgram : IDisposable
{
    public MeshGLProgram(uint uboBInding)
    {
        var builder = new GLProgramBuilder();
        program = builder.AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
            .AddShaderFromString(ShaderType.FragmentShader, fragmentShaderSource)
            .Build();

        program.BindUniformBlock("UBO", uboBInding);
    }

    public void Use()
    {
        program.Use();
    }

    public void Dispose()
    {
        program.Dispose();
    }

    private readonly int uniformResolutionLocation;

    private GLProgram program;

    private const string vertexShaderSource = """"
        #version 330 core

        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec4 aColor;

        layout(std140) uniform UBO
        {
            vec2 screenResolution;
        };

        out vec4 ourColor;

        void main()
        {
        	gl_Position = vec4(aPos.x / screenResolution.x * 2.0 - 1.0, 1.0 - aPos.y / screenResolution.y * 2.0, aPos.z, 1.0);
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
