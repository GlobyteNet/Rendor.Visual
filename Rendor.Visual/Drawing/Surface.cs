using System.Diagnostics;
using System.Numerics;

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
        var xBasis = b - a;
        // calculate the perpendicular vector of length 1
        var yBasis = Point.Normalize(new Point(-xBasis.Y, xBasis.X, 0.0f));

        var points = new Point[6];
        for (int i = 0; i < 6; i++)
        {
            // calculate the points of the line by using instance data
            points[i] = a + xBasis * LineInstance[i].X + yBasis * paint.LineWidth * LineInstance[i].Y;
        }

        FillTriangle(points[0], points[1], points[2], paint);
        FillTriangle(points[3], points[4], points[5], paint);
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
            FillCircle(b, paint.LineWidth, paint);
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

    public void FillCircle(Point center, float radius, Paint paint)
    {
        for (int i = 0; i < CircleInstance.Length - 1; i++)
        {
            var a = center + CircleInstance[i] * radius;
            var b = center + CircleInstance[i + 1] * radius;

            FillTriangle(center, a, b, paint);
        }
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

    private static Point[] GetCircleGeometry(int resolution)
    {
        var points = new Point[resolution + 2];
        points[0] = new Point(0.0f, 0.0f, 0.0f);

        for (int i = 0; i <= resolution; i++)
        {
            var angle = 2.0f * MathF.PI * i / resolution;
            points[i + 1] = new Point(0.5f * MathF.Cos(angle), 0.5f * MathF.Sin(angle), 0.0f);
        }

        return points;
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

    private Point[] LineInstance =
    [
        new Point(0.0f, -0.5f, 0.0f),
        new Point(1.0f, -0.5f, 0.0f),
        new Point(1.0f, 0.5f, 0.0f),
        new Point(0.0f, -0.5f, 0.0f),
        new Point(1.0f, 0.5f, 0.0f),
        new Point(0.0f, 0.5f, 0.0f)
    ];

    private Point[] CircleInstance = GetCircleGeometry(16);
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
