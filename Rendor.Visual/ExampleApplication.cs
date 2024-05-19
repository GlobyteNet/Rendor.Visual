using Rendor.Visual.Drawing;
using Rendor.Visual.Windowing;

namespace Rendor.Visual
{
    internal class ExampleApplication : Application
    {
        public override void OnIdle()
        {
            throw new System.NotImplementedException();
        }

        public override void OnRender()
        {
        }
    }

    internal class ExampleWindow : Window
    {
        public override void OnIdle()
        {
            //Invalidate();
        }

        public override void OnRender(Surface surface)
        {
            //surface.FillTriangle(vertices[0], vertices[1], vertices[2], SolidPaint.Red);
            //surface.FillTriangle(vertices[1], vertices[2], vertices[3], SolidPaint.Blue);

            //surface.FillRectangle(new Point(100.0f, 100.0f, 0.0f), new Point(200.0f, 200.0f, 0.0f), SolidPaint.Green);

            var paint = new Paint { Color = new Color(1, 0, 0), LineWidth = 10.0f };

            for (int i = 0; i < 10_000_000; i++)
            {
                var start = new Point(Random.Shared.Next(0, 800), Random.Shared.Next(0, 600), 0.0f);
                var end = new Point(Random.Shared.Next(0, 800), Random.Shared.Next(0, 600), 0.0f);

                surface.DrawLine(start, end, paint);
            }

            //surface.DrawLine(new Point(00.0f, 00.0f, 0.0f), new Point(400.0f, 400.0f, 0.0f), new Paint { Color = new Color(1, 0, 1), LineWidth = 10.0f });
            //surface.DrawLine(new Point(400.0f, 400.0f, 0.0f), new Point(800.0f, 00.0f, 0.0f), new Paint { Color = new Color(0, 1, 1), LineWidth = 20.0f });
            //surface.DrawLine(new Point(800.0f, 00.0f, 0.0f), new Point(00.0f, 10.0f, 0.0f), new Paint { Color = new Color(1, 1, 0), LineWidth = 30.0f });

            //surface.DrawRectangle(new Point(300.0f, 120.0f, 0.0f), new Point(400.0f, 40.0f, 0.0f), new Paint { Color = new Color(1, 0, 1), LineWidth = 2.0f });

            //surface.DrawTriangle(new Point(500.0f, 200.0f, 0.0f), new Point(600.0f, 300.0f, 0.0f), new Point(700.0f, 200.0f, 0.0f), new Paint { Color = new Color(0, 1, 1), LineWidth = 21.0f });

            //var path = new Drawing.Path();
            //path.Points.Add(new Point(700.0f, 350.0f, 0.0f));
            //path.Points.Add(new Point(600.0f, 300.0f, 0.0f));
            //path.Points.Add(new Point(600.0f, 400.0f, 0.0f));
            //path.Points.Add(new Point(300.0f, 110.0f, 0.0f));
            //path.Points.Add(new Point(500.0f, 400.0f, 0.0f));

            //surface.DrawPath(path, new Paint { Color = new Color(0.9f, 0.6f, 0.1f), LineWidth = 40.0f, LineJoin = LineJoin.Bevel, LineCap = LineCap.Square });
        }

        Point[] vertices =
        [
            new Point(0.0f, 0.0f, 0.0f),
            new Point(761.0f, 0.0f, 0.0f),
            new Point(0.0f, 553.0f, 0.0f),
            new Point(761.0f, 553.0f, 0.0f)
        ];
    }
}
