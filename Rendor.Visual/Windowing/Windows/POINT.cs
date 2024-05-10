using System.Runtime.InteropServices;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the windef.h POINT structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    // note: in windef.h, the fields are LONG.
    public int X, Y;
}
