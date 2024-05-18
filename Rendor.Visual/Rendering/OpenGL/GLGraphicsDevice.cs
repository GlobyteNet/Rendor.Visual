using Rendor.Visual.Drawing;
using Rendor.Visual.GUI;
using Rendor.Visual.Windowing;
using Rendor.Visual.Windowing.Windows;

namespace Rendor.Visual.Rendering.OpenGL;

internal class GLGraphicsDevice : GraphicsDevice
{
    static GLGraphicsDevice()
    {
    }

    public GLGraphicsDevice(NativeWindow window)
    {
        InitExtension(window);
        SetViewport(window.Width, window.Height);

        program = new MeshGlProgram();
        vertexBuffer = new GLBuffer<ColorPoint>();
        vertexArray = new VertexArray();

        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        vertexArray.AddPoint3f(vertexBuffer, 0, 28, 0); // position
        vertexArray.AddPoint4f(vertexBuffer, 1, 28, 12); // color
        vertexArray.Build();
    }

    public override void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    public override void SetViewport(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
    }

    public override (float, float) U_Resolution
    {
        set => program.U_Resolution = value;
    }

    public override void Render(Surface surface)
    {
        vertexBuffer.BufferData(surface.points.ToArray(), BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);

        program.Use();
        vertexArray.Bind();

        GL.DrawArrays(DrawMode.Triangles, 0, surface.points.Count);

        vertexArray.Unbind();
    }

    public override void Dispose()
    {
        vertexBuffer.Dispose();
        vertexArray.Dispose();
        program.Dispose();
    }

    private void InitExtension(NativeWindow window)
    {
        if (window is WindowsWindow windowsWindow)
        {
            extension = new WGL(windowsWindow);
        }
        else
        {
            throw new PlatformNotSupportedException("Windows only");
        }
    }

    private GLExtension extension;

    private GLBuffer<ColorPoint> vertexBuffer;
    private VertexArray vertexArray;
    private MeshGlProgram program;
}

abstract class GLExtension
{
}
