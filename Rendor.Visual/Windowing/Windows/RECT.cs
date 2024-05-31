using System.Runtime.InteropServices;

using LONG = int;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the windef.h RECT structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct RECT
{
    public LONG Left, Top, Right, Bottom;
}
