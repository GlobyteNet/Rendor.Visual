﻿namespace Rendor.Visual.Drawing;

public struct Point
{
    public Point(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public static Point operator+(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Point operator-(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Point operator*(Point a, float b) => new Point(a.X * b, a.Y * b, a.Z * b);
    public static Point operator/(Point a, float b) => new Point(a.X / b, a.Y / b, a.Z / b);

    public static Point Normalize(Point a)
    {
        var length = MathF.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        return new Point(a.X / length, a.Y / length, a.Z / length);
    }

    public override string ToString()
    {
        return $"({X}, {Y}";
    }
}
