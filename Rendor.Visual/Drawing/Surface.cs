using Rendor.Visual.Drawing.Commands;
using Rendor.Visual.Rendering.OpenGL;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Rendor.Visual.Drawing;

public class Surface
{
    internal List<DrawCommand> drawCommands = new();
    internal List<ColorPoint> points = new List<ColorPoint>();

    public void DrawPath(Path path, Paint paint)
    {
        DrawLineCap(path.Points[1], path.Points[0], paint);

        for (int i = 0; i < path.Points.Count - 1; i++)
        {
            DrawLine(path.Points[i], path.Points[i + 1], paint);

            if (path.Points.Count > 2 && i < path.Points.Count - 1 && i > 0)
            {
                DrawLineJoint(path.Points[i - 1], path.Points[i], path.Points[i + 1], paint);
            }
        }

        DrawLineCap(path.Points[^2], path.Points[^1], paint);
    }

    public void DrawLine(Point a, Point b, Paint paint)
    {
        if (TryGetLastCommand<DrawLineCommand>(out var lastCommand))
        {
            lastCommand.Lines.Add(new Line(a, b, paint.Color, paint.LineWidth));
        }
        else
        {
            var command = new DrawLineCommand();
            command.Lines.Add(new Line(a, b, paint.Color, paint.LineWidth));
            drawCommands.Add(command);
        }
        return;

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

    private void DrawLineCap(Point a, Point b, Paint paint)
    {
        if (paint.LineCap == LineCap.Butt)
        {
            return;
        }
        else if (paint.LineCap == LineCap.Round)
        {
            FillCircle(a, paint.LineWidth, paint);
        }
        else
        {
            DrawLineCapSquare(a, b, paint);
        }
    }

    private void DrawLineCapSquare(Point a, Point b, Paint paint)
    {
        Point xBasis = Point.Normalize(b - a);
        Point yBasis = new Point(-xBasis.Y, xBasis.X, 0.0f);
        var points = new Point[6];

        for (int i = 0; i < 6; i++)
        {
            points[i] = b + xBasis * LineInstance[i].X * paint.LineWidth / 2 + yBasis * paint.LineWidth * LineInstance[i].Y;
        }

        FillTriangle(points[0], points[1], points[2], paint);
        FillTriangle(points[3], points[4], points[5], paint);
    }

    private void DrawLineJoint(Point a, Point b, Point c, Paint paint)
    {
        if (paint.LineJoin == LineJoin.Bevel)
        {
            DrawLineJoinBevel(a, b, c, paint);
        }
        else if (paint.LineJoin == LineJoin.Miter)
        {
            DrawLineJoinMiter(a, b, c, paint);
        }
        else if (paint.LineJoin == LineJoin.Round)
        {
            FillCircle(b, paint.LineWidth, paint);
        }
    }

    private void DrawLineJoinMiter(Point a, Point b, Point c, Paint paint)
    {
        var tangent = Point.Normalize(Point.Normalize(c - b) + Point.Normalize(b - a));
        var miter = new Point(-tangent.Y, tangent.X, 0.0f);

        if (float.IsNaN(miter.X))
        {
            return;
        }

        var ab = b - a;
        var bc = b - c;
        var abNorm = Point.Normalize(new Point(-ab.Y, ab.X));
        var bcNorm = -Point.Normalize(new Point(-bc.Y, bc.X));

        var sigma = MathF.Sign(Point.Dot(ab + bc, miter));

        var p0 = 0.5f * paint.LineWidth * sigma * (sigma < 0 ? abNorm : bcNorm);
        var p1 = 0.5f * miter * sigma * (paint.LineWidth / Point.Dot(miter, abNorm));
        var p2 = 0.5f * paint.LineWidth * sigma * (sigma < 0 ? bcNorm : abNorm);

        var points = new Point[6];

        for ( var i = 0; i < 6; i++ )
        {
            points[i] = b + MiterJoin[i].X * p0 + MiterJoin[i].Y * p1 + MiterJoin[i].Z * p2;
        }

        FillTriangle(points[0], points[1], points[2], paint);
        FillTriangle(points[3], points[4], points[5], paint);
    }

    private void DrawLineJoinBevel(Point a, Point b, Point c, Paint paint)
    {
        var tangent = Point.Normalize(Point.Normalize(c - b) + Point.Normalize(b - a));
        var normal = new Point(-tangent.Y, tangent.X, 0.0f);

        var ab = b - a;
        var bc = b - c;
        var abNorm = Point.Normalize(new Point(-ab.Y, ab.X));
        var bcNorm = -Point.Normalize(new Point(-bc.Y, bc.X));

        var sigma = MathF.Sign(Point.Dot(ab + bc, normal));

        var p0 = 0.5f * paint.LineWidth * sigma * (sigma < 0 ? abNorm : bcNorm);
        var p1 = 0.5f * paint.LineWidth * sigma * (sigma < 0 ? bcNorm : abNorm);

        var points = new Point[3];
        for ( var i = 0; i < 3; i++ )
        {
            points[i] = b + BevelJoin[i].X * p0 + BevelJoin[i].Y * p1;
        }

        FillTriangle(points[0], points[1], points[2], paint);
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

    private bool TryGetLastCommand<T>([NotNullWhen(true)] out T? command) where T : DrawCommand
    {
        if (drawCommands.Count == 0)
        {
            command = null;
            return false;
        }

        var lastCommand = drawCommands[^1];

        if (lastCommand is T tCommand)
        {
            command = tCommand;
            return true;
        }
        
        command = null;
        return false;
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

    private Point[] MiterJoin = [
        new Point(0.0f, 0.0f, 0.0f),
        new Point(1.0f, 0.0f, 0.0f),
        new Point(0.0f, 1.0f, 0.0f),
        new Point(0.0f, 0.0f, 0.0f),
        new Point(0.0f, 1.0f, 0.0f),
        new Point(0.0f, 0.0f, 1.0f)
    ];

    private Point[] BevelJoin = [
        new Point(0.0f, 0.0f, 0.0f),
        new Point(1.0f, 0.0f, 0.0f),
        new Point(0.0f, 1.0f, 0.0f),
    ];

    private Point[] CircleInstance = GetCircleGeometry(16);
}

[StructLayout(LayoutKind.Sequential)]
public struct Line
{
    public Line(Point start, Point end, Color color, float width)
    {
        Start = new Point2(start.X, start.Y);
        End = new Point2(end.X, end.Y);
        Color = color;
        Width = width;
    }

    public Point2 Start { get; set; }
    public Point2 End { get; set; }

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
