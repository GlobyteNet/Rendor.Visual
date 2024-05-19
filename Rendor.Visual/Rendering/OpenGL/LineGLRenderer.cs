using Rendor.Visual.Drawing;
using Rendor.Visual.Drawing.Commands;
using System.Runtime.InteropServices;

namespace Rendor.Visual.Rendering.OpenGL
{
    internal class LineGLRenderer
    {
        public LineGLRenderer()
        {
            program = new LineGLProgram();
            vertexBuffer = new GLBuffer<Line>();
            lineInstanceBuffer = new GLBuffer<Point2>();
            vertexArray = new VertexArray();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            vertexArray.AddPoint2f(lineInstanceBuffer, 0, 8, 0); // instance data
            vertexArray.AddPoint2f(vertexBuffer, 1, 36, 0, 1); // position
            vertexArray.AddPoint2f(vertexBuffer, 2, 36, 8, 1); // position
            vertexArray.AddPoint4f(vertexBuffer, 3, 36, 16, 1); // color
            vertexArray.AddPoint1f(vertexBuffer, 4, 36, 32, 1); // width
            vertexArray.Build();

            lineInstanceBuffer.BufferData(LineInstance, BufferTarget.ArrayBuffer, BufferUsage.StaticDraw);
        }

        public void Render(DrawLineCommand command)
        {
            //var points = ToPoints(command);
            vertexBuffer.BufferData(command.Lines.ToArray(), BufferTarget.ArrayBuffer, BufferUsage.DynamicDraw);

            program.Use();
            vertexArray.Bind();

            GL.DrawArraysInstanced(DrawMode.Triangles, 0, 6, command.Lines.Count);

            vertexArray.Unbind();
        }

        public (float, float) U_Resolution
        {
            set => program.U_Resolution = value;
        }

        //private ColorPoint[] ToPoints(DrawLineCommand command)
        //{
        //    var points = new List<ColorPoint>();

        //    for (var i = 0; i < command.Lines.Count; i++)
        //    {
        //        var a = command.Lines[i].Start;
        //        var b = command.Lines[i].End;

        //        var xBasis = b - a;
        //        // calculate the perpendicular vector of length 1
        //        var yBasis = Point.Normalize(new Point(-xBasis.Y, xBasis.X, 0.0f));

        //        for (int j = 0; j < 6; j++)
        //        {
        //            // calculate the points of the line by using instance data
        //            var point = a + xBasis * LineInstance[j].X + yBasis * command.Lines[i].Width * LineInstance[j].Y;
        //            points.Add(new ColorPoint(command.Lines[i].Color, point));
        //        }

        //    }

        //    return points.ToArray();
        //}

        private GLBuffer<Line> vertexBuffer;
        private GLBuffer<Point2> lineInstanceBuffer;
        private VertexArray vertexArray;
        private LineGLProgram program;

        private Point2[] LineInstance =
        [
            new Point2(0.0f, -0.5f),
            new Point2(1.0f, -0.5f),
            new Point2(1.0f, 0.5f),
            new Point2(0.0f, -0.5f),
            new Point2(1.0f, 0.5f),
            new Point2(0.0f, 0.5f)
        ];
    }

    public struct LinePoint
    {
        public LinePoint(Point point, Color color, float width)
        {
            Point = point;
            Color = color;
            Width = width;
        }

        public Point Point { get; private set; }
            
        public Color Color { get; private set; }

        public float Width { get; private set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point2
    {
        public Point2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
