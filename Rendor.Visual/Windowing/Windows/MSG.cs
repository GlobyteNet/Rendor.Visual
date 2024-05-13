using System.Runtime.InteropServices;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the winuser.h MSG structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct MSG
{
    public nint hwnd;
    public uint message;
    public nuint wParam;
    public nint lParam;
    public uint time;
    public POINT pt;
}
