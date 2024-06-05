namespace Rendor.Visual.Rendering.OpenGL;

internal class LineGLProgram : IDisposable
{
    public LineGLProgram(uint uboBinding)
    {
        var builder = new GLProgramBuilder();
        program = builder.AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
            .AddShaderFromString(ShaderType.FragmentShader, fragmentShaderSource)
            .Build();

        program.BindUniformBlock("UBO", uboBinding);
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
        layout(location = 0) in vec2 aPos;
        layout(location = 1) in vec3 aStart;
        layout(location = 2) in vec3 aEnd;
        layout(location = 3) in vec4 aColor;
        layout(location = 4) in float aWidth;

        layout(std140) uniform UBO
        {
            vec2 screenResolution;
        };

        out vec4 Color;

        vec3 toScreenSpace(vec3 point)
        {
            return vec3(
                (point.x / screenResolution.x * 2.0 - 1.0), // x
                1.0 - point.y / screenResolution.y * 2.0,   // y
                point.z
            );
        }

        void main()
        {
            vec3 xBasis = aEnd - aStart;
            vec3 yBasis = normalize(vec3(-xBasis.y, xBasis.x, 0));
            vec3 point = aStart + xBasis * aPos.x + yBasis * aWidth * aPos.y;
            Color = aColor;
            
            gl_Position = vec4(toScreenSpace(point), 1.0);
        }
        """;

    private const string fragmentShaderSource = """
        #version 330 core

        in vec4 Color;

        out vec4 FragColor;

        void main()
        {
            FragColor = Color;
        }
        """;
}
