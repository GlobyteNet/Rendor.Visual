using System.Runtime.InteropServices;

using WORD = ushort;
using DWORD = uint;
using BYTE = byte;

namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the wingdi.h PIXELFORMATDESCRIPTOR structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct PIXELFORMATDESCRIPTOR
{
    public WORD nSize;
    public WORD nVersion;
    public DWORD dwFlags;
    public BYTE iPixelType;
    public BYTE cColorBits;
    public BYTE cRedBits;
    public BYTE cRedShift;
    public BYTE cGreenBits;
    public BYTE cGreenShift;
    public BYTE cBlueBits;
    public BYTE cBlueShift;
    public BYTE cAlphaBits;
    public BYTE cAlphaShift;
    public BYTE cAccumBits;
    public BYTE cAccumRedBits;
    public BYTE cAccumGreenBits;
    public BYTE cAccumBlueBits;
    public BYTE cAccumAlphaBits;
    public BYTE cDepthBits;
    public BYTE cStencilBits;
    public BYTE cAuxBuffers;
    public BYTE iLayerType;
    public BYTE bReserved;
    public DWORD dwLayerMask;
    public DWORD dwVisibleMask;
    public DWORD dwDamageMask;
}
