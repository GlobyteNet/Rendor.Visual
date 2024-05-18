using Rendor.Visual.Drawing;
using Rendor.Visual.GUI;

namespace Rendor.Visual.Rendering;

/// <summary>
/// Represents the drawing API.
/// </summary>
public abstract class GraphicsDevice : IDisposable
{
    public abstract void Clear();

    public abstract void SetViewport(int width, int height);

    public abstract void Render(Surface surface);

    public abstract void Dispose();

    public abstract (float, float) U_Resolution { set; }
}
