namespace Rendor.Visual.Rendering.OpenGL;

internal class LineGLProgram : IDisposable
{
    public LineGLProgram()
    {
        var builder = new GLProgramBuilder();
        program = builder.AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
            .AddShaderFromString(ShaderType.FragmentShader, fragmentShaderSource)
            .Build();

        u_ResolutionLocation = program.GetUniformLocation("u_Resolution");
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
            GL.Uniform2f(u_ResolutionLocation, value.Item1, value.Item2);
        }
    }

    private GLProgram program;

    private int u_ResolutionLocation;

    private const string vertexShaderSource = """
        #version 330 core
        layout(location = 0) in vec2 aPos;
        layout(location = 1) in vec2 aFrom;
        layout(location = 2) in vec2 aTo;
        layout(location = 3) in vec4 aColor;
        layout(location = 4) in float aWidth;

        uniform vec2 u_Resolution;

        out vec4 Color;

        vec2 toSpace(vec2 point)
        {
            float aspectRatio = u_Resolution.y / u_Resolution.x;

        // Adjust the point to normalized device coordinates with aspect ratio correction
        vec2 val = vec2(
            (point.x / u_Resolution.x * 2.0 - 1.0),
            1.0 - point.y / u_Resolution.y * 2.0
        );

        return vec2(val.x, val.y);
        }

        float toWidth(float width)
        {
            float value = 1.0 / u_Resolution.y * width;
            return value;
        }

        void main()
        {
            vec2 start = aFrom;
            vec2 end = aTo;

            vec2 xBasis = end - start;
            vec2 yBasis = normalize(vec2(-xBasis.y, xBasis.x));
            vec2 point = start + xBasis * aPos.x + yBasis * aWidth * aPos.y;
            Color = aColor;
            
            gl_Position = vec4(toSpace(point), 0.0, 1.0);
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
