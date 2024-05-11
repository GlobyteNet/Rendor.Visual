using Rendor.Visual.Drawing;
using Rendor.Visual.Rendering;
using Rendor.Visual.Rendering.OpenGL;
using Rendor.Visual.Windowing.Windows;
using System.Runtime.InteropServices;

namespace Rendor.Visual.Windowing;

public abstract class Window : IDisposable
{
    public static Window Create(BackendType backend)
    {
        Window window;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            window = new WindowsWindow();
        }
        else
        {
            throw new PlatformNotSupportedException("Windows only");
        }

        GraphicsDevice graphicsDevice;
        if (backend == BackendType.OpenGL)
        {
            graphicsDevice = new GLGraphicsDevice(window);
        }
        else
        {
            throw new PlatformNotSupportedException("OpenGL only");
        }

        window.GraphicsDevice = graphicsDevice;

        return window;
    }

    public abstract void Show();

    /// <summary>
    /// Polls for window events.
    /// </summary>
    public abstract void PollEvents();

    public abstract void SwapBuffers();

    public abstract void Dispose();

    public Surface Surface { get; init; } = new Surface();

    public abstract GraphicsDevice GraphicsDevice { get; protected set; }

    public abstract bool IsVisible { get; }

    public int Width { get; protected set; }

    public int Height { get; protected set; }
}

public enum BackendType
{
    OpenGL,
    Vulkan,
    DirectX
}
