using System.Runtime.InteropServices;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the windef.h RECT structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int Left, Top, Right, Bottom;
}
