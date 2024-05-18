using System.Text;

namespace Rendor.Visual.Rendering.OpenGL
{
    internal class GLProgramBuilder
    {
        private readonly List<uint> shaders = new();

        public GLProgramBuilder AddShaderFromString(ShaderType type, string source)
        {
            shaders.Add(CreateShader(type, source));
            return this;
        }

        public GLProgram Build()
        {
            uint program = GL.CreateProgram();
            foreach (var shader in shaders)
            {
                GL.AttachShader(program, shader);
            }
            GL.LinkProgram(program);

            GL.GetProgramiv(program, ParameterName.LinkStatus, out bool success);
            if (!success)
            {
                byte[] infoLog = new byte[512];
                GL.GetProgramInfoLog(program, 512, out _, infoLog);
                string infoLogStr = Encoding.ASCII.GetString(infoLog);
                Console.WriteLine("ERROR::SHADER::PROGRAM::LINKING_FAILED\n" + infoLogStr);
            }

            foreach (var shader in shaders)
            {
                GL.DeleteShader(shader);
            }

            return new GLProgram(program);
        }

        private uint CreateShader(ShaderType type, string source)
        {
            uint shader = GL.CreateShader(type);

            GL.ShaderSource(shader, [source]);
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
