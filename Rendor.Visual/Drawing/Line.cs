using System.Runtime.InteropServices;

namespace Rendor.Visual.Drawing;

[StructLayout(LayoutKind.Sequential)]
public struct Line
{
    public Line(Point start, Point end, Color color, float width)
    {
        Start = start;
        End = end;
        Color = color;
        Width = width;
    }

    public Point Start { get; set; }
    public Point End { get; set; }

    public Color Color { get; set; }
    public float Width { get; set; }

    public Point Intersect(Line another)
    {
        var x1 = Start.X;
        var y1 = Start.Y;
        var x2 = End.X;
        var y2 = End.Y;

        var x3 = another.Start.X;
        var y3 = another.Start.Y;
        var x4 = another.End.X;
        var y4 = another.End.Y;

        var d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

        if (d == 0)
        {
            return new Point(0.0f, 0.0f, 0.0f);
        }

        var x = ((x3 - x4) * (x1 * y2 - y1 * x2) - (x1 - x2) * (x3 * y4 - y3 * x4)) / d;
        var y = ((y3 - y4) * (x1 * y2 - y1 * x2) - (y1 - y2) * (x3 * y4 - y3 * x4)) / d;

        return new Point(x, y, 0.0f);
    }
}
