using System.Runtime.InteropServices;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the winuser.h WNDCLASS structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
struct WNDCLASS
{
    public uint style;
    public nint lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public nint hInstance;
    public nint hIcon;
    public nint hCursor;
    public nint hbrBackground;
    public string lpszMenuName;
    public string lpszClassName;
}
