using System.Runtime.InteropServices;

using UINT = uint;
using WNDPROC = nint;
using HINSTANCE = nint;
using HICON = nint;
using HCURSOR = nint;
using HBRUSH = nint;
using LPCSTR = string;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the winuser.h WNDCLASS structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct WNDCLASS
{
    public UINT style;
    public WNDPROC lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public HINSTANCE hInstance;
    public HICON hIcon;
    public HCURSOR hCursor;
    public HBRUSH hbrBackground;
    public LPCSTR lpszMenuName;
    public LPCSTR lpszClassName;
}
