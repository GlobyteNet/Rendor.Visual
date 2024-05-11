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

    public float r, g, b, a;
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
}

class OpenGLProgram
{
    [STAThread]
    public static void Run()
    {
        var window = Window.Create(BackendType.OpenGL);
        window.Show();

        var vertices = new Point[]
        {
            new Point(0.0f, 0.0f, 0.0f),
            new Point(761.0f, 0.0f, 0.0f),
            new Point(0.0f, 553.0f, 0.0f),
            new Point(0.0f, 0.0f, 0.0f)
        };

        window.Surface.DrawTriangle(vertices[0], vertices[1], vertices[2]);
        window.Surface.DrawTriangle(vertices[1], vertices[2], vertices[3]);

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

            window.GraphicsDevice.U_Color = color;

            window.GraphicsDevice.Clear();

            window.SwapBuffers();

            sw.Stop();
            //Console.WriteLine("Elapsed: " + sw.ElapsedTicks);
        }

        // Cleanup
        window.Dispose();
    }
}