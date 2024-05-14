namespace Rendor.Visual.Drawing;

public static class SolidPaint
{
    public static Paint Red => new Paint { Color = new Color(1.0f, 0.0f, 0.0f) };
    public static Paint Green => new Paint { Color = new Color(0.0f, 1.0f, 0.0f) };
    public static Paint Blue => new Paint { Color = new Color(0.0f, 0.0f, 1.0f) };
}
