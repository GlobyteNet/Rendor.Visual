namespace Rendor.Visual.Rendering.OpenGL;

internal class BevelJoinGLProgram : IDisposable
{
    public BevelJoinGLProgram(uint uboBinding)
    {
        program = new GLProgramBuilder()
            .AddShaderFromString(ShaderType.VertexShader, vertexShaderSource)
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
        layout(location = 1) in vec2 aStart;
        layout(location = 2) in vec2 aMiddle;
        layout(location = 3) in vec2 aEnd;
        layout(location = 4) in vec4 aColor;
        layout(location = 5) in float aWidth;

        layout(std140) uniform UBO
        {
            vec2 screenResolution;
        };

        out vec4 Color;

        vec2 toScreenSpace(vec2 point)
        {
            return vec2(
                (point.x / screenResolution.x * 2.0 - 1.0), // x
                1.0 - point.y / screenResolution.y * 2.0    // y
            );
        }

        void main()
        {
            vec2 tangent = normalize(normalize(aEnd - aMiddle) + normalize(aMiddle - aStart));
            vec2 normal = vec2(-tangent.y, tangent.x);

            vec2 ab = aMiddle - aStart;
            vec2 cb = aMiddle - aEnd;

            vec2 abn = normalize(vec2(-ab.y, ab.x));
            vec2 cbn = -normalize(vec2(-cb.y, cb.x));

            float sigma = sign(dot(ab + cb, normal));

            vec2 p0 = 0.5 * sigma * aWidth * (sigma < 0.0 ? abn : cbn);
            vec2 p1 = 0.5 * sigma * aWidth * (sigma < 0.0 ? cbn : abn);
            vec2 point = aMiddle + aPos.x * p0 + aPos.y * p1;

            Color = aColor;
            gl_Position = vec4(toScreenSpace(point), 0.0, 1.0);
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
