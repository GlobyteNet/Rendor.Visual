namespace Rendor.Visual.Drawing;

public class Paint
{
    public Color Color { get; set; }
    public float LineWidth { get; set; } = 10.0f;
    public LineJoin LineJoin { get; set; } = LineJoin.Bevel;
    public LineCap LineCap { get; set; } = LineCap.Butt;
}
