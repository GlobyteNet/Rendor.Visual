using Rendor.Visual.GUI;

namespace Rendor.Visual.Rendering.OpenGL;

public class MainGlProgram : GlProgram
{
    public MainGlProgram() : base(vertexShaderSource, fragmentShaderSource)
    {
        uniformColorLocation = GL.GetUniformLocation(ID, "u_Color");
    }

    public Color U_Color
    {
        set
        {
            GL.Uniform4f(uniformColorLocation, value.r, value.g, value.b, value.a);
        }
    }

    private int uniformColorLocation;


    static string[] vertexShaderSource = new string[]
    {
        "#version 330 core\n layout (location = 0) in vec3 aPos;\n void main()\n {\n   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n }\0"
    };

    static string[] fragmentShaderSource = new string[]
    {
        "#version 330 core\n out vec4 FragColor;\n uniform vec4 u_Color;\n void main()\n {\n    FragColor = u_Color;\n }\n\0"
    };
}
