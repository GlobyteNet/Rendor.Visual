using CorePlayground.Main.GUI;
using CorePlayground.Main.GUI.Rendering.OpenGL;
using CorePlayground.Main.GUI.WM;
using System.Diagnostics;
using System.Text;

namespace CorePlayground.Main.OpenGL
{
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

    public class Surface
    {
        internal List<Point> points = new List<Point>();

        public void DrawTriangle(Point a, Point b, Point c)
        {
            points.Add(MapCoordsToScreen(a));
            points.Add(MapCoordsToScreen(b));
            points.Add(MapCoordsToScreen(c));
        }

        private Point MapCoordsToScreen(Point point)
        {
            Debug.Assert(Width > 0 && Height > 0, "Width and Height must be greater than 0");

            var normalized = new Point
            {
                X = (2.0f * point.X) / Width - 1.0f,
                Y = 1.0f - (2.0f * point.Y) / Height,
                Z = point.Z
            };

            return normalized;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }

    public interface IRenderer
    {
        Surface Surface { get; }
        void Execute();
    }

    public class GLRenderer : IRenderer
    {
        static GLRenderer()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public GLRenderer()
        {
            vertexArray.AddPoint3f(vertexBuffer, 0);
            vertexArray.Build();
        }

        /// <summary>
        /// Clears the color buffer without changing the surface.
        /// </summary>
        public void ClearColor()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Execute()
        {
            vertexBuffer.BufferData(Surface.points.ToArray(), BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);

            program.Use();
            vertexArray.Bind();

            GL.DrawArrays(DrawMode.Triangles, 0, Surface.points.Count);

            vertexArray.Unbind();
        }

        public Color U_Color
        {
            set => program.U_Color = value;
        }

        public Surface Surface { get; } = new Surface();

        private GLBuffer<Point> vertexBuffer = new GLBuffer<Point>();
        private VertexArray vertexArray = new VertexArray();

        private MainGlProgram program = new MainGlProgram();
    }

    class OpenGLProgram
    {
        [STAThread]
        public static void Run()
        {
            var window = Window.Create();
            window.Show();

            var vertices = new Point[]
            {
                new Point(0.0f, 0.0f, 0.0f),
                new Point(761.0f, 0.0f, 0.0f),
                new Point(0.0f, 553.0f, 0.0f),
                new Point(100.0f, 100.0f, 0.0f)
            };

            var renderer = new GLRenderer();
            var surface = renderer.Surface;
            surface.Width = window.Width;
            surface.Height = window.Height;

            surface.DrawTriangle(vertices[0], vertices[1], vertices[2]);
            surface.DrawTriangle(vertices[1], vertices[2], vertices[3]);

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

                renderer.U_Color = color;

                renderer.ClearColor();

                renderer.Execute();

                window.SwapBuffers();

                sw.Stop();
                Console.WriteLine("Elapsed: " + sw.ElapsedTicks);
            }

            // Cleanup
            window.Dispose();
        }
    }

    enum DWFlags : uint
    {
        PFD_DRAW_TO_WINDOW = 0x00000004,
        PFD_SUPPORT_OPENGL = 0x00000020,
        PFD_DOUBLEBUFFER = 0x00000001,
        PFD_TYPE_RGBA = 0
    }
}
