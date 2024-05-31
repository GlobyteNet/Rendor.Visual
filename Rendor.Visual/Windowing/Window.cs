using Rendor.Visual.Drawing;
using System.Diagnostics;

namespace Rendor.Visual.Windowing
{
    /// <summary>
    /// Overridable class for handling window events and rendering.
    /// </summary>
    internal abstract class Window
    {
        public Window()
        {
            nativeWindow = NativeWindow.Create(BackendType.OpenGL);
            nativeWindow.OnPaint = Paint;
            nativeWindow.Show();
        }

        public void Run()
        {
            while (nativeWindow.IsVisible)
            {
                var hadEvents = nativeWindow.PollEvents();

                if (!hadEvents)
                {
                    Thread.Sleep(0);
                    OnIdle();
                }
            }
        }

        public abstract void OnRender(Surface surface);

        public abstract void OnIdle();

        public void Invalidate()
        {
            nativeWindow.Invalidate();
        }

        private void Paint()
        {
            var sw = new Stopwatch();
            sw.Start();
            OnRender(nativeWindow.Surface);
            sw.Stop();
            Console.WriteLine($"Render time: {sw.ElapsedMilliseconds}ms");
            sw.Restart();
            nativeWindow.SwapBuffers();
            sw.Stop();
            Console.WriteLine($"SwapBuffers time: {sw.ElapsedMilliseconds}ms");
            nativeWindow.Surface.Clear();
        }

        private NativeWindow nativeWindow;
    }
}
