using System.Diagnostics;
using Rendor.Visual.GUI;

namespace Rendor.Visual.Drawing;

public class Surface
{
    internal List<Point> points = new List<Point>();

    public void DrawTriangle(Point a, Point b, Point c)
    {
        //points.Add(MapCoordsToScreen(a));
        //points.Add(MapCoordsToScreen(b));
        //points.Add(MapCoordsToScreen(c));
        points.Add(a);
        points.Add(b);
        points.Add(c);
    }

    private Point MapCoordsToScreen(Point point)
    {
        Debug.Assert(Width > 0 && Height > 0, "Width and Height must be greater than 0");

        var normalized = new Point
        {
            X = 2.0f * point.X / Width - 1.0f,
            Y = 1.0f - 2.0f * point.Y / Height,
            Z = point.Z
        };

        return normalized;
    }

    public int Width { get; set; }
    public int Height { get; set; }
}
