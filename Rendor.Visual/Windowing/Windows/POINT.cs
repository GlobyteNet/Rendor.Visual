using System.Runtime.InteropServices;

using LONG = int;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the windef.h POINT structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct POINT
{
    public LONG X, Y;
}
