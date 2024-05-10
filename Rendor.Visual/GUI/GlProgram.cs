using CorePlayground.Main.GUI.Rendering.OpenGL;
using System.Text;

namespace CorePlayground.Main.GUI
{
    public class GlProgram
    {
        public uint ID { get; private set; }

        public GlProgram(string[] vertexShaderSource, string[] fragmentShaderSource)
        {
            uint vertexShader = CreateShader(ShaderType.VertexShader, vertexShaderSource);
            uint fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderSource);

            ID = GL.CreateProgram();

            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            GL.LinkProgram(ID);

            GL.GetProgramiv(ID, ParameterName.LinkStatus, out bool success);
            if (!success)
            {
                byte[] infoLog = new byte[512];
                GL.GetProgramInfoLog(ID, 512, out _, infoLog);
                string infoLogStr = Encoding.ASCII.GetString(infoLog);
                Console.WriteLine("ERROR::SHADER::PROGRAM::LINKING_FAILED\n" + infoLogStr);
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(ID);
        }

        private uint CreateShader(ShaderType type, string[] source)
        {
            uint shader = GL.CreateShader(type);
            GL.ShaderSource(shader, 1, source, null);
            GL.CompileShader(shader);

            bool success;
            GL.GetShaderiv(shader, ParameterName.CompileStatus, out success);
            if (!success)
            {
                char[] infoLog = new char[512];
                GL.GetShaderInfoLog(shader, 512, out _, infoLog);
                Console.WriteLine($"ERROR::SHADER::{type}::COMPILATION_FAILED\n{infoLog}");
            }

            return shader;
        }
    }
}
