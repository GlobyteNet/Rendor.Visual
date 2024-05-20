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
        LoadFunction(out glBindBufferBase, nameof(glBindBufferBase));
        LoadFunction(out glBindVertexArray, nameof(glBindVertexArray));
        LoadFunction(out glBufferData, nameof(glBufferData));
        LoadFunction(out glBufferSubData, nameof(glBufferSubData));
        LoadFunction(out glClear, nameof(glClear));
        LoadFunction(out glClearColor, nameof(glClearColor));
        LoadFunction(out glCompileShader, nameof(glCompileShader));
        LoadFunction(out glCreateProgram, nameof(glCreateProgram));
        LoadFunction(out glCreateShader, nameof(glCreateShader));
        LoadFunction(out glCullFace, nameof(glCullFace));
        LoadFunction(out glDebugMessageCallback, nameof(glDebugMessageCallback));
        LoadFunction(out glDeleteBuffers, nameof(glDeleteBuffers));
        LoadFunction(out glDeleteProgram, nameof(glDeleteProgram));
        LoadFunction(out glDeleteShader, nameof(glDeleteShader));
        LoadFunction(out glDeleteVertexArrays, nameof(glDeleteVertexArrays));
        LoadFunction(out glDrawArrays, nameof(glDrawArrays));
        LoadFunction(out glDrawArraysInstanced, nameof(glDrawArraysInstanced));
        LoadFunction(out glDrawElements, nameof(glDrawElements));
        LoadFunction(out glEnable, nameof(glEnable));
        LoadFunction(out glEnableVertexAttribArray, nameof(glEnableVertexAttribArray));
        LoadFunction(out glFlush, nameof(glFlush));
        LoadFunction(out glGenBuffers, nameof(glGenBuffers));
        LoadFunction(out glGenVertexArrays, nameof(glGenVertexArrays));
        LoadFunction(out glGetProgramInfoLog, nameof(glGetProgramInfoLog));
        LoadFunction(out glGetProgramiv, nameof(glGetProgramiv));
        LoadFunction(out glGetShaderInfoLog, nameof(glGetShaderInfoLog));
        LoadFunction(out glGetShaderiv, nameof(glGetShaderiv));
        LoadFunction(out glGetUniformBlockIndex, nameof(glGetUniformBlockIndex));
        LoadFunction(out glGetUniformLocation, nameof(glGetUniformLocation));
        LoadFunction(out glLinkProgram, nameof(glLinkProgram));
        LoadFunction(out glShaderSource, nameof(glShaderSource));
        LoadFunction(out glUniformBlockBinding, nameof(glUniformBlockBinding));
        LoadFunction(out glUseProgram, nameof(glUseProgram));
        LoadFunction(out glVertexAttribDivisor, nameof(glVertexAttribDivisor));
        LoadFunction(out glVertexAttribPointer, nameof(glVertexAttribPointer));
        LoadFunction(out glViewport, nameof(glViewport));
        LoadFunction(out glUniform2f, nameof(glUniform2f));
        LoadFunction(out glUniform4f, nameof(glUniform4f));

        LoadFunction(out glGetError, nameof(glGetError));

        Enable(GLCapability.DebugOutput);
        DebugMessageCallback(debugProc, IntPtr.Zero);
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
    }

    #endregion

    #region Bind Buffer Base

    delegate void BindBufferBaseDelegate(uint target, uint index, uint buffer);
    static readonly BindBufferBaseDelegate glBindBufferBase;

    /// <summary>
    /// Binds a buffer object to a buffer target at a specific index. This is used for binding buffer objects to indexed buffer targets.
    /// </summary>
    /// <param name="target">The target to which the buffer object is bound</param>
    /// <param name="index">The index at which the buffer object should be bound</param>
    /// <param name="buffer">The buffer object to bind</param>
    public static void BindBufferBase(BufferTarget target, uint index, uint buffer)
    {
        glBindBufferBase((uint)target, index, buffer);
    }

    #endregion

    #region Bind Vertex Array

    delegate void BindVertexArrayDelegate(uint array);
    static readonly BindVertexArrayDelegate glBindVertexArray;

    public static void BindVertexArray(uint array)
    {
        glBindVertexArray(array);
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
    }

    #endregion

    #region Clear

    delegate void ClearDelegate(uint mask);
    static readonly ClearDelegate glClear;

    public static void Clear(ClearBufferMask mask)
    {
        glClear((uint)mask);
    }

    #endregion

    #region Clear Color

    delegate void ClearColorDelegate(float red, float green, float blue, float alpha);
    static readonly ClearColorDelegate glClearColor;

    public static void ClearColor(float red, float green, float blue, float alpha)
    {
        glClearColor(red, green, blue, alpha);
    }

    #endregion

    #region Compile Shader

    delegate void CompileShaderDelegate(uint shader);
    static readonly CompileShaderDelegate glCompileShader;

    public static void CompileShader(uint shader)
    {
        glCompileShader(shader);
    }

    #endregion

    #region Create Program

    delegate uint CreateProgramDelegate();
    static readonly CreateProgramDelegate glCreateProgram;

    public static uint CreateProgram()
    {
        var program = glCreateProgram();
        
        return program;
    }

    #endregion

    #region Create Shader

    delegate uint CreateShaderDelegate(uint type);
    static readonly CreateShaderDelegate glCreateShader;

    public static uint CreateShader(ShaderType type)
    {
        var shader = glCreateShader((uint)type);
        
        return shader;
    }

    #endregion

    #region Cull Face

    delegate void CullFaceDelegate(uint mode);
    static readonly CullFaceDelegate glCullFace;

    public static void CullFace(uint mode)
    {
        glCullFace(mode);
    }

    #endregion

    #region Debug Message Callback

    delegate void DebugMessageCallbackDelegate(DebugProc callback, IntPtr userParam);
    static readonly DebugMessageCallbackDelegate glDebugMessageCallback;

    public delegate void DebugProc(ErrorSource source, ErrorType type, uint id, ErrorSeverety severity, int length, string message, IntPtr userParam);

    public static void DebugMessageCallback(DebugProc callback, IntPtr userParam)
    {
        glDebugMessageCallback(callback, userParam);
    }

    #endregion

    #region Delete Buffer

    delegate void DeleteBuffersDelegate(int n, ref uint buffers);
    static readonly DeleteBuffersDelegate glDeleteBuffers;

    public static void DeleteBuffer(uint buffer)
    {
        glDeleteBuffers(1, ref buffer);
    }

    #endregion

    #region Delete Program

    delegate void DeleteProgramDelegate(uint program);
    static readonly DeleteProgramDelegate glDeleteProgram;

    public static void DeleteProgram(uint program)
    {
        glDeleteProgram(program);
    }

    #endregion

    #region Delete Shader

    delegate void DeleteShaderDelegate(uint shader);
    static readonly DeleteShaderDelegate glDeleteShader;

    public static void DeleteShader(uint shader)
    {
        glDeleteShader(shader);
    }

    #endregion

    #region Delete Vertex Array 

    delegate void DeleteVertexArraysDelegate(int n, ref uint arrays);
    static readonly DeleteVertexArraysDelegate glDeleteVertexArrays;

    public static void DeleteVertexArray(uint array)
    {
        glDeleteVertexArrays(1, ref array);
    }

    #endregion

    #region Draw Arrays

    delegate void DrawArraysDelegate(uint mode, int first, int count);
    static readonly DrawArraysDelegate glDrawArrays;

    public static void DrawArrays(DrawMode mode, int first, int count)
    {
        glDrawArrays((uint)mode, first, count);
    }

    #endregion

    #region Draw Arrays Instanced

    delegate void DrawArraysInstancedDelegate(uint mode, int first, int count, int instanceCount);
    static readonly DrawArraysInstancedDelegate glDrawArraysInstanced;

    public static void DrawArraysInstanced(DrawMode mode, int first, int count, int instanceCount)
    {
        glDrawArraysInstanced((uint)mode, first, count, instanceCount);
    }

    #endregion

    #region Draw Elements

    delegate void DrawElementsDelegate(uint mode, int count, uint type, int indices);
    static readonly DrawElementsDelegate glDrawElements;

    public static void DrawElements(DrawMode mode, int count, DataType type, int indices)
    {
        glDrawElements((uint)mode, count, (uint)type, indices);
    }

    #endregion

    #region Enable

    delegate void EnableDelegate(GLCapability cap);
    static readonly EnableDelegate glEnable;

    public static void Enable(GLCapability cap)
    {
        glEnable(cap);
    }

    #endregion

    #region Enable Vertex Attrib Array

    delegate void EnableVertexAttribArrayDelegate(uint index);
    static readonly EnableVertexAttribArrayDelegate glEnableVertexAttribArray;

    public static void EnableVertexAttribArray(uint index)
    {
        glEnableVertexAttribArray(index);
    }

    #endregion

    #region Flush

    delegate void FlushDelegate();
    static readonly FlushDelegate glFlush;

    public static void Flush()
    {
        glFlush();
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
        
        return array;
    }

    #endregion

    #region Get Program Info Log

    delegate void GetProgramInfoLogDelegate(uint program, int maxLength, out int length, [Out] byte[] infoLog);
    static readonly GetProgramInfoLogDelegate glGetProgramInfoLog;

    public static void GetProgramInfoLog(uint program, int maxLength, out int length, byte[] infoLog)
    {
        glGetProgramInfoLog(program, maxLength, out length, infoLog);
    }

    #endregion

    #region Get Program Integer Vector

    delegate void GetProgramivDelegate(uint program, uint pname, out bool success);
    static readonly GetProgramivDelegate glGetProgramiv;

    public static void GetProgramiv(uint program, ParameterName pname, out bool success)
    {
        glGetProgramiv(program, (uint)pname, out success);
    }

    #endregion

    #region Get Shader Info Log

    unsafe delegate void GetShaderInfoLogDelegate(uint shader, int maxLength, out int length, byte* infoLog);
    static readonly GetShaderInfoLogDelegate glGetShaderInfoLog;

    public unsafe static void GetShaderInfoLog(uint shader, int maxLength, out int length, byte* infoLog)
    {
        glGetShaderInfoLog(shader, maxLength, out length, infoLog);
    }

    #endregion

    #region Get Shader Integer Vector

    delegate void GetShaderivDelegate(uint shader, uint pname, out bool success);
    static readonly GetShaderivDelegate glGetShaderiv;

    public static void GetShaderiv(uint shader, ParameterName pname, out bool success)
    {
        glGetShaderiv(shader, (uint)pname, out success);
    }

    #endregion

    #region Get Uniform Block Index

    delegate uint GetUniformBlockIndexDelegate(uint program, string name);
    static readonly GetUniformBlockIndexDelegate glGetUniformBlockIndex;

    /// <summary>
    /// Retrieves the internal index of the uniform block with the specified name. This index can be used to bind the uniform block to a binding point which is then used to bind a specific buffer to it.
    /// </summary>
    /// <param name="program">Id of the program object</param>
    /// <param name="name">Name of the uniform block</param>
    /// <returns>The internal index of the uniform block</returns>
    public static uint GetUniformBlockIndex(uint program, string name)
    {
        var index = glGetUniformBlockIndex(program, name);
        
        return index;
    }

    #endregion

    #region Get Uniform Location

    delegate int GetUniformLocationDelegate(uint program, string name);
    static readonly GetUniformLocationDelegate glGetUniformLocation;

    public static int GetUniformLocation(uint program, string name)
    {
        var location = glGetUniformLocation(program, name);
        
        return location;
    }

    #endregion

    #region Link Program

    delegate void LinkProgramDelegate(uint program);
    static readonly LinkProgramDelegate glLinkProgram;

    public static void LinkProgram(uint program)
    {
        glLinkProgram(program);
    }

    #endregion

    #region Shader Source

    delegate void ShaderSourceDelegate(uint shader, int count, string[] source, int[]? length);
    static readonly ShaderSourceDelegate glShaderSource;

    /// <summary>
    /// Sets the source code of the shader. The source code is not compiled until <see cref="CompileShader(uint)" /> is called.
    /// </summary>
    /// <param name="shader">The id of the shader object</param>
    /// <param name="count">Number of strings in the <paramref name="source"/> array which when combined form the source code</param>
    /// <param name="source">Array of strings that form the source code</param>
    /// <param name="length">Array of integers that specify the length of each string in the <paramref name="source" /> array</param>
    public static void ShaderSource(uint shader, string[] source, int[]? length = null)
    {
        glShaderSource(shader, source.Length, source, length);
    }

    #endregion

    #region Uniform Block Binding

    delegate void UniformBlockBindingDelegate(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
    static readonly UniformBlockBindingDelegate glUniformBlockBinding;

    /// <summary>
    /// Binds a uniform block to a binding point. The data in the buffer object bound to the binding point will be used for that uniform block.
    /// </summary>
    /// <param name="program">Id of the program object</param>
    /// <param name="uniformBlockIndex">Index of the uniform block</param>
    /// <param name="uniformBlockBinding">Binding point to which the uniform block should be bound</param>
    public static void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding)
    {
        glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
    }

    #endregion

    #region Use Program

    delegate void UseProgramDelegate(uint program);
    static readonly UseProgramDelegate glUseProgram;

    public static void UseProgram(uint program)
    {
        glUseProgram(program);
    }

    #endregion

    #region Vertex Attrib Divisor

    delegate void VertexAttribDivisorDelegate(uint index, uint divisor);
    static readonly VertexAttribDivisorDelegate glVertexAttribDivisor;

    public static void VertexAttribDivisor(uint index, uint divisor)
    {
        glVertexAttribDivisor(index, divisor);
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
    }

    #endregion

    #region Viewport

    delegate void ViewportDelegate(int x, int y, int width, int height);
    static readonly ViewportDelegate glViewport;

    public static void Viewport(int x, int y, int width, int height)
    {
        glViewport(x, y, width, height);
    }

    #endregion

    #region Uniform 2 Float

    delegate void Uniform2fDelegate(int location, float v0, float v1);
    static readonly Uniform2fDelegate glUniform2f;

    public static void Uniform2f(int location, float v0, float v1)
    {
        glUniform2f(location, v0, v1);
    }

    #endregion

    #region Uniform 4 Float

    delegate void Uniform4fDelegate(int location, float v0, float v1, float v2, float v3);
    static readonly Uniform4fDelegate glUniform4f;

    public static void Uniform4f(int location, float v0, float v1, float v2, float v3)
    {
        glUniform4f(location, v0, v1, v2, v3);
    }

    #endregion

    #region Check Errors

    delegate uint GetErrorDelegate();
    static readonly GetErrorDelegate glGetError;

    private static void ErrorCallback(ErrorSource source, ErrorType type, uint id, ErrorSeverety severity, int length, string message, IntPtr userParam)
    {
        Console.WriteLine($"OpenGL Error: {message}");
        Debug.Assert(false, $"OpenGL Error: {message}");
    }

    private static DebugProc debugProc = ErrorCallback; // Keep a reference to the delegate so it doesn't get garbage collected

    #endregion
}