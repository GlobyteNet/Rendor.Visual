using System.Diagnostics;
using System.Runtime.InteropServices;
using Rendor.Visual.GUI;

namespace Rendor.Visual.Drawing;

public class Surface
{
    internal List<ColorPoint> points = new List<ColorPoint>();

    public void DrawPath(Path path, Paint paint)
    {
        for (int i = 0; i < path.Points.Count - 1; i++)
        {
            DrawLine(path.Points[i], path.Points[i + 1], paint);

            if (path.Points.Count > 2 && i < path.Points.Count - 1 && i > 0)
            {
                DrawLineJoint(path.Points[i - 1], path.Points[i], path.Points[i + 1], paint);
            }
        }
    }

    public void DrawLine(Point a, Point b, Paint paint)
    {
        var p1 = GetPerpendicularPoints(a, b, paint.LineWidth / 2.0f);
        var p2 = GetPerpendicularPoints(b, a, paint.LineWidth / 2.0f);

        FillTriangle(p1.Item1, p1.Item2, p2.Item1, paint);
        FillTriangle(p1.Item1, p2.Item1, p2.Item2, paint);
    }

    private void DrawLineJoint(Point a, Point b, Point c, Paint paint)
    {
        var p1 = GetPerpendicularPoints(b, a, paint.LineWidth / 2.0f);
        var p2 = GetPerpendicularPoints(b, c, paint.LineWidth / 2.0f);

        var angle = Angle(a, b, c);
        var newPaint = new Paint { Color = -paint.Color, LineWidth = paint.LineWidth };

        if (paint.LineJoin == LineJoin.Bevel)
        {
            if (angle > 0.0f)
            {
                FillTriangle(p1.Item1, p2.Item2, b, newPaint);
            }
            else
            {
                FillTriangle(p1.Item2, p2.Item1, b, newPaint);
            }
        }
        else if (paint.LineJoin == LineJoin.Miter)
        {
            var p3 = GetPerpendicularPoints(a, b, paint.LineWidth /2.0f);
            var p4 = GetPerpendicularPoints(c, b, paint.LineWidth / 2.0f);

            // Calculate the intersection point of the two lines
            if (angle > 0.0f)
            {
                // find intersection of p1.Item1 -> p3.Item1 and p2.Item2 -> p4.Item2
                var m1 = (p3.Item2.Y - p1.Item1.Y) / (p3.Item1.X - p1.Item1.X);
                var m2 = (p4.Item1.Y - p2.Item1.Y) / (p4.Item1.X - p2.Item2.X);

                var x = (m1 * p1.Item1.X - p1.Item1.Y - m2 * p2.Item2.X + p2.Item2.Y) / (m1 - m2);
                var y = m1 * (x - p1.Item1.X) + p1.Item1.Y;

                var p5 = new Point(x, y, 0.0f);

                FillTriangle(p1.Item1, p2.Item2, p5, newPaint);
            }
            else
            {
            }
        }
        else if (paint.LineJoin == LineJoin.Round)
        {
            var distance = paint.LineWidth / 2.0f / MathF.Tan(angle / 2.0f);
            var p3 = new Point(b.X + distance * (a.X - b.X), b.Y + distance * (a.Y - b.Y), 0.0f);

            FillTriangle(p1.Item1, p2.Item2, p3, newPaint);
            FillTriangle(p1.Item2, p2.Item1, p3, newPaint);
        }
    }

    private float Distance(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;

        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    private float Angle(Point a, Point b, Point c)
    {
        return MathF.Atan2(c.Y - b.Y, c.X - b.X) - MathF.Atan2(a.Y - b.Y, a.X - b.X);
    }

    public void DrawTriangle(Point a, Point b, Point c, Paint paint)
    {
        DrawLine(a, b, paint);
        DrawLine(b, c, paint);
        DrawLine(c, a, paint);
    }

    public void DrawRectangle(Point a, Point b, Paint paint)
    {
        var c = new Point(b.X, a.Y, 0.0f);
        var d = new Point(a.X, b.Y, 0.0f);

        DrawLine(a, c, paint);
        DrawLine(a, d, paint);
        DrawLine(b, c, paint);
        DrawLine(b, d, paint);
    }

    public void FillRectangle(Point a, Point b, Paint paint)
    {
        var c = new Point(b.X, a.Y, 0.0f);
        var d = new Point(a.X, b.Y, 0.0f);

        FillTriangle(a, b, c, paint);
        FillTriangle(a, b, d, paint);
    }

    public void FillTriangle(Point a, Point b, Point c, Paint paint)
    {
        points.Add(new ColorPoint(paint.Color, a));
        points.Add(new ColorPoint(paint.Color, b));
        points.Add(new ColorPoint(paint.Color, c));
    }

    /// <summary>
    /// Calculates 2 points that are perpendicular to the line defined by a and b and are distance away from a point in both directions.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private (Point, Point) GetPerpendicularPoints(Point a, Point b, float distance)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;

        var length = (float)Math.Sqrt(dx * dx + dy * dy);

        var nx = dx / length;
        var ny = dy / length;

        var px = a.X + distance * ny;
        var py = a.Y - distance * nx;

        var p1 = new Point(px, py, 0.0f);

        px = a.X - distance * ny;
        py = a.Y + distance * nx;

        var p2 = new Point(px, py, 0.0f);

        return (p1, p2);
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

public class Path
{
    public List<Point> Points { get; set; } = new List<Point>();
}

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

public class Paint
{
    public Color Color { get; set; }
    public float LineWidth { get; set; } = 10.0f;
    public LineJoin LineJoin { get; set; } = LineJoin.Bevel;
}

public enum LineJoin
{
    Miter,
    Bevel,
    Round
}

public static class SolidPaint
{
    public static Paint Red => new Paint { Color = new Color(1.0f, 0.0f, 0.0f) };
    public static Paint Green => new Paint { Color = new Color(0.0f, 1.0f, 0.0f) };
    public static Paint Blue => new Paint { Color = new Color(0.0f, 0.0f, 1.0f) };
}

public struct Line
{
    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
    }

    public Point Start { get; set; }
    public Point End { get; set; }

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
