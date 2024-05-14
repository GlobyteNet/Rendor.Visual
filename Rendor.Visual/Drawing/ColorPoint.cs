using System.Runtime.InteropServices;

namespace Rendor.Visual.Drawing;

[StructLayout(LayoutKind.Sequential)] // This is required for the buffer data to work
public struct ColorPoint
{
    public ColorPoint(Color color, Point point)
    {
        Color = color;
        Point = point;
    }

    public Point Point { get; set; }
    public Color Color { get; set; }
}
