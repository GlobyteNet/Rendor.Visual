namespace Rendor.Visual.Rendering.OpenGL;

internal class TexGLProgram : IDisposable
{
    public TexGLProgram()
    {
        var builder = new GLProgramBuilder();
        program = builder.AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
            .AddShaderFromString(ShaderType.FragmentShader, fragmentShaderSource)
            .Build();
    }

    public void Use()
    {
        program.Use();
    }

    public void Dispose()
    {
        program.Dispose();
    }

    private GLProgram program;

    private const string vertexShaderSource = """
        #version 330 core
        layout(location = 0) in vec3 aPos;
        layout(location = 1) in vec2 aTexCoord;

        out vec2 TexCoord;

        void main()
        {
            gl_Position = vec4(aPos, 1.0);
            TexCoord = aTexCoord;
        }
        """;

    private const string fragmentShaderSource = """
        #version 330 core
        out vec4 FragColor;

        in vec2 TexCoord;

        uniform sampler2D texture1;

        void main()
        {
            FragColor = texture(texture1, TexCoord);
        }
        """;
}
