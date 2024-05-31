namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Represents the flags for the pixel format descriptor.
/// </summary>
internal enum DWFlags : uint
{
    PFD_DRAW_TO_WINDOW = 0x00000004,
    PFD_SUPPORT_OPENGL = 0x00000020,
    PFD_DOUBLEBUFFER = 0x00000001
}