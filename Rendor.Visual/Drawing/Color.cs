namespace Rendor.Visual.Drawing;

public struct Color
{
    public Color(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        a = 1.0f;
    }

    public float r, g, b, a;

    public static Color operator +(Color a) => a;
    public static Color operator -(Color a) => new Color(1.0f - a.r, 1.0f - a.g, 1.0f - a.b, 1.0f - a.a);
}
