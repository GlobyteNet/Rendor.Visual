namespace Rendor.Visual.Rendering.OpenGL;

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
    ElementArrayBuffer = 0x8893,
    UniformBuffer = 0x8A11,
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

public enum GLCapability
{
    Multisample = 0x809D,
    DebugOutput = 0x92E0
}

public enum ErrorSource : uint
{
    /// <summary> Calls to the OpenGL API </summary>
    API = 0x8246,
    /// <summary> Calls to a window-system API </summary>
    WindowSystem = 0x8247,
    /// <summary> A compiler for a shading language </summary>
    ShaderCompiler = 0x8248,
    /// <summary> An application associated with the OpenGL </summary>
    ThirdParty = 0x8249,
    /// <summary> The Application itself </summary>
    Application = 0x824A,
    /// <summary> Some other source </summary>
    Other = 0x824B
}

public enum ErrorType : uint
{
    /// <summary> An error which would cause the OpenGL state to be invalid </summary>
    Error = 0x824C,
    /// <summary> A deprecation warning </summary>
    DeprecatedBehavior = 0x824D,
    /// <summary> Undefined behavior </summary>
    UndefinedBehavior = 0x824E,
    /// <summary> Implementation dependent behavior </summary>
    Portability = 0x824F,
    /// <summary> Potential performance problems </summary>
    Performance = 0x8250,
    /// <summary> Some other type </summary>
    Other = 0x8251,
    /// <summary> Command stream annotation </summary>
    Marker = 0x8268,
    /// <summary> Group pushing </summary>
    PushGroup = 0x8269,
    /// <summary> Group popping </summary>
    PopGroup = 0x826A,
}

public enum ErrorSeverety : uint
{
    /// <summary> All OpenGL errors </summary>
    High = 0x9146,
    /// <summary> Severe performance warnings, shader compilation/linking warnings, or the use of deprecated functionality </summary>
    Medium = 0x9147,
    /// <summary>  Redundant state change performance warning or other minor performance warnings </summary>
    Low = 0x9148,
    /// <summary> Any message that isn't an error </summary>
    Notification = 0x826B
}