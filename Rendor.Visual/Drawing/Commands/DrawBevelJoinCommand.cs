namespace Rendor.Visual.Drawing.Commands;

internal class DrawBevelJoinCommand : DrawCommand
{
    public override DrawCommandType Type => DrawCommandType.BevelJoin;

    public List<LineSegment> LineSegments { get; set; } = new();
}

public struct LineSegment
{
    public LineSegment(Point start, Point middle, Point end, Color color, float width)
    {
        Start = new Vector2f(start.X, start.Y);
        Middle = new Vector2f(middle.X, middle.Y);
        End = new Vector2f(end.X, end.Y);
        Color = color;
        Width = width;
    }

    public Vector2f Start { get; set; }

    public Vector2f Middle { get; set; }

    public Vector2f End { get; set; }

    public Color Color { get; set; }

    public float Width { get; set; }
}
