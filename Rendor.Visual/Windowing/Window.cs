using Rendor.Visual.Drawing;

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
            OnRender(nativeWindow.Surface);
            nativeWindow.SwapBuffers();
        }

        private NativeWindow nativeWindow;
    }
}
