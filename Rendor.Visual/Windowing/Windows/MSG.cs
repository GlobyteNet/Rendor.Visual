using System.Runtime.InteropServices;

using HWND = nint;
using UINT = uint;
using WPARAM = nuint;
using LPARAM = nint;
using DWORD = uint;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the winuser.h MSG structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct MSG
{
    public HWND hwnd;
    public UINT message;
    public WPARAM wParam;
    public LPARAM lParam;
    public DWORD time;
    public POINT pt;
}
