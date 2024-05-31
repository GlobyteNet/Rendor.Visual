using System.Runtime.InteropServices;

using HDC = nint;
using BOOL = int;
using BYTE = byte;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the winuser.h PAINTSTRUCT structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct PAINTSTRUCT
{
    public HDC hdc;
    public BOOL fErase;
    public RECT rcPaint;
    public BOOL fRestore;
    public BOOL fIncUpdate;
    unsafe public fixed BYTE rgbReserved[32];
}
