using System.Runtime.InteropServices;

namespace Rendor.Visual.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2f
    {
        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
