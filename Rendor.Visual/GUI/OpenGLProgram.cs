using Rendor.Visual.Drawing;
using Rendor.Visual.Rendering.OpenGL;
using Rendor.Visual.Windowing;
using System.Diagnostics;

namespace Rendor.Visual.GUI;

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

    public static Color operator+(Color a) => a;
    public static Color operator-(Color a) => new Color(1.0f - a.r, 1.0f - a.g, 1.0f - a.b, 1.0f - a.a);
}

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

    public override string ToString()
    {
        return $"({X}, {Y}";
    }
}

class OpenGLProgram
{
    [STAThread]
    public static void Run()
    {
        var window = NativeWindow.Create(BackendType.OpenGL);
        window.Show();

        var vertices = new Point[]
        {
            new Point(0.0f, 0.0f, 0.0f),
            new Point(761.0f, 0.0f, 0.0f),
            new Point(0.0f, 553.0f, 0.0f),
            new Point(761.0f, 553.0f, 0.0f)
        };

        window.Surface.FillTriangle(vertices[0], vertices[1], vertices[2], SolidPaint.Red);
        window.Surface.FillTriangle(vertices[1], vertices[2], vertices[3], SolidPaint.Blue);

        window.Surface.FillRectangle(new Point(100.0f, 100.0f, 0.0f), new Point(200.0f, 200.0f, 0.0f), SolidPaint.Green);

        var color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        var sw = new Stopwatch();

        while (window.IsVisible)
        {
            sw.Restart();

            window.PollEvents();

            // slowly change the red color
            if (color.r >= 1.0f)
                color.r = 0.0f;
            else
                color.r += 0.01f;

            //window.GraphicsDevice.U_Color = color;

            window.GraphicsDevice.Clear();

            window.SwapBuffers();

            sw.Stop();
            //Console.WriteLine("Elapsed: " + sw.ElapsedTicks);
        }

        // Cleanup
        window.Dispose();
    }
}