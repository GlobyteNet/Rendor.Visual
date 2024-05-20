using Rendor.Visual.Drawing;
using Rendor.Visual.Drawing.Commands;
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


        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        ubo = new GLBuffer<Vector2f>();
        ubo.BufferData(new Vector2f[1], BufferTarget.UniformBuffer, BufferUsage.DynamicDraw);

        GL.BindBufferBase(BufferTarget.UniformBuffer, UBO_BINDING, ubo.Id);

        lineRenderer = new LineGLRenderer(UBO_BINDING);
        meshRenderer = new MeshGLRenderer(UBO_BINDING);
    }

    public override void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    public override void SetViewport(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
    }

    public override Vector2f ScreenResolution
    {
        set
        {
            ubo.BufferData([value], BufferTarget.UniformBuffer, BufferUsage.DynamicDraw);
        }
    }

    public override void Render(Surface surface)
    {
        //Clear();

        foreach (var command in surface.drawCommands)
        {
            if (command is DrawLineCommand drawLineCommand)
            {
                lineRenderer.Render(drawLineCommand);
            } else if (command is DrawMeshCommand drawMeshCommand)
            {
                meshRenderer.Render(drawMeshCommand);
            }
            else
            {
                throw new NotImplementedException("Command not implemented");
            }
        }
    }

    public override void Dispose()
    {
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

    private LineGLRenderer lineRenderer;
    private MeshGLRenderer meshRenderer;

    private GLBuffer<Vector2f> ubo;
    private const int UBO_BINDING = 0;
}