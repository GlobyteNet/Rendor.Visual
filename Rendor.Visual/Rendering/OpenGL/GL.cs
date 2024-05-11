using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rendor.Visual.Rendering.OpenGL;

/// <summary>
/// Provides access to OpenGL functions.
/// </summary>
internal static class GL
{
    #region Initialization

    static GL()
    {
        GetProcAddress = GetProcAddressFunction();

        LoadFunction(out glAttachShader, nameof(glAttachShader));
        LoadFunction(out glBindBuffer, nameof(glBindBuffer));
        LoadFunction(out glBindVertexArray, nameof(glBindVertexArray));
        LoadFunction(out glBufferData, nameof(glBufferData));
        LoadFunction(out glBufferSubData, nameof(glBufferSubData));
        LoadFunction(out glClear, nameof(glClear));
        LoadFunction(out glClearColor, nameof(glClearColor));
        LoadFunction(out glCompileShader, nameof(glCompileShader));
        LoadFunction(out glCreateProgram, nameof(glCreateProgram));
        LoadFunction(out glCreateShader, nameof(glCreateShader));
        LoadFunction(out glCullFace, nameof(glCullFace));
        LoadFunction(out glDeleteBuffers, nameof(glDeleteBuffers));
        LoadFunction(out glDeleteShader, nameof(glDeleteShader));
        LoadFunction(out glDrawArrays, nameof(glDrawArrays));
        LoadFunction(out glDrawElements, nameof(glDrawElements));
        LoadFunction(out glEnableVertexAttribArray, nameof(glEnableVertexAttribArray));
        LoadFunction(out glFlush, nameof(glFlush));
        LoadFunction(out glGenBuffers, nameof(glGenBuffers));
        LoadFunction(out glGenVertexArrays, nameof(glGenVertexArrays));
        LoadFunction(out glGetProgramInfoLog, nameof(glGetProgramInfoLog));
        LoadFunction(out glGetProgramiv, nameof(glGetProgramiv));
        LoadFunction(out glGetShaderInfoLog, nameof(glGetShaderInfoLog));
        LoadFunction(out glGetShaderiv, nameof(glGetShaderiv));
        LoadFunction(out glGetUniformLocation, nameof(glGetUniformLocation));
        LoadFunction(out glLinkProgram, nameof(glLinkProgram));
        LoadFunction(out glShaderSource, nameof(glShaderSource));
        LoadFunction(out glUseProgram, nameof(glUseProgram));
        LoadFunction(out glVertexAttribPointer, nameof(glVertexAttribPointer));
        LoadFunction(out glViewport, nameof(glViewport));
        LoadFunction(out glUniform2f, nameof(glUniform2f));
        LoadFunction(out glUniform4f, nameof(glUniform4f));

        LoadFunction(out glGetError, nameof(glGetError));
    }

    private static GetProcAddressDelegate GetProcAddressFunction()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            nint lib = NativeLibrary.Load("opengl32.dll");
            nint getProcAddressPtr = NativeLibrary.GetExport(lib, "wglGetProcAddress");

            return Marshal.GetDelegateForFunctionPointer<GetProcAddressDelegate>(getProcAddressPtr);
        }
        else
        {
            throw new PlatformNotSupportedException("Only Windows is supported at this time");
        }
    }

    delegate nint GetProcAddressDelegate(string procName);
    static GetProcAddressDelegate GetProcAddress;

    private static void LoadFunction<TDelegate>(out TDelegate function, string name)
        where TDelegate : Delegate
    {
        nint ptr = GetProcAddress(name);

        if (ptr == 0)
        {
            throw new EntryPointNotFoundException($"Could not find function {name}");
        }

        function = Marshal.GetDelegateForFunctionPointer<TDelegate>(ptr);
    }

    #endregion

    #region Attach Shader

    delegate void AttachShaderDelegate(uint program, uint shader);
    static readonly AttachShaderDelegate glAttachShader;

    public static void AttachShader(uint program, uint shader)
    {
        glAttachShader(program, shader);
        CheckErrors();
    }

    #endregion

    #region Bind Buffer

    delegate void BindBufferDelegate(uint target, uint buffer);
    static readonly BindBufferDelegate glBindBuffer;

    /// <summary>
    /// Binds a buffer object to a buffer target. What this means is that
    /// the buffer object will now be used whenever some operation requires
    /// a buffer of the specified target.
    /// </summary>
    /// <param name="target">The target to which the buffer object is bound</param>
    /// <param name="buffer">The buffer object to bind</param>
    public static void BindBuffer(BufferTarget target, uint buffer)
    {
        glBindBuffer((uint)target, buffer);
        CheckErrors();
    }

    #endregion

    #region Bind Vertex Array

    delegate void BindVertexArrayDelegate(uint array);
    static readonly BindVertexArrayDelegate glBindVertexArray;

    public static void BindVertexArray(uint array)
    {
        glBindVertexArray(array);
        CheckErrors();
    }

    #endregion

    #region Buffer Data

    unsafe delegate void BufferDataDelegate(uint target, int size, void* data, uint usage);
    static readonly BufferDataDelegate glBufferData;

    /// <summary>
    /// Creates a new data store for the buffer object currently bound to the target. Any pre-existing data store is deleted.
    /// </summary>
    /// <typeparam name="T">The type of the data to upload</typeparam>
    /// <param name="target">Uploads the data to the buffer object bound to this target</param>
    /// <param name="data">The data to upload</param>
    /// <param name="usage">The expected usage pattern of the data store</param>
    public unsafe static void BufferData<T>(BufferTarget target, T[] data, BufferUsage usage) where T : unmanaged
    {
        var size = data.Length * sizeof(T);

        fixed (T* dataPtr = data)
        {
            glBufferData((uint)target, size, dataPtr, (uint)usage);
        }
        CheckErrors();
    }

    #endregion

    #region Buffer Sub Data

    unsafe delegate void BufferSubDataDelegate(uint target, int offset, int size, void* data);
    static readonly BufferSubDataDelegate glBufferSubData;

    /// <summary>
    /// Updates a part or all of the data in a buffer object. This function cannot be used to increase the size of a buffer object.
    /// </summary>
    /// <typeparam name="T">The type of the data to upload</typeparam>
    /// <param name="target">Uploads the data to the buffer object bound to this target</param>
    /// <param name="offset">The offset in bytes from the beginning of the buffer object where the data should be uploaded</param>
    /// <param name="data">The data to upload</param>
    public unsafe static void BufferSubData<T>(BufferTarget target, int offset, T[] data) where T : unmanaged
    {
        var size = data.Length * sizeof(T);

        fixed (T* dataPtr = data)
        {
            glBufferSubData((uint)target, offset, size, dataPtr);
        }
        CheckErrors();
    }

    #endregion

    #region Clear

    delegate void ClearDelegate(uint mask);
    static readonly ClearDelegate glClear;

    public static void Clear(ClearBufferMask mask)
    {
        glClear((uint)mask);
        CheckErrors();
    }

    #endregion

    #region Clear Color

    delegate void ClearColorDelegate(float red, float green, float blue, float alpha);
    static readonly ClearColorDelegate glClearColor;

    public static void ClearColor(float red, float green, float blue, float alpha)
    {
        glClearColor(red, green, blue, alpha);
        CheckErrors();
    }

    #endregion

    #region Compile Shader

    delegate void CompileShaderDelegate(uint shader);
    static readonly CompileShaderDelegate glCompileShader;

    public static void CompileShader(uint shader)
    {
        glCompileShader(shader);
        CheckErrors();
    }

    #endregion

    #region Create Program

    delegate uint CreateProgramDelegate();
    static readonly CreateProgramDelegate glCreateProgram;

    public static uint CreateProgram()
    {
        var program = glCreateProgram();
        CheckErrors();
        return program;
    }

    #endregion

    #region Create Shader

    delegate uint CreateShaderDelegate(uint type);
    static readonly CreateShaderDelegate glCreateShader;

    public static uint CreateShader(ShaderType type)
    {
        var shader = glCreateShader((uint)type);
        CheckErrors();
        return shader;
    }

    #endregion

    #region Cull Face

    delegate void CullFaceDelegate(uint mode);
    static readonly CullFaceDelegate glCullFace;

    public static void CullFace(uint mode)
    {
        glCullFace(mode);
        CheckErrors();
    }

    #endregion

    #region Delete Buffer

    delegate void DeleteBuffersDelegate(int n, ref uint buffers);
    static readonly DeleteBuffersDelegate glDeleteBuffers;

    public static void DeleteBuffer(uint buffer)
    {
        glDeleteBuffers(1, ref buffer);
        CheckErrors();
    }

    #endregion

    #region Delete Shader

    delegate void DeleteShaderDelegate(uint shader);
    static readonly DeleteShaderDelegate glDeleteShader;

    public static void DeleteShader(uint shader)
    {
        glDeleteShader(shader);
        CheckErrors();
    }

    #endregion

    #region Draw Arrays

    delegate void DrawArraysDelegate(uint mode, int first, int count);
    static readonly DrawArraysDelegate glDrawArrays;

    public static void DrawArrays(DrawMode mode, int first, int count)
    {
        glDrawArrays((uint)mode, first, count);
        CheckErrors();
    }

    #endregion

    #region Draw Elements

    delegate void DrawElementsDelegate(uint mode, int count, uint type, int indices);
    static readonly DrawElementsDelegate glDrawElements;

    public static void DrawElements(DrawMode mode, int count, DataType type, int indices)
    {
        glDrawElements((uint)mode, count, (uint)type, indices);
        CheckErrors();
    }

    #endregion

    #region Enable Vertex Attrib Array

    delegate void EnableVertexAttribArrayDelegate(uint index);
    static readonly EnableVertexAttribArrayDelegate glEnableVertexAttribArray;

    public static void EnableVertexAttribArray(uint index)
    {
        glEnableVertexAttribArray(index);
        CheckErrors();
    }

    #endregion

    #region Flush

    delegate void FlushDelegate();
    static readonly FlushDelegate glFlush;

    public static void Flush()
    {
        glFlush();
        CheckErrors();
    }

    #endregion

    #region Gen Buffer

    delegate void GenBuffersDelegate(int n, ref uint buffers);
    static readonly GenBuffersDelegate glGenBuffers;

    /// <summary>
    /// Generates buffer object name. This function in itself does not
    /// create any buffer but rather helps to find an unused name for a new buffer object.
    /// </summary>
    /// <returns>The name of the buffer object</returns>
    public static uint GenBuffer()
    {
        uint buffer = 0;
        glGenBuffers(1, ref buffer);
        CheckErrors();
        return buffer;
    }

    #endregion

    #region Gen Vertex Array

    delegate void GenVertexArraysDelegate(int n, ref uint arrays);
    static readonly GenVertexArraysDelegate glGenVertexArrays;

    public static uint GenVertexArray()
    {
        uint array = 0;
        glGenVertexArrays(1, ref array);
        CheckErrors();
        return array;
    }

    #endregion

    #region Get Program Info Log

    delegate void GetProgramInfoLogDelegate(uint program, int maxLength, out int length, [Out] byte[] infoLog);
    static readonly GetProgramInfoLogDelegate glGetProgramInfoLog;

    public static void GetProgramInfoLog(uint program, int maxLength, out int length, byte[] infoLog)
    {
        glGetProgramInfoLog(program, maxLength, out length, infoLog);
        CheckErrors();
    }

    #endregion

    #region Get Program Integer Vector

    delegate void GetProgramivDelegate(uint program, uint pname, out bool success);
    static readonly GetProgramivDelegate glGetProgramiv;

    public static void GetProgramiv(uint program, ParameterName pname, out bool success)
    {
        glGetProgramiv(program, (uint)pname, out success);
        CheckErrors();
    }

    #endregion

    #region Get Shader Info Log

    delegate void GetShaderInfoLogDelegate(uint shader, int maxLength, out int length, char[] infoLog);
    static readonly GetShaderInfoLogDelegate glGetShaderInfoLog;

    public static void GetShaderInfoLog(uint shader, int maxLength, out int length, char[] infoLog)
    {
        glGetShaderInfoLog(shader, maxLength, out length, infoLog);
        CheckErrors();
    }

    #endregion

    #region Get Shader Integer Vector

    delegate void GetShaderivDelegate(uint shader, uint pname, out bool success);
    static readonly GetShaderivDelegate glGetShaderiv;

    public static void GetShaderiv(uint shader, ParameterName pname, out bool success)
    {
        glGetShaderiv(shader, (uint)pname, out success);
        CheckErrors();
    }

    #endregion

    #region Get Uniform Location

    delegate int GetUniformLocationDelegate(uint program, string name);
    static readonly GetUniformLocationDelegate glGetUniformLocation;

    public static int GetUniformLocation(uint program, string name)
    {
        var location = glGetUniformLocation(program, name);
        CheckErrors();
        return location;
    }

    #endregion

    #region Link Program

    delegate void LinkProgramDelegate(uint program);
    static readonly LinkProgramDelegate glLinkProgram;

    public static void LinkProgram(uint program)
    {
        glLinkProgram(program);
        CheckErrors();
    }

    #endregion

    #region Shader Source

    delegate void ShaderSourceDelegate(uint shader, int count, string[] source, int[] length);
    static readonly ShaderSourceDelegate glShaderSource;

    public static void ShaderSource(uint shader, int count, string[] source, int[]? length)
    {
        glShaderSource(shader, count, source, length);
        CheckErrors();
    }

    #endregion

    #region Use Program

    delegate void UseProgramDelegate(uint program);
    static readonly UseProgramDelegate glUseProgram;

    public static void UseProgram(uint program)
    {
        glUseProgram(program);
        CheckErrors();
    }

    #endregion

    #region Vertex Attrib Pointer

    delegate void VertexAttribPointerDelegate(uint index, int size, uint type, bool normalized, int stride, int offset);
    static readonly VertexAttribPointerDelegate glVertexAttribPointer;

    /// <summary>
    /// Defines how the data should be read to the shader. This function should be called after the buffer object that contains the data has been bound.
    /// </summary>
    /// <param name="index">The index of the vertex attribute that should be modified. It should match the layout location in the shader</param>
    /// <param name="size">The number of components per vertex attribute. Generally, the number corresponds to the vector type in the shader. For example, a vec3 in the shader would have a size of 3</param>
    /// <param name="type">The data type of each component in the array. For vertex data, it is usually float</param>
    /// <param name="normalized">Whether the data should be normalized. When normalized, the data is scaled to the range [-1, 1] (for signed values) or [0, 1] (for unsigned values) and then converted to floating point</param>
    /// <param name="stride">The number of bytes between the beginning of one vertex and the beginning of the next vertex. If there are no gaps between the vertices, this value can be 0</param>
    /// <param name="offset">The number of bytes from the beginning of the vertex to the beginning of this attribute</param>
    public static void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, int offset)
    {
        glVertexAttribPointer(index, size, (uint)type, normalized, stride, offset);
        CheckErrors();
    }

    #endregion

    #region Viewport

    delegate void ViewportDelegate(int x, int y, int width, int height);
    static readonly ViewportDelegate glViewport;

    public static void Viewport(int x, int y, int width, int height)
    {
        glViewport(x, y, width, height);
        CheckErrors();
    }

    #endregion

    #region Uniform 2 Float

    delegate void Uniform2fDelegate(int location, float v0, float v1);
    static readonly Uniform2fDelegate glUniform2f;

    public static void Uniform2f(int location, float v0, float v1)
    {
        glUniform2f(location, v0, v1);
        CheckErrors();
    }

    #endregion

    #region Uniform 4 Float

    delegate void Uniform4fDelegate(int location, float v0, float v1, float v2, float v3);
    static readonly Uniform4fDelegate glUniform4f;

    public static void Uniform4f(int location, float v0, float v1, float v2, float v3)
    {
        glUniform4f(location, v0, v1, v2, v3);
        CheckErrors();
    }

    #endregion

    #region Check Errors

    delegate uint GetErrorDelegate();
    static readonly GetErrorDelegate glGetError;

    [Conditional("DEBUG")]
    private static void CheckErrors()
    {
        uint error;

        while ((error = glGetError()) != 0)
        {
            Console.WriteLine($"OpenGL Error: {error}");
            Debug.Assert(false, $"OpenGL Error: {error}");
        }
    }

    #endregion
}

public enum ClearBufferMask : uint
{
    DepthBufferBit = 0x00000100,
    StencilBufferBit = 0x00000400,
    ColorBufferBit = 0x00004000,
}

public enum ShaderType : uint
{
    FragmentShader = 0x8B30,
    VertexShader = 0x8B31,
}

public enum BufferTarget : uint
{
    ArrayBuffer = 0x8892,
    ElementArrayBuffer = 0x8893
}

public enum CullFaceMode : uint
{
    Back = 0x0405
}

public enum ErrorCode : uint
{
    NoError = 0,
    InvalidEnum = 0x0500,
    InvalidValue = 0x0501,
    InvalidOperation = 0x0502,
    StackOverflow = 0x0503,
    StackUnderflow = 0x0504,
    OutOfMemory = 0x0505
}

public enum DataType : uint
{
    UnsignedInt = 0x1405,
    Float = 0x1406,
}

public enum ParameterName : uint
{
    CompileStatus = 0x8B81,
    LinkStatus = 0x8B82,
}

public enum BufferUsage : uint
{
    StreamDraw = 0x88E0,
    StaticDraw = 0x88E4,
    DynamicDraw = 0x88E8,
}

public enum DrawMode : uint
{
    Triangles = 0x0004,
    TriangleStrip = 0x0005,
}
